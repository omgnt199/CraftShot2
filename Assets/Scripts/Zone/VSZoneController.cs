using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VSZoneController : MonoBehaviour
{
    public VSTeamSide TeamOccupying;
    //public float timeToOcuppyZone = 5f;
    private bool isOccppied = false;
    public List<GameObject> MembersAInZone;
    public List<GameObject> MembersBInZone;

    private int scoreToOccupyZone = 100;
    private float scoreOccupyTeamA = 0f;
    private float scoreOccupyTeamB = 0f;
    private float zoneTimer = 0f;
    private int scorePerSecond = 2;

    public Image[] ZoneImgTeamA;
    public Image[] ZoneImgTeamB;

    [Header("Zone Effect")]
    public GameObject ZoneNoTeamEffect;
    public GameObject ZoneTeamAEffect;
    public GameObject ZoneTeamBEffect;
    private GameObject _currentZoneEffect;
    // Start is called before the first frame update
    void Start()
    {
        _currentZoneEffect = ZoneNoTeamEffect;
        MembersAInZone = new List<GameObject>();
        MembersBInZone = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Members A :" + MembersAInZone.Count + ", Members B :" + MembersBInZone.Count);
        if (!VSGameManager.Instance.IsEndGame)
        {
            //Caculate score (different score team) each team to occupy this zone
            scoreOccupyTeamA += Time.deltaTime * Mathf.Min(3, (MembersAInZone.Count - MembersBInZone.Count)) * 10f;
            scoreOccupyTeamB += Time.deltaTime * Mathf.Min(3, (MembersBInZone.Count - MembersAInZone.Count)) * 10f;

            //Determine team occupy this zone
            if (scoreOccupyTeamA < 0) scoreOccupyTeamA = 0;
            if (scoreOccupyTeamA > 100) scoreOccupyTeamA = 100;
            if (scoreOccupyTeamB < 0) scoreOccupyTeamB = 0;
            if (scoreOccupyTeamB > 100) scoreOccupyTeamB = 100;
            if (scoreOccupyTeamA == 100 && TeamOccupying != VSTeamSide.TeamAlly)
            {
                scoreOccupyTeamB = 0;
                TeamOccupying = VSTeamSide.TeamAlly;
                isOccppied = true;
                _currentZoneEffect.SetActive(false);
                ZoneTeamAEffect.SetActive(true);
                _currentZoneEffect = ZoneTeamAEffect;
            }
            else if (scoreOccupyTeamB == 100 && TeamOccupying != VSTeamSide.TeamEnemy)
            {
                scoreOccupyTeamA = 0;
                TeamOccupying = VSTeamSide.TeamEnemy;
                isOccppied = true;
                _currentZoneEffect.SetActive(false);
                ZoneTeamBEffect.SetActive(true);
                _currentZoneEffect = ZoneTeamBEffect;
            }
            //Update UI
            foreach (var zoneimg in ZoneImgTeamA) zoneimg.fillAmount = scoreOccupyTeamA / scoreToOccupyZone;
            foreach (var zoneimg in ZoneImgTeamB) zoneimg.fillAmount = scoreOccupyTeamB / scoreToOccupyZone;
            //Calculate member's point
            if (isOccppied)
            {
                zoneTimer += Time.deltaTime;
                if (zoneTimer >= 1f)
                {
                    VSGameManager.Instance.UpdateTeamScore(TeamOccupying, scorePerSecond);
                    if (TeamOccupying == VSTeamSide.TeamAlly)
                    {
                        foreach (var member in MembersAInZone)
                            member.GetComponent<VSPlayerInfo>().Points += 2;
                    }
                    else if (TeamOccupying == VSTeamSide.TeamEnemy)
                    {
                        foreach (var member in MembersBInZone)
                            member.GetComponent<VSPlayerInfo>().Points += 2;
                    }
                    zoneTimer = 0;
                }
            }
            else zoneTimer = 0;
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Bot"))
        {
            //
            GameObject member = other.gameObject;
            //Debug.Log(member);
            if (member.GetComponent<VSPlayerInfo>().Team.TeamSide == VSTeamSide.TeamAlly)
            {
                if (!MembersAInZone.Contains(member))
                    MembersAInZone.Add(member);
            }
            else
            {
                if (!MembersBInZone.Contains(member))
                    MembersBInZone.Add(member);
            }
            //
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //
        GameObject member = other.gameObject;
        if (member.GetComponent<VSPlayerInfo>().Team.TeamSide == VSTeamSide.TeamAlly) MembersAInZone.Remove(member);
        else MembersBInZone.Remove(member);
        //
    }

    public void RemoveMemberInZoneWhenDead(GameObject member)
    {
        MembersAInZone.Remove(member);
        MembersBInZone.Remove(member);
    }
}
