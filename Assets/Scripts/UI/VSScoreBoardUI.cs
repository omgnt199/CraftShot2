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
    [SerializeField] private GameObject RewardPopUp;
    //Text
    [SerializeField] private TextMeshProUGUI TabText;
    [SerializeField] private GameObject WinTitle;
    [SerializeField] private GameObject LoseTitle;
    //Sprite
    [SerializeField] private Sprite _mainPlayerBarSprite;
    [SerializeField] private Sprite _defaultPlayerBarSprite;
    [SerializeField] private Sprite _allyTeamBarSprite;
    [SerializeField] private Sprite _enemyTeamBarSprite;
    //Image
    [SerializeField] private Image _rightTeamBarImg;

    //
    [SerializeField] private Button _playerButton;
    //
    private Dictionary<VSPlayerInfo, int> _playerTeamAValue;
    private Dictionary<VSPlayerInfo, int> _playerTeamBValue;
    private Dictionary<VSPlayerInfo, int> _playerValue;
    private string _Mode;
    private int _playerRankInDeathmatchMode;

    public int PlayerRankInDeathmatchMode => _playerRankInDeathmatchMode;



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

        foreach (var player in GameManager.Instance.PlayerList)
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
                    foreach (var it in _playerValue.OrderByDescending(pair => pair.Value))
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
        if (_Mode == "Domination")
        {
            if (((DominationModeSO)GameManager.Instance.CurrentMode).TeamWin.TeamSide == VSTeamSide.TeamAlly) WinTitle.SetActive(true);
            else if (((DominationModeSO)GameManager.Instance.CurrentMode).TeamWin.TeamSide == VSTeamSide.TeamEnemy) LoseTitle.SetActive(true);
        }
        else if (_Mode == "Deathmatch")
        {
            if (_playerRankInDeathmatchMode <= 3) WinTitle.SetActive(true);
            else LoseTitle.SetActive(true);
        }
        _playerButton.onClick.AddListener(ShowRewardPopUp);
    }
    public void ShowRewardPopUp()
    {
        gameObject.SetActive(false);
        RewardPopUp.SetActive(true);
    }
}
