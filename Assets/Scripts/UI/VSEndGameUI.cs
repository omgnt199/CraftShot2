using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VSEndGameUI : MonoBehaviour
{
    public static VSEndGameUI instance;
    public TextMeshProUGUI TeamWinTxt;
    public TextMeshProUGUI YourKillsTxt;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetEndGameUI(VSTeamSide teamwin,int yourKills)
    {
        TeamWinTxt.text = teamwin.ToString();
        YourKillsTxt.text = yourKills.ToString();
    }
}
