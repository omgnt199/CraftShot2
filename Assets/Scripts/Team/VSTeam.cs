using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VSTeam
{
    public VSTeamSide TeamSide;
    public int Size { get; set; }
    public int Score { get; set; }
    public List<GameObject> MemberList { get; set; }
    public VSTeam(VSTeamSide team,int size, int score)
    {
        TeamSide = team;
        Size = size;
        Score = score;
        MemberList = new List<GameObject>();
    }
    public VSTeam(VSTeamSide team,int size, int score, List<GameObject> memberList)
    {
        TeamSide = team;
        Size = size;
        Score = score;
        MemberList = new List<GameObject>(memberList);
    }
    public void AddMember(GameObject member)
    {
        MemberList.Add(member);
        member.GetComponent<VSPlayerInfo>().SetTeam(this);
    }
    public static bool operator ==(VSTeam first, VSTeam second)
    {
        return first.TeamSide == second.TeamSide && first.TeamSide != VSTeamSide.NoSide && second.TeamSide != VSTeamSide.NoSide;
    }
    public static bool operator !=(VSTeam first, VSTeam second)
    {
        return first.TeamSide != second.TeamSide || first.TeamSide == VSTeamSide.NoSide || second.TeamSide == VSTeamSide.NoSide;
    }
    public void UpdateScore(int delta) => Score += delta;

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return base.ToString();
    }
}
