using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "DominationMode",menuName ="ScriptableObject/GameMode/Domination")]
public class DominationModeSO : GameModeSO
{
    public int ModeTime;
    public Team TeamAlly;
    public Team TeamEnemy;
    public int TargetScore;
    private Team _teamWin;
    [SerializeField] private CounterSO _Counter;
    [SerializeField] private VoidEventChannelSO _updateCounterUIEvent;
    private float timer = 0;
    public override void EnterMode()
    {
        _teamWin = null;
        _Counter.SetInitialTime(ModeTime);
        _Counter.SetCurrentTime(ModeTime);
    }
    public override void UpdateMode()
    {
        UpdateCounter();
        if (TeamAlly.Score >= TargetScore) _teamWin = TeamAlly;
        else if (TeamEnemy.Score >= TargetScore) _teamWin = TeamEnemy;

        if (_teamWin != null)
        {
            _teamWin = Mathf.Max(TeamAlly.Score, TeamEnemy.Score) == TeamAlly.Score ? TeamAlly : TeamEnemy;
            EndMode();
        }

    }

    public override void EndMode()
    {
        VSGameManager.Instance.Teamwin = _teamWin;
        VSGameManager.Instance.OnEndGame();
    }
    void UpdateCounter()
    {
        timer += Time.deltaTime;
        if (timer >= 1f)
        {
            _Counter.UpdateCurrenTime(1);
            _updateCounterUIEvent.RaiseEvent();
        }
    }

}
