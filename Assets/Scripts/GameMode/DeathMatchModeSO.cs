using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "DeathmatchMode", menuName = "ScriptableObject/GameMode/DeathMatch")]
public class DeathMatchModeSO : GameModeSO
{
    public int ModeTime;
    public GameObject BotPrefab;
    [SerializeField] private CounterSO _counterConfig;
    [SerializeField] private IntEventChanelSO _updateCounterUIEvent;

    private GameObject _mainPlayer;
    private Map _mapPick;
    private float timer = 0;
    public override void EnterMode()
    {
        _mainPlayer = GameManager.Instance.MainPlayer;
        _mapPick = GameManager.Instance.MapPick;
        _mapPick.DeactiveZone();
        SpawnDeathMatch();
        _counterConfig.SetInitialTime(ModeTime);
        _counterConfig.SetCurrentTime(ModeTime);
        VSInGameUIScript.instance.LoadUIDeathMatchMode();
    }


    public override void UpdateMode()
    {
        UpdateCounter();
        if (_counterConfig.CurrentTime == 0) EndMode();
    }
    public override void EndMode()
    {
        _updateCounterUIEvent.OnEventRaised = null;
        GameManager.Instance.OnEndGame();
    }
    void UpdateCounter()
    {
        timer += Time.deltaTime;
        if (timer >= 1f)
        {
            timer = 0f;
            _counterConfig.UpdateCurrentTime(1);
            _updateCounterUIEvent.RaiseEvent(_counterConfig.CurrentTime);
        }
    }
    public void SpawnDeathMatch()
    {
        Team teamDeathmatch = new Team(VSTeamSide.NoSide, 10, 0);
        _mainPlayer.transform.position = _mapPick.DeathmatchSpawn.GetChild(Random.Range(0, _mapPick.DeathmatchSpawn.childCount)).position;
        teamDeathmatch.AddMember(_mainPlayer);
        for (int i = 1; i <= teamDeathmatch.Size - 1; i++)
        {
            GameObject bot = Instantiate(BotPrefab, _mapPick.DeathmatchSpawn.GetChild(i).position, Quaternion.identity);
            bot.GetComponent<VSBotController>().SetCharacterSkin(GlobalData.Instance.EquipmentPool.GetRandomEquipmentByType(VSEquipmentType.Character).Model);
            GameManager.Instance.PlayerList.Add(bot);
            teamDeathmatch.AddMember(bot);
        }
    }
}
