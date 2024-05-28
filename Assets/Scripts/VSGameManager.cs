using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public enum VSGameState
{
    Enter,
    GamePlay,
    Victory,
    Lose
}

public class VSGameManager : MonoBehaviour
{
    public static VSGameManager Instance;
    [Header("Script")]
    public MapPool s_MapPool;
    [Header("Player")]
    [SerializeField] private GameObject _mainPlayer;
    [SerializeField] private List<GameObject> _playerList;
    [Header("Prefab")]
    public GameObject BotPrefab;
    [Header("Team")]
    public Transform TeamASpawn;
    public Transform TeamBSpawn;
    public Transform DeathMatchSpawn;
    private VSTeam _teamAlly;
    private VSTeam _teamEnemy;
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
    public string Mode;

    //Equipemnent
    public VSEquipmentPool EquipmentPool;
    public VSTeamSide Teamwin;
    //
    private float _reviveDelayTime = 3f;
    //Bools
    private bool _isEndGame = false;

    //Getter/Setter
    public List<GameObject> PlayerList { get => _playerList;}
    public bool IsEndGame { get => _isEndGame; }
    public VSTeam TeamAlly { get => _teamAlly; }
    public VSTeam TeamEnmey { get => _teamEnemy; }
    private void Awake()
    {
        Instance = this;
        Init();
    }
    // Start is called before the first frame update
    void Start()
    {
        GameState = VSGameState.Enter;
        UpdateGameState(VSGameState.Enter);
    }

    // Update is called once per frame
    void Update()
    {
        HandleMode(Mode);
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
        _teamAlly = new VSTeam(VSTeamSide.TeamAlly, 5, 0);
        _teamEnemy = new VSTeam(VSTeamSide.TeamEnemy, 5, 0);
        PlayerEventListener.MainPlayer = _mainPlayer;
    }
    void UpdateGameState(VSGameState newState)
    {
        GameState = newState;
        switch (newState)
        {
            case VSGameState.Enter:
                LoadPlayerEquipment();
                EnterMode(VSGlobals.MODE);
                break;
            case VSGameState.Victory:
                break;
            case VSGameState.Lose:
                break;
        }
        OnGameStateChange?.Invoke(newState);
    }

    void HandleMode(string mode)
    {
        switch (mode)
        {
            case "Domination":
                if (!_isEndGame)
                {
                    if (_teamAlly.Score >= VSGlobals.DOMINATION_MAX_SCORE || _teamEnemy.Score >= VSGlobals.DOMINATION_MAX_SCORE)
                    {
                        OnEndGame();
                    }
                }
                break;
            case "Deathmatch":

                break;
        }
    }
    void EnterMode(string mode)
    {
        Mode = mode;
        SetUpMapPick();
        //Load Player
        switch (mode)
        {
            case "Domination":
                SpawnTeamAlly();
                SpawnTeamEnemy();
                _mapPick.ActiveZone();
                VSInGameUIScript.instance.LoadUIDominationMode();
                break;
            case "Deathmatch":
                SpawnDeathMatch();
                _mapPick.DeactiveZone();
                VSInGameUIScript.instance.LoadUIDeathMatchMode();
                break;
        }

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
        VSInGameUIScript.instance.SetLargeMapImage(_mapPick.LargeMapSprite);
    }
    void SpawnDeathMatch()
    {
        VSTeam teamDeathmatch = new VSTeam(VSTeamSide.NoSide, 10, 0);
        _mainPlayer.transform.position = DeathMatchSpawn.GetChild(UnityEngine.Random.Range(0, DeathMatchSpawn.childCount)).position;
        _playerList.Add(_mainPlayer);
        teamDeathmatch.AddMember(_mainPlayer);
        for (int i = 1; i <= teamDeathmatch.Size - 1; i++)
        {
            GameObject bot = Instantiate(BotPrefab, DeathMatchSpawn.GetChild(i).position, Quaternion.identity);
            _playerList.Add(bot);
            teamDeathmatch.AddMember(bot);
        }
    }
    void SpawnTeamAlly()
    {
        _teamAlly.AddMember(_mainPlayer);
        _playerList.Add(_mainPlayer);
        _mainPlayer.transform.position = TeamASpawn.transform.GetChild(0).position;
        for(int i = 1; i <= _teamAlly.Size - 1; i++)
        {
            GameObject bot = Instantiate(BotPrefab, TeamASpawn.transform.GetChild(i).position, Quaternion.identity);
            bot.GetComponent<VSBotController>().ZonePoints = new List<Transform>(_mapPick.ZonesPositionDominationMode);
            _teamAlly.AddMember(bot);
            _playerList.Add(bot);
        }
    }

    void SpawnTeamEnemy()
    {
        for (int i = 0; i < _teamEnemy.Size; i++)
        {
            GameObject bot = Instantiate(BotPrefab, TeamBSpawn.transform.GetChild(i).position, Quaternion.identity);
            bot.GetComponent<VSBotController>().ZonePoints = new List<Transform>(_mapPick.ZonesPositionDominationMode);
            _teamEnemy.AddMember(bot);
            _playerList.Add(bot);
        }
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
        //Remove member in zones
        foreach (Transform zone in Zones) zone.GetComponent<VSZoneController>().RemoveMemberInZoneWhenDead(player);
        player.SetActive(false);
        if (player.gameObject.CompareTag("Player"))
        {
            DeathCamera.SetActive(true);
            VSInGameUIScript.instance.LoadPLayerDeadUI();
        }
        yield return new WaitForSeconds(_reviveDelayTime);
        if (Mode == "Domination")
        {
            if (player.GetComponent<VSPlayerInfo>().Team.TeamSide == VSTeamSide.TeamAlly) player.transform.position = TeamASpawn.GetChild(UnityEngine.Random.Range(0, _teamMembers)).position;
            else player.transform.position = TeamBSpawn.GetChild(UnityEngine.Random.Range(0, _teamMembers)).position;
        }
        else player.transform.position = DeathMatchSpawn.GetChild(UnityEngine.Random.Range(0, DeathMatchSpawn.childCount)).position;
        player.SetActive(true);
        player.GetComponent<VSPlayerInfo>().HP = 100;
        player.GetComponent<CharacterController>().height = 3f;
        if (player.gameObject.CompareTag("Player"))
        {
            DeathCamera.SetActive(false);
            player.GetComponent<VSPlayerController>().ResetWeapon();
            VSInGameUIScript.instance.LoadPlayerAfterReviveUI();
        }
        else player.GetComponent<VSBotController>().SearchWalkPoint();
    }
    public void UpdateTeamScore(VSTeamSide team, int scoreBonus)
    {
        if (team == VSTeamSide.TeamAlly) _teamAlly.Score = Mathf.Min(_teamAlly.Score + scoreBonus, VSGlobals.DOMINATION_MAX_SCORE);
        else _teamEnemy.Score = Mathf.Min(_teamEnemy.Score + scoreBonus, VSGlobals.DOMINATION_MAX_SCORE);
        VSInGameUIScript.instance.UpdateTeamScoreBar(_teamAlly.Score, _teamEnemy.Score);
    }

    public void OnEndGame()
    {
        _isEndGame = true;
        DeathCamera.SetActive(true);
        if (Mode == "Domination")
        {
            Teamwin = _teamAlly.Score >= _teamEnemy.Score ? VSTeamSide.TeamAlly : VSTeamSide.TeamEnemy;
        }

        //Deactive player
        foreach (var player in _playerList) player.SetActive(false);
        VSInGameUIScript.instance.LoadUIEndGame();
    }

}