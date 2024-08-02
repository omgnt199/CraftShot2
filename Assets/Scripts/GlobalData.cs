using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GlobalData : Singleton<GlobalData>
{
    public ServiceManager serviceManager;
    public TimePlayManager TimePlayManager;
    public VSEquipmentPool EquipmentPool;
    public VSDailyTaskManager DailyTaskManager;
    public GameSettingSO GameSettingSO;
    public static float CurrentTimeApplication;
    public bool IsNewDay = false;

    [SerializeField] private Cheat _Cheat;
    private int _dailyTaskAmount = 4;
    public override void Awake()
    {
        base.Awake();
        LoadData();
    }
    private void Start()
    {
    }
    void LoadData()
    {
        CurrencyData.Load();
        if (PlayerPrefs.GetInt("FirstOpenApp") != 1)
        {
            LoadEquipmentOnFirstOpen();
            PlayerPrefs.SetString("PlayerName", "Guest");
            PlayerPrefs.SetInt("PlayerLevel", 1);
            PlayerPrefs.SetInt("FirstOpenApp", 1);
            CurrencyData.UpdateCurrency(CurrencyType.Coin, 200);
            CurrencyData.UpdateCurrency(CurrencyType.Diamond, 5);
        }
        PlayerGlobalInfo.Load();
        PlayerEquipmentInfo.Load();

        CheckNewDay();
        if (IsNewDay) LoadDataOnNewDay();

        DailyTaskManager.LoadDailyTaskToday();

        TimePlayManager.Initialize();

        serviceManager?.Init();
    }
    private void Update()
    {
        CurrentTimeApplication = Time.time;
    }
    private void OnApplicationQuit()
    {
        TimePlayManager.Tracking();
    }
    private void OnDisable()
    {
        TimePlayManager.Tracking();
    }

    void CheckNewDay()
    {
        DateTime date = DateTime.Now;
        if (date.Day != PlayerPrefs.GetInt("d_Day"))
        {
            IsNewDay = true;
            PlayerPrefs.SetInt("d_Day", date.Day);
        }
    }
    void LoadDataOnNewDay()
    {
        DailyTaskManager.RegisterNewDailyTasks(_dailyTaskAmount);
    }

    void LoadEquipmentOnFirstOpen()
    {
        PlayerPrefs.SetString("VSPrimaryWeaponUsing", "Scar");
        PlayerPrefs.SetString("VSSecondaryWeaponUsing", "Desert Eagle");
        PlayerPrefs.SetString("VSSupportWeaponUsing", "Karambit");
        PlayerPrefs.SetString("VSNadeUsing", "Grenade");


        PlayerEquipmentInfo.Add("Scar");
        PlayerEquipmentInfo.Add("Desert Eagle");
        PlayerEquipmentInfo.Add("Karambit");
        PlayerEquipmentInfo.Add("Grenade");

        PlayerEquipmentInfo.Save();
    }

}
