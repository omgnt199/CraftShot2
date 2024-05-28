using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GlobalData : Singleton<GlobalData>
{
    public TimePlayManager TimePlayManager;
    public VSEquipmentPool EquipmentPool;
    public VSDailyTaskManager DailyTaskManager;
    public static float CurrentTimeApplication;
    public bool IsNewDay = false;

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

        CheckNewDay();
        if(IsNewDay) LoadDataOnNewDay();

        DailyTaskManager.LoadDailyTaskToday();

        PlayerGlobalInfo.Load();
        TimePlayManager.Initialize();
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
        if(date.Day != PlayerPrefs.GetInt("d_Day"))
        {
            IsNewDay = true;
            PlayerPrefs.SetInt("d_Day", date.Day);
        }
    }
    void LoadDataOnNewDay()
    {
        DailyTaskManager.RegisterNewDailyTasks(_dailyTaskAmount);
    }

}
