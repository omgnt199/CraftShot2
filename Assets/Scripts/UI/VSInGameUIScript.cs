using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class VSInGameUIScript : MonoBehaviour
{
    public static VSInGameUIScript instance;
    public GameObject Player;
    [Header("PopUp")]
    public GameObject ReloadingPopUp;
    public GameObject OutOfAmmoPopUp;
    public GameObject HealthBar;
    public GameObject KillReportPopUp;
    public GameObject KillReportPrefab;
    public GameObject TakeDamagePopUp;
    public GameObject LeaderBoardPopUp;
    public GameObject CrossHair;
    public GameObject Counter;
    public GameObject TeamAScoreBar;
    public GameObject TeamBScoreBar;
    public GameObject ZoneUI;
    public GameObject GunView;
    public GameObject TakeDamageNavPrefab;
    public GameObject LargeMapPopUp;
    [Header("Image")]
    public Image TeamAScoreBarImg;
    public Image TeamBScoreBarImg;
    [Header("Text")]
    public TextMeshProUGUI TeamAScoreText;
    public TextMeshProUGUI TeamBScoreText;
    public TextMeshProUGUI HealthTxt;
    public TextMeshProUGUI MagazineTxt;
    public TextMeshProUGUI AmmoTxt;
    public TextMeshProUGUI NadeAmountTxt;
    [Header("Button")]
    public Button ReplayBtn;
    public RectTransform rect;
    public GameObject DamageScorePrefab;

    private void Awake()
    {
        instance = this;
    }
    public void LoadUIDominationMode()
    {
        TeamAScoreBar.SetActive(true);
        TeamBScoreBar.SetActive(true);
        ZoneUI.SetActive(true);
    }

    public void LoadUIDeathMatchMode()
    {
        TeamAScoreBar.SetActive(false);
        TeamBScoreBar.SetActive(false);
        ZoneUI.SetActive(false);
    }
    public void SetUpPlayerUI()
    {
        //Weapon Ammo UI
        int totalammo = Player.GetComponent<VSPlayerControlWeapon>().TotalAmmo;
        int magazine = Player.GetComponent<VSPlayerControlWeapon>().Magazine;
        MagazineTxt.text = magazine.ToString() + "/" + (totalammo - magazine).ToString();
        //Health Bar
        HealthTxt.text = "100/100";
    }
    public void UpdateAmmoUI(int magazine, int ammo)
    {
        MagazineTxt.text = magazine.ToString();
        AmmoTxt.text = "/" + ammo.ToString();
    }
    public void UpdatePlayerHPUI(int hp)
    {
        HealthTxt.text = hp + "/100";
        HealthBar.GetComponent<Image>().fillAmount = (float)Mathf.Max(0, hp) / 100f;
    }
    public void ShowReloadingPopUp() => ReloadingPopUp.SetActive(true);
    public void HideRelaodingPopUp() => ReloadingPopUp.SetActive(false);
    public void ShowOutOfAmmoPopUp()
    {
        OutOfAmmoPopUp.SetActive(true);
        CancelInvoke("HideOutOfAmmoPopUp");
        Invoke("HideOutOfAmmoPopUp", 1.5f);
    }
    public void HideOutOfAmmoPopUp() => OutOfAmmoPopUp.SetActive(false);
    public void ShowDamgeScore(int damage)
    {
        GameObject scoreObj = Instantiate(DamageScorePrefab, rect.transform);
        scoreObj.GetComponent<TextMeshProUGUI>().text = damage.ToString();
        scoreObj.transform.DOScale(1.5f, 0.5f);
        scoreObj.GetComponent<RectTransform>().DOAnchorPos(new Vector2(Random.Range(-200f, 200f), Random.Range(-200f, 200f)), 0.5f).OnComplete(() => Destroy(scoreObj));
    }
    public void UpdateTeamScoreBar(int scoreA, int scoreB)
    {
        TeamAScoreText.text = scoreA.ToString();
        TeamBScoreText.text = scoreB.ToString();
        TeamAScoreBarImg.fillAmount = (float)scoreA / (float)VSGlobals.DOMINATION_MAX_SCORE;
        TeamBScoreBarImg.fillAmount = (float)scoreB / (float)VSGlobals.DOMINATION_MAX_SCORE;
    }

    public void ShowKillReport(string p1, VSTeamSide p1Team, string p2, VSTeamSide p2Team, Sprite weaponIcon, KillType specialKill)
    {
        GameObject report = Instantiate(KillReportPrefab, KillReportPopUp.transform);
        report.GetComponent<VSKillReportUI>().SetKillReport(p1, p1Team, p2, p2Team, weaponIcon, specialKill);
        report.transform.SetSiblingIndex(0);
    }

    public void ShowTakeDamagePopUp(Vector2 nav)
    {
        TakeDamagePopUp.SetActive(true);
        //GameObject navobj = Instantiate(TakeDamageNavPrefab, TakeDamagePopUp.transform);
        //navobj.GetComponent<RectTransform>().anchoredPosition = -nav * 400f;
    }
    public void HideTakeDamagePopUp() => TakeDamagePopUp.SetActive(false);

    public void LoadUIEndGame()
    {
        LeaderBoardPopUp.SetActive(true);
        ReplayBtn.gameObject.SetActive(true);
        VSScoreBoardUI.Instance.SetWinLose();

    }
    public void ShowLeaderBoardPopUp() => LeaderBoardPopUp.SetActive(!LeaderBoardPopUp.activeSelf);
    public void HideEndLeaderBoardPopUp()
    {
        LeaderBoardPopUp.SetActive(false);
        if (GameManager.Instance.IsEndGame)
        {
            //ServiceManager.ShowInter();
            SceneManager.LoadScene("VoxelStrikeGamePlay");
        }
    }
    public void LoadPLayerDeadUI()
    {
        CrossHair.SetActive(false);
        Counter.SetActive(true);
        GunView.SetActive(false);
    }
    public void LoadPlayerAfterReviveUI()
    {
        CrossHair.SetActive(true);
        Counter.SetActive(false);
        GunView.SetActive(true);
        HideRelaodingPopUp();
        UpdatePlayerHPUI(100);
    }
    public void UpdateNadeUI(int amount)
    {
        NadeAmountTxt.text = amount.ToString() + "/0";
    }
    public void SetLargeMapImage(Sprite sprite) => LargeMapPopUp.transform.Find("MainMap").GetComponent<Image>().sprite = sprite;
    public void ShowHideLargeMap() => LargeMapPopUp.SetActive(!LargeMapPopUp.activeSelf);
}