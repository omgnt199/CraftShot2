using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VSZoneController : MonoBehaviour
{
    public VSTeamSide TeamOccupying;
    //public float timeToOcuppyZone = 5f;
    private bool isOccppied = false;
    public List<GameObject> MembersAllyInZone;
    public List<GameObject> MembersEnemyInZone;

    private int scoreToOccupyZone = 100;
    private float scoreOccupyTeamAlly = 0f;
    private float scoreOccupyTeamEnemy = 0f;
    private float zoneTimer = 0f;
    private int scorePerSecond = 1;

    public Image[] ZoneImgTeamAlly;
    public Image[] ZoneImgTeamEnemy;

    [Header("Zone Effect")]
    public GameObject ZoneNoTeamEffect;
    public GameObject ZoneTeamAllyEffect;
    public GameObject ZoneTeamEnemyEffect;
    private GameObject _currentZoneEffect;

    [SerializeField] private DominationModeSO _dominationMode;
    // Start is called before the first frame update
    void Start()
    {
        _currentZoneEffect = ZoneNoTeamEffect;
        MembersAllyInZone = new List<GameObject>();
        MembersEnemyInZone = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Members A :" + MembersAInZone.Count + ", Members B :" + MembersBInZone.Count);
        if (!GameManager.Instance.IsEndGame)
        {
            //Caculate score (different score team) each team to occupy this zone
            scoreOccupyTeamAlly += Time.deltaTime * Mathf.Min(3, (MembersAllyInZone.Count - MembersEnemyInZone.Count)) * 5f;
            scoreOccupyTeamEnemy += Time.deltaTime * Mathf.Min(3, (MembersEnemyInZone.Count - MembersAllyInZone.Count)) * 5f;

            //Determine team occupy this zone
            if (scoreOccupyTeamAlly < 0) scoreOccupyTeamAlly = 0;
            if (scoreOccupyTeamAlly > 100) scoreOccupyTeamAlly = 100;
            if (scoreOccupyTeamEnemy < 0) scoreOccupyTeamEnemy = 0;
            if (scoreOccupyTeamEnemy > 100) scoreOccupyTeamEnemy = 100;
            if (scoreOccupyTeamAlly == 100 && TeamOccupying != VSTeamSide.TeamAlly)
            {
                scoreOccupyTeamEnemy = 0;
                TeamOccupying = VSTeamSide.TeamAlly;
                isOccppied = true;
                _currentZoneEffect.SetActive(false);
                ZoneTeamAllyEffect.SetActive(true);
                _currentZoneEffect = ZoneTeamAllyEffect;
            }
            else if (scoreOccupyTeamEnemy == 100 && TeamOccupying != VSTeamSide.TeamEnemy)
            {
                scoreOccupyTeamAlly = 0;
                TeamOccupying = VSTeamSide.TeamEnemy;
                isOccppied = true;
                _currentZoneEffect.SetActive(false);
                ZoneTeamEnemyEffect.SetActive(true);
                _currentZoneEffect = ZoneTeamEnemyEffect;
            }
            //Update UI
            foreach (Image zoneimg in ZoneImgTeamAlly) zoneimg.fillAmount = scoreOccupyTeamAlly / scoreToOccupyZone;
            foreach (Image zoneimg in ZoneImgTeamEnemy) zoneimg.fillAmount = scoreOccupyTeamEnemy / scoreToOccupyZone;
            //Calculate member's point
            if (isOccppied)
            {
                zoneTimer += Time.deltaTime;
                if (zoneTimer >= 1f)
                {
                    _dominationMode.OnUpdateTeamScore?.Invoke(TeamOccupying, scorePerSecond);
                    if (TeamOccupying == VSTeamSide.TeamAlly)
                    {
                        foreach (var member in MembersAllyInZone)
                            member.GetComponent<VSPlayerInfo>().Points += 2;
                    }
                    else if (TeamOccupying == VSTeamSide.TeamEnemy)
                    {
                        foreach (var member in MembersEnemyInZone)
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
                if (!MembersAllyInZone.Contains(member))
                    MembersAllyInZone.Add(member);
            }
            else if (member.GetComponent<VSPlayerInfo>().Team.TeamSide == VSTeamSide.TeamEnemy)
            {
                if (!MembersEnemyInZone.Contains(member))
                    MembersEnemyInZone.Add(member);
            }
            //
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //
        GameObject member = other.GetComponentInParent<VSPlayerInfo>().gameObject;
        RemoveMemberInZoneWhenDead(member);
        //
    }

    public void RemoveMemberInZoneWhenDead(GameObject member)
    {
        MembersAllyInZone.Remove(member);
        MembersEnemyInZone.Remove(member);
    }
}
