using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VSPlayerInfo : MonoBehaviour
{
    [Header("Player Info")]
    public string Name;
    public int HP = 100;
    public int Kills = 0;
    public int Deaths = 0;
    public int Points = 0;
    [Header("Team Info")]
    public TextMeshProUGUI NameTxt;
    public VSTeam Team;
    public Image TeamArrow;
    public Sprite TeamAlly_Arrow_Sprite;
    public Sprite TeamEnemy_Arrow_Sprite;
    public Image TeamGPSImg;
    public Sprite TeamAlly_GPS_Sprite;
    public Sprite TeamEnemy_GPS_Sprite;
    // Start is called before the first frame update
    void Start()
    {
        if (!gameObject.CompareTag("Player"))
        {
            Name = NVJOBNameGen.GiveAName(Random.Range(1, 4));
            Name = Name.Substring(0, this.Name.IndexOf(" "));
            NameTxt.text = Name;
        }
        else Name = "You";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetTeam(VSTeam team)
    {
        Team = team;
        //TeamName.text = team.ToString();
        if (!gameObject.CompareTag("Player"))
        {
            if (team.TeamSide == VSTeamSide.TeamAlly)
            {
                NameTxt.color = new Color32(21, 166, 251, 255);
                TeamArrow.sprite = TeamAlly_Arrow_Sprite;
                TeamGPSImg.sprite = TeamAlly_GPS_Sprite;
            }
            else
            {
                NameTxt.color = new Color32(252, 40, 40, 255);
                TeamArrow.sprite = TeamEnemy_Arrow_Sprite;
                TeamGPSImg.sprite = TeamEnemy_GPS_Sprite;
            }
        }
    }
    public void UpdateHP(int delta)
    {
        HP = Mathf.Max(HP + delta, 0);
        if(gameObject.CompareTag("Player"))
        {
            VSInGameUIScript.instance.UpdatePlayerHPUI(HP);
        }
    }
    public void OnDeath()
    {
        Deaths++;
        VSGameManager.Instance.OnOnePlayerDead(gameObject);
    }
}
