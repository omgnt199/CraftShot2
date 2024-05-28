using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MiniGamesSettingPopUpUI : MonoBehaviour
{
    [SerializeField] Button CancelBtn, HomeBtn, SoundBtn, VibrationBtn;
    [SerializeField] Sprite On, Off;
    public static bool Sound = true;
    public static bool Vibration = true;
    private void Awake()
    {
        LoadSettingPopUp();
        CancelBtn.onClick.AddListener(HideSettingPopUp);
        HomeBtn.onClick.AddListener(LoadMainMenuScene);
        SoundBtn.onClick.AddListener(ChangeSoundStatus);
        VibrationBtn.onClick.AddListener(ChangeVibrationStatus);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void LoadSettingPopUp()
    {
        if (Sound) ChangeButtonIcon(SoundBtn, On);
        else ChangeButtonIcon(SoundBtn, Off);
        if (Vibration) ChangeButtonIcon(VibrationBtn, On);
        else ChangeButtonIcon(VibrationBtn, Off);
    }
    void ChangeButtonIcon(Button btn, Sprite icon)
    {
        btn.gameObject.GetComponent<Image>().sprite = icon;
    }
    void ChangeSoundStatus()
    {
        if (Sound)
        {
            ChangeButtonIcon(SoundBtn, Off);
            Sound = false;
        }
        else
        {
            ChangeButtonIcon(SoundBtn, On);
            Sound = true;
        }
        AudioListener.volume = Sound ? 1 : 0;
    }
    void ChangeVibrationStatus()
    {
        if (Vibration)
        {
            ChangeButtonIcon(VibrationBtn, Off);
            Vibration = false;
        }
        else
        {
            ChangeButtonIcon(VibrationBtn, On);
            Vibration = true;
        }
    }
    void HideSettingPopUp()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }
    void LoadMainMenuScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("CrossyRoadMainScene");
    }
}
