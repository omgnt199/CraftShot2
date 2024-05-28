using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Linq;

public class LeaderBoardController : MonoBehaviour
{
    public static LeaderBoardController instance;
    [SerializeField] GameObject Board, RankBoard, PlayerScoreBoard, PlayerScoreRowPrefab, RankRowPrefab;
    public bool UpdatingLeaderBoard = true;
    private Vector2[] playerScoreRowPosition;
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!UpdatingLeaderBoard)
            UpdateLeaderBoard();
    }
    void UpdateLeaderBoard()
    {
        var sequence = DOTween.Sequence();
        int N = PlayerScoreBoard.transform.childCount;
        Dictionary<GameObject, int> scorePlayerDict = new Dictionary<GameObject, int>();
        Dictionary<GameObject, int> scorePLayerIndexDict = new Dictionary<GameObject, int>();
        for (int i = 0; i < N; i++)
        {
            int score = System.Int32.Parse(PlayerScoreBoard.transform.GetChild(i).Find("Score").GetComponent<Text>().text);
            scorePlayerDict[PlayerScoreBoard.transform.GetChild(i).gameObject] = score;
            scorePLayerIndexDict[PlayerScoreBoard.transform.GetChild(i).gameObject] = i;
        }
        //Debug.Log(scorePlayerDict.Count);
        int index = 0;
        foreach (KeyValuePair<GameObject, int> scorePlayer in scorePlayerDict.OrderByDescending(key => key.Value))
        {
            scorePlayer.Key.transform.SetSiblingIndex(index);
            index++;
        }
        StartCoroutine(WaitForRenderLeaderBoardUI());
        StartCoroutine(WaitForUpdateLeaderBoard());
    }
    IEnumerator WaitForRenderLeaderBoardUI()
    {
        yield return null;
        SetUpLeaderBoard();
    }
    IEnumerator WaitForUpdateLeaderBoard()
    {
        UpdatingLeaderBoard = true;
        yield return new WaitForSeconds(0.2f);
        UpdatingLeaderBoard = false;
    }
    public GameObject ModifyPlayerScoreRow(int rank, string playerName)
    {
        GameObject scoreRow = Instantiate(PlayerScoreRowPrefab, PlayerScoreBoard.transform);
        GameObject rankRow = Instantiate(RankRowPrefab, RankBoard.transform);
        scoreRow.transform.Find("Name").GetComponent<Text>().text = playerName;
        rankRow.transform.Find("Rank").GetComponent<Text>().text = "#" + rank;
        if (PlayerScoreBoard.transform.childCount > 1) rankRow.transform.Find("Crown").GetComponent<Image>().enabled = false;
        if (playerName == "You")
        {
            scoreRow.transform.GetComponent<Image>().color = new Color32(152, 152, 152, 90);
            rankRow.transform.GetComponent<Image>().color = new Color32(152, 152, 152, 90);
        }
        return scoreRow;
    }
    public void SetUpLeaderBoard()
    {
        int N = PlayerScoreBoard.transform.childCount;
        for(int i = 0; i < N; i++)
        {
            if (i <= 3 || i >= N - 1)
            {
                RankBoard.transform.GetChild(i).gameObject.SetActive(true);
                PlayerScoreBoard.transform.GetChild(i).gameObject.SetActive(true);
                PlayerScoreBoard.transform.GetChild(i).Find("Name").GetComponent<Text>().enabled = true;
                PlayerScoreBoard.transform.GetChild(i).Find("Score").GetComponent<Text>().enabled = true;
                PlayerScoreBoard.transform.GetChild(i).GetComponent<Image>().color = new Color32(152, 152, 152, 0);
                RankBoard.transform.GetChild(i).GetComponent<Image>().color = new Color32(152, 152, 152, 0);
                if (PlayerScoreBoard.transform.GetChild(i).Find("Name").GetComponent<Text>().text == "You" && i !=3)
                {
                    PlayerScoreBoard.transform.GetChild(i).GetComponent<Image>().color = new Color32(152, 152, 152, 90);
                    RankBoard.transform.GetChild(i).GetComponent<Image>().color = new Color32(152, 152, 152, 90);
                }
            }
            else
            {
                RankBoard.transform.GetChild(i).gameObject.SetActive(false);
                PlayerScoreBoard.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        RankBoard.transform.GetChild(3).Find("Rank").GetComponent<Text>().enabled = false;
        PlayerScoreBoard.transform.GetChild(3).Find("Name").GetComponent<Text>().enabled = false;
        PlayerScoreBoard.transform.GetChild(3).Find("Score").GetComponent<Text>().enabled = false;
    }
    public void UpdatePlayerScore(GameObject PlayerScoreRow, int score)
    {
        PlayerScoreRow.GetComponent<PlayerScoreRow>().UpdateScore(score);
    }
}
