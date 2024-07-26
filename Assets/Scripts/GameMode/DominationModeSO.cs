using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(fileName = "DominationMode",menuName ="ScriptableObject/GameMode/Domination")]
public class DominationModeSO : GameModeSO
{
    public int ModeTime;
    public Team TeamAlly;
    public Team TeamEnemy;
    public int TargetScore;
    public GameObject BotPrefab;
    [SerializeField] private CounterSO _Counter;
    [SerializeField] private VoidEventChannelSO _updateCounterUIEvent;

    private GameObject _mainPlayer;
    private Map _mapPick;
    private Team _teamWin;
    private float timer = 0;

    public UnityAction<VSTeamSide, int> OnUpdateTeamScore;

    public override void EnterMode()
    {
        OnUpdateTeamScore += UpdateTeamScore;
        _mainPlayer = GameManager.Instance.MainPlayer;
        _mapPick = GameManager.Instance.MapPick;

        TeamAlly = new Team(VSTeamSide.TeamAlly, 5, 0);
        TeamEnemy = new Team(VSTeamSide.TeamEnemy, 5, 0);
        _teamWin = null;

        SpawnTeamAlly();
        SpawnTeamEnemy();
        _mapPick.ActiveZone();

        _Counter.SetInitialTime(ModeTime);
        _Counter.SetCurrentTime(ModeTime);
        VSInGameUIScript.instance.LoadUIDominationMode();
    }
    public override void UpdateMode()
    {
        UpdateCounter();
        if (TeamAlly.Score >= TargetScore)
        {
            _teamWin = TeamAlly;
            EndMode();
        }
        else if (TeamEnemy.Score >= TargetScore)
        {
            _teamWin = TeamEnemy;
            EndMode();
        }

        if (_Counter.CurrentTime == 0)
        {
            _teamWin = Mathf.Max(TeamAlly.Score, TeamEnemy.Score) == TeamAlly.Score ? TeamAlly : TeamEnemy;
            EndMode();
        }

    }

    public override void EndMode()
    {
        OnUpdateTeamScore -= UpdateTeamScore;
        GameManager.Instance.OnEndGame();
    }
    void UpdateCounter()
    {
        timer += Time.deltaTime;
        if (timer >= 1f)
        {
            timer = 0f;
            _Counter.UpdateCurrentTime(1);
            _updateCounterUIEvent.RaiseEvent();
        }
    }

    void SpawnTeamAlly()
    {
        TeamAlly.AddMember(GameManager.Instance.MainPlayer);
        _mainPlayer.transform.position = _mapPick.FirstTeamSpawn.transform.GetChild(0).position;
        for (int i = 1; i <= TeamAlly.Size - 1; i++)
        {
            GameObject bot = Instantiate(BotPrefab, _mapPick.FirstTeamSpawn.transform.GetChild(i).position, Quaternion.identity);
            bot.GetComponent<VSBotController>().ZonePoints = new List<Transform>(_mapPick.ZonesPositionDominationMode);
            bot.GetComponent<VSBotController>().SetCharacterSkin(GlobalData.Instance.EquipmentPool.GetEquipmentByName("SkinPolice").Model);
            TeamAlly.AddMember(bot);
            GameManager.Instance.PlayerList.Add(bot);
        }
    }

    void SpawnTeamEnemy()
    {
        for (int i = 0; i < TeamEnemy.Size; i++)
        {
            GameObject bot = Instantiate(BotPrefab, _mapPick.SecondTeamSpawn.transform.GetChild(i).position, Quaternion.identity);
            bot.GetComponent<VSBotController>().ZonePoints = new List<Transform>(_mapPick.ZonesPositionDominationMode);
            bot.GetComponent<VSBotController>().SetCharacterSkin(GlobalData.Instance.EquipmentPool.GetEquipmentByName("SkinThief").Model);
            TeamEnemy.AddMember(bot);
            GameManager.Instance.PlayerList.Add(bot);
        }
    }
    public void UpdateTeamScore(VSTeamSide team, int scoreBonus)
    {
        if (team == VSTeamSide.TeamAlly) TeamAlly.Score = Mathf.Min(TeamAlly.Score + scoreBonus, TargetScore);
        else TeamEnemy.Score = Mathf.Min(TeamEnemy.Score + scoreBonus, TargetScore);
        VSInGameUIScript.instance.UpdateTeamScoreBar(TeamAlly.Score, TeamEnemy.Score);
    }
}
