using GoogleMobileAds.Api;
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


    Queue<Action> jobs = new Queue<Action>();
    public override void Awake()
    {
        base.Awake();

    }
    private void Start()
    {
        LoadData();
    }
    void LoadData()
    {
        CurrencyData.Load();
        if (PlayerPrefs.GetInt("FirstOpenApp") != 1)
        {
            LoadEquipmentOnFirstOpen();
            PlayerPrefs.SetString("PlayerName", "Guest");
            PlayerPrefs.SetInt("PlayerLevel", 1);
            CurrencyData.UpdateCurrency(CurrencyType.Coin, 200);
            CurrencyData.UpdateCurrency(CurrencyType.Diamond, 5);

            PlayerPrefs.SetInt("FirstOpenApp", 1);
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

        while (jobs.Count > 0)
            jobs.Dequeue().Invoke();

    }

    internal void AddJob(Action newJob)
    {
        jobs.Enqueue(newJob);
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
        //IsNewDay = true;
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
