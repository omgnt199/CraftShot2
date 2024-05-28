using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "MiniGameTimePlay",menuName ="MinigameScriptable/TimePlay")]
public class TimePlayMgr : ScriptableObject
{
    public MiniGame MiniGame;
    public Dictionary<string, int> TimePlayModeDict;
    public string Level;
    public float TimePlay;
    public float MarkTime;
    private bool _isMark;
    public void Initialize()
    {
        _isMark = false;
        TimePlayModeDict = new Dictionary<string, int>();
        TimePlay = 0;
        MarkTime = 0;
    }
    public void MarkTimeStart()
    {
        _isMark = true;
        MarkTime = GlobalData.CurrentTimeApplication;
    }
    public void UpdateTimePlay()
    {
        if (_isMark)
        {
            _isMark = false;
            TimePlay += (GlobalData.CurrentTimeApplication - MarkTime);
            Debug.Log(TimePlay);
        }
    }

    public void Tracking()
    {
        if (TimePlay > 0)
        {
            string timePlay = "0";
            float mins = TimePlay / 60f;
            if (mins > 0 && mins < 2f) timePlay = "<2 min";
            else if (mins >= 2f && mins < 5) timePlay = "2-5 min";
            else if (mins >= 5 && mins < 10) timePlay = "5-10 min";
            else if (mins >= 10 && mins < 20) timePlay = "10-20 mins";
            else if (mins >= 20) timePlay = "20+ min";
            string modePlay;
            if (TimePlayModeDict.Count > 0)
            {
                modePlay = "[";
                int index = 0;
                foreach (var it in TimePlayModeDict)
                {
                    string mode = "{\"modeName\":" + "\"" + it.Key + "\",\"time\":" + it.Value.ToString() + "}";
                    modePlay += mode;
                    if (index < TimePlayModeDict.Count - 1) modePlay += ",";
                    index++;
                }
                modePlay += "]";
            }
            else modePlay = "";
            Debug.Log(MiniGame.ToString() + "," + Level + "," + modePlay + "," + timePlay);
            //ServiceManager.TryLog("Mini_game", new Parameter[] { new("name", MiniGame.ToString()), new("level", Level), new("mode", modePlay), new("playtime", timePlay) });
        }
    }

}
