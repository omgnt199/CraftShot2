using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.Burst.Intrinsics;
using Assets.Scripts.Common;

public enum VSGameState
{
    Enter,
    GamePlay,
    Victory,
    Lose
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [Header("Script")]
    public MapPool s_MapPool;
    [Header("Player")]
    [SerializeField] private GameObject _mainPlayer;
    [SerializeField] private List<GameObject> _playerList;
    [Header("Prefab")]
    public GameObject BotPrefab;
    [Header("Team")]
    [HideInInspector] public Transform TeamASpawn;
    [HideInInspector] public Transform TeamBSpawn;
    [HideInInspector] public Transform DeathMatchSpawn;
    private const int _teamMembers = 5;
    private const int _deathMatchMembers = 10;

    [Header("Zones")]
    public List<Transform> Zones;
    [Header("Camera")]
    public GameObject DeathCamera;
    public Camera LargeMapCamera;
    //MapPick
    private Map _mapPick;
    //GameState
    [HideInInspector] VSGameState GameState;
    public static event Action<VSGameState> OnGameStateChange;
    //Mode
    public GameModePoolSO ModePool;
    private GameModeSO _currentMode;
    public string Mode;

    //Equipemnent
    public VSEquipmentPool EquipmentPool;
    public ItemPowerPoolSO PowerPool;
    public Team Teamwin;
    //
    private float _reviveDelayTime = 3f;
    //Bools
    private bool _isEndGame = false;

    //Getter/Setter
    public GameObject MainPlayer => _mainPlayer;
    public List<GameObject> PlayerList { get => _playerList; }
    public bool IsEndGame { get => _isEndGame; }
    public Map MapPick => _mapPick;
    public GameModeSO CurrentMode => _currentMode;
    private void Awake()
    {
        Instance = this;
        Init();
    }
    // Start is called before the first frame update
    void Start()
    {



    }

    // Update is called once per frame
    void Update()
    {
        _currentMode.UpdateMode();
    }
    private void OnApplicationQuit()
    {
        //GlobalData.Instance.TimePlayManager.VoxelStrikeTimePlayMgr.UpdateTimePlay();
    }
    private void OnDisable()
    {
        //GlobalData.Instance.TimePlayManager.VoxelStrikeTimePlayMgr.UpdateTimePlay();
    }

    void Init()
    {
        _playerList = new List<GameObject>();
        PlayerEventListener.MainPlayer = _mainPlayer;
        _playerList.Add(_mainPlayer);
        SetUpMapPick();
        GameState = VSGameState.Enter;
        UpdateGameState(VSGameState.Enter);
    }
    void UpdateGameState(VSGameState newState)
    {
        GameState = newState;
        switch (newState)
        {
            case VSGameState.Enter:
                LoadPlayerEquipment();
                LoadMode(VSGlobals.MODE);
                break;
            case VSGameState.Victory:
                break;
            case VSGameState.Lose:
                break;
        }
        OnGameStateChange?.Invoke(newState);
    }

    void LoadMode(string mode)
    {
        Mode = mode;
        _currentMode = ModePool.GetGameModeByName(mode);
        _currentMode.EnterMode();
        //GlobalData.Instance.TimePlayManager.VoxelStrikeTimePlayMgr.MarkTimeStart();
        //GlobalData.Instance.TimePlayManager.VoxelStrikeTimePlayMgr.TimePlayModeDict[Mode]++;
    }
    void SetUpMapPick()
    {
        _mapPick = s_MapPool.PickRandomMap(Mode);
        TeamASpawn = _mapPick.FirstTeamSpawn;
        TeamBSpawn = _mapPick.SecondTeamSpawn;
        DeathMatchSpawn = _mapPick.DeathmatchSpawn;
        Zones = new List<Transform>(_mapPick.ZonesPositionDominationMode);
        LargeMapCamera.transform.localPosition = _mapPick.LargeMapCameraPosition;
        LargeMapCamera.orthographicSize = _mapPick.LargeMapCameraSize;
        //VSInGameUIScript.instance.SetLargeMapImage(_mapPick.LargeMapSprite);
    }
    public void LoadPlayerEquipment()
    {
        string primaryWPName = PlayerPrefs.GetString("VSPrimaryWeaponUsing");
        string secondaryWPName = PlayerPrefs.GetString("VSSecondaryWeaponUsing");
        string supportWPName = PlayerPrefs.GetString("VSSupportWeaponUsing");
        string nadeName = PlayerPrefs.GetString("VSNadeUsing");

        VSGun primaryWP;
        VSGun secondaryWP;
        VSSupportWeapon supportWP;
        VSNade nadeWP;

        if (primaryWPName == null || primaryWPName == "")
        {
            primaryWP = (VSGun)EquipmentPool.GetEquipmentByName("Scar");
            PlayerPrefs.SetString("VSPrimaryWeaponUsing", primaryWP.Name);
        }
        else primaryWP = (VSGun)EquipmentPool.GetEquipmentByName(primaryWPName);

        if (secondaryWPName == null || secondaryWPName == "") secondaryWP = (VSGun)EquipmentPool.GetEquipmentByName("Desert Eagle");
        else secondaryWP = (VSGun)EquipmentPool.GetEquipmentByName(secondaryWPName);

        if (supportWPName == null || supportWPName == "") supportWP = (VSSupportWeapon)EquipmentPool.GetEquipmentByName("Karambit");
        else supportWP = (VSSupportWeapon)EquipmentPool.GetEquipmentByName(supportWPName);

        if (nadeName == null || nadeName == "") nadeWP = (VSNade)EquipmentPool.GetEquipmentByName("Grenade");
        else nadeWP = (VSNade)EquipmentPool.GetEquipmentByName(nadeName);

        _mainPlayer.GetComponent<VSPlayerController>().SetUpWeapon(primaryWP, secondaryWP, supportWP, nadeWP);

    }
    public void OnOnePlayerDead(GameObject player)
    {
        StartCoroutine(DelayRevive(player));
    }
    IEnumerator DelayRevive(GameObject player)
    {
        yield return new WaitForSeconds(_reviveDelayTime);
        _currentMode.Revive(player);

    }
    public void OnEndGame()
    {
        _isEndGame = true;
        DeathCamera.SetActive(true);

        //Deactive player
        foreach (var player in _playerList) player.SetActive(false);
        VSInGameUIScript.instance.LoadUIEndGame();
    }

}