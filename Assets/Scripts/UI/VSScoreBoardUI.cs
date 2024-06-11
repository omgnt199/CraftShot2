using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.UI;


public enum VSScoreSortType
{
    Kills,
    Deaths,
    Points
}
public class VSScoreBoardUI : MonoBehaviour
{
    public static VSScoreBoardUI Instance;
    [Header("Script")]
    public List<VSPlayerInfo> PlayerList = new List<VSPlayerInfo>();
    public List<VSPlayerInfo> PlayersTeamA = new List<VSPlayerInfo>();
    public List<VSPlayerInfo> PlayersTeamB = new List<VSPlayerInfo>();
    [Header("UI")]
    [SerializeField] private GameObject LeafTable;
    [SerializeField] private GameObject RightTable;
    [SerializeField] private GameObject WinLose;
    //Text
    [SerializeField] private TextMeshProUGUI TabText;
    [SerializeField] private TextMeshProUGUI WinLoseTxt;
    [SerializeField] private TextMeshProUGUI CoinRewardTxt;
    //Sprite
    [SerializeField] private Sprite _mainPlayerBarSprite;
    [SerializeField] private Sprite _defaultPlayerBarSprite;
    [SerializeField] private Sprite _allyTeamBarSprite;
    [SerializeField] private Sprite _enemyTeamBarSprite;
    //Image
    [SerializeField] private Image _rightTeamBarImg;
    //
    private Dictionary<VSPlayerInfo, int> _playerTeamAValue;
    private Dictionary<VSPlayerInfo, int> _playerTeamBValue;
    private Dictionary<VSPlayerInfo, int> _playerValue;
    private string _Mode;
    private int _playerRankInDeathmatchMode;



    bool isShowing = false;
    private string _typeSorting = "Kills";
    private void Awake()
    {
        if (Instance == null) Instance = this;

        _Mode = GameManager.Instance.Mode;
        if (_Mode == "Domination")
        {
            TabText.text = "Team";
            _rightTeamBarImg.sprite = _enemyTeamBarSprite;
        }
        else if (_Mode == "Deathmatch")
        {
            TabText.text = "Player";
            _rightTeamBarImg.sprite = _allyTeamBarSprite;
        }

        foreach(var player in GameManager.Instance.PlayerList)
        {
            PlayerList.Add(player.GetComponent<VSPlayerInfo>());
            if (player.GetComponent<VSPlayerInfo>().Team.TeamSide == VSTeamSide.TeamAlly) PlayersTeamA.Add(player.GetComponent<VSPlayerInfo>());
            else if (player.GetComponent<VSPlayerInfo>().Team.TeamSide == VSTeamSide.TeamEnemy) PlayersTeamB.Add(player.GetComponent<VSPlayerInfo>());
        }
        UpdateScoreBoard(_typeSorting);
    }
    private void OnEnable()
    {
        isShowing = true;
    }
    private void OnDisable()
    {
        isShowing = false;
    }
    private void Update()
    {
        if (isShowing) UpdateScoreBoard(_typeSorting);
    }
    public void UpdateScoreBoard(string sortType)
    {
        _typeSorting = sortType;

        _playerValue = new Dictionary<VSPlayerInfo, int>();
        _playerTeamAValue = new Dictionary<VSPlayerInfo, int>();
        _playerTeamBValue = new Dictionary<VSPlayerInfo, int>();
        switch (_Mode)
        {
            case "Domination":
                {
                    if (sortType == VSScoreSortType.Kills.ToString())
                    {
                        foreach (VSPlayerInfo info in PlayersTeamA) _playerTeamAValue.Add(info, info.Kills);
                        foreach (VSPlayerInfo info in PlayersTeamB) _playerTeamBValue.Add(info, info.Kills);
                    }
                    else if (sortType == VSScoreSortType.Points.ToString())
                    {
                        foreach (VSPlayerInfo info in PlayersTeamA) _playerTeamAValue.Add(info, info.Points);
                        foreach (VSPlayerInfo info in PlayersTeamB) _playerTeamBValue.Add(info, info.Points);
                    }
                    else if (sortType == VSScoreSortType.Deaths.ToString())
                    {
                        foreach (VSPlayerInfo info in PlayersTeamA) _playerTeamAValue.Add(info, info.Deaths);
                        foreach (VSPlayerInfo info in PlayersTeamB) _playerTeamBValue.Add(info, info.Deaths);
                    }

                    int index = 1;
                    foreach (var it in _playerTeamAValue.OrderByDescending(pair => pair.Value))
                    {
                        LeafTable.transform.GetChild(index).GetComponent<Image>().sprite = _defaultPlayerBarSprite;
                        if (it.Key.gameObject.CompareTag("Player")) LeafTable.transform.GetChild(index).GetComponent<Image>().sprite = _mainPlayerBarSprite;
                        LeafTable.transform.GetChild(index).Find("Name").GetComponent<TextMeshProUGUI>().text = it.Key.Name;
                        LeafTable.transform.GetChild(index).Find("Kills").GetComponent<TextMeshProUGUI>().text = it.Key.Kills.ToString();
                        LeafTable.transform.GetChild(index).Find("Deaths").GetComponent<TextMeshProUGUI>().text = it.Key.Deaths.ToString();
                        LeafTable.transform.GetChild(index).Find("Points").GetComponent<TextMeshProUGUI>().text = it.Key.Points.ToString();
                        index++;
                    }
                    index = 1;
                    foreach (var it in _playerTeamBValue.OrderByDescending(pair => pair.Value))
                    {
                        RightTable.transform.GetChild(index).GetComponent<Image>().sprite = _defaultPlayerBarSprite;
                        RightTable.transform.GetChild(index).Find("Name").GetComponent<TextMeshProUGUI>().text = it.Key.Name;
                        RightTable.transform.GetChild(index).Find("Kills").GetComponent<TextMeshProUGUI>().text = it.Key.Kills.ToString();
                        RightTable.transform.GetChild(index).Find("Deaths").GetComponent<TextMeshProUGUI>().text = it.Key.Deaths.ToString();
                        RightTable.transform.GetChild(index).Find("Points").GetComponent<TextMeshProUGUI>().text = it.Key.Points.ToString();
                        index++;
                    }
                    break;
                }
            case "Deathmatch":
                {
                    if (sortType == VSScoreSortType.Kills.ToString()) foreach (VSPlayerInfo info in PlayerList) _playerValue.Add(info, info.Kills);
                    else if (sortType == VSScoreSortType.Points.ToString()) foreach (VSPlayerInfo info in PlayerList) _playerValue.Add(info, info.Points);
                    else if (sortType == VSScoreSortType.Deaths.ToString()) foreach (VSPlayerInfo info in PlayerList) _playerValue.Add(info, info.Deaths);

                    int indexTable = 1;
                    int indexPlayer = 0;
                    foreach(var it in _playerValue.OrderByDescending(pair => pair.Value))
                    {
                        if (indexPlayer < 5)
                        {
                            LeafTable.transform.GetChild(indexTable).GetComponent<Image>().sprite = _defaultPlayerBarSprite;
                            if (it.Key.gameObject.CompareTag("Player"))
                            {
                                LeafTable.transform.GetChild(indexTable).GetComponent<Image>().sprite = _mainPlayerBarSprite;
                                _playerRankInDeathmatchMode = indexPlayer + 1;
                            }
                            LeafTable.transform.GetChild(indexTable).Find("Name").GetComponent<TextMeshProUGUI>().text = it.Key.Name;
                            LeafTable.transform.GetChild(indexTable).Find("Kills").GetComponent<TextMeshProUGUI>().text = it.Key.Kills.ToString();
                            LeafTable.transform.GetChild(indexTable).Find("Deaths").GetComponent<TextMeshProUGUI>().text = it.Key.Deaths.ToString();
                            LeafTable.transform.GetChild(indexTable).Find("Points").GetComponent<TextMeshProUGUI>().text = it.Key.Points.ToString();
                        }
                        else
                        {
                            if (indexPlayer == 5) indexTable = 1;
                            RightTable.transform.GetChild(indexTable).GetComponent<Image>().sprite = _defaultPlayerBarSprite;
                            if (it.Key.gameObject.CompareTag("Player"))
                            {
                                RightTable.transform.GetChild(indexTable).GetComponent<Image>().sprite = _mainPlayerBarSprite;
                                _playerRankInDeathmatchMode = indexPlayer + 1;
                            }
                            RightTable.transform.GetChild(indexTable).Find("Name").GetComponent<TextMeshProUGUI>().text = it.Key.Name;
                            RightTable.transform.GetChild(indexTable).Find("Kills").GetComponent<TextMeshProUGUI>().text = it.Key.Kills.ToString();
                            RightTable.transform.GetChild(indexTable).Find("Deaths").GetComponent<TextMeshProUGUI>().text = it.Key.Deaths.ToString();
                            RightTable.transform.GetChild(indexTable).Find("Points").GetComponent<TextMeshProUGUI>().text = it.Key.Points.ToString();
                        }
                        indexPlayer++;
                        indexTable++;
                    }
                    break;
                }

        }
      

    }

    public void SetWinLose()
    {
        if(_Mode == "Domination")
        {
            WinLose.SetActive(true);
            if (GameManager.Instance.Teamwin.TeamSide == VSTeamSide.TeamAlly)
            {
                WinLoseTxt.text = "VICTORY";
                WinLoseTxt.color = new Color32(0, 209, 255, 255);
                CoinRewardTxt.text = "+150";
                //CurrencyData.UpdateCurrency(Currency.Coin, 150);
            }
            else if (GameManager.Instance.Teamwin.TeamSide == VSTeamSide.TeamEnemy)
            {
                WinLoseTxt.text = "LOSE";
                WinLoseTxt.color = new Color32(200, 93, 84, 255);
                CoinRewardTxt.text = "+30";
                //CurrencyData.UpdateCurrency(Currency.Coin, 30);
            }
        }
        else if(_Mode == "Deathmatch")
        {
            WinLose.SetActive(true);
            if(_playerRankInDeathmatchMode == 1)
            {
                WinLoseTxt.text = "VICTORY";
                WinLoseTxt.color = new Color32(0, 209, 255, 255);
                CoinRewardTxt.text = "+150";
                //CurrencyData.UpdateCurrency(Currency.Coin, 150);
            }
            else if(_playerRankInDeathmatchMode > 1)
            {
                int coinReward;
                if (_playerRankInDeathmatchMode >= 6) coinReward = 10 * (10 - _playerRankInDeathmatchMode + 1);
                else coinReward = 50 + (5 - _playerRankInDeathmatchMode + 1) * 20;
                WinLoseTxt.text = "LOSE ";
                WinLoseTxt.color = new Color32(200, 93, 84, 255);
                CoinRewardTxt.text = "+" + coinReward.ToString();
                //CurrencyData.UpdateCurrency(Currency.Coin, coinReward);
            }
        }
    }

}
