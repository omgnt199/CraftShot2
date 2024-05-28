using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum KillType
{
    None,
    HeadShot,
    Blind
}
public class VSKillReportUI : MonoBehaviour
{
    public TextMeshProUGUI P1;
    public TextMeshProUGUI P2;
    public Image WeaponIcon;
    public Image DeathTypeIcon;

    public Sprite HeadShotKill_Sprite;
    public Sprite BlindKill_Sprite;
    private Dictionary<KillType, Sprite> _dictSpecialKillIcon;
    private Color teamAColor;
    private Color teamBColor;
    private void Awake()
    {
        _dictSpecialKillIcon = new Dictionary<KillType, Sprite>()
        {
            {KillType.None, null},
            {KillType.HeadShot, HeadShotKill_Sprite},
            {KillType.Blind, BlindKill_Sprite}
        };

        teamAColor = new Color32(75, 115, 207, 255); 
        teamBColor = new Color32(255, 102, 102, 255);
    }
    public void SetKillReport(string p1,VSTeamSide p1Team,string p2, VSTeamSide p2Team, Sprite weaponIcon, KillType specialKill)
    {
        P1.text = p1;
        P2.text = p2;
        if (p1Team == VSTeamSide.TeamAlly) P1.color = teamAColor;
        else P1.color = teamBColor;
        if (p2Team == VSTeamSide.TeamAlly) P2.color = teamAColor;
        else P2.color = teamBColor;
        if(p2Team == VSTeamSide.NoSide)
        {
            P1.color = teamAColor;
            P2.color = teamBColor;
        }

        WeaponIcon.sprite = weaponIcon;
        DeathTypeIcon.sprite = _dictSpecialKillIcon[specialKill];

        DeathTypeIcon.gameObject.SetActive(!(specialKill == KillType.None));
        Invoke(nameof(DestroyUI), 3f);
    }
    void DestroyUI()
    {
        Destroy(gameObject);
    }
}
