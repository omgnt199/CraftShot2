using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

[CreateAssetMenu(menuName = "VoxelStrikeTask/DailyTaskManager")]
public class VSDailyTaskManager : ScriptableObject
{
    public List<VSDailyTask> DailyTaskToday;
    public VSDailyTaskPool TaskPool;
    private int _taskAmount;
    private bool _isRegister = false;
    private string _fileSaveName = "dailyTaskData.dat";
    private void Awake()
    {
        DailyTaskToday = new List<VSDailyTask>();
    }
    public void RegisterNewDailyTasks(int taskAmount)
    {
        ResetOldTaskProgress();
        if (!File.Exists(Application.persistentDataPath + "/" + _fileSaveName)) CreateFileSave();
        //var path = Application.persistentDataPath + "/" + _fileSaveName;
        //Debug.Log(path);
        DailyTaskToday = new List<VSDailyTask>(TaskPool.GetDailyTaskNewDay(taskAmount));
        SaveDailyTaskToday();
        _isRegister = true;
        PlayerPrefs.SetString("DailyTaskRegistered", "true");
    }
    public void LoadDailyTaskToday()
    {
        DailyTaskData data = null;
        data = DataManager.Load(_fileSaveName, data);
        DailyTaskToday.Clear();
        foreach (var id in data.DailyTasksInfo)
        {
            DailyTaskToday.Add(TaskPool.GetDailyTaskByInfo(id));
        }
        DailyTaskObserver.SetDailyTaskToObserver(DailyTaskToday);
    }
    public void SaveDailyTaskToday()
    {
        DailyTaskData data = new DailyTaskData();
        var taskInfoList = DailyTaskToday.Select(task => task.TaskInfo);
        data.DailyTasksInfo = new List<string>(taskInfoList);
        DataManager.Save(_fileSaveName, data);
    }
    void CreateFileSave()
    {
        FileStream fs =  File.Create(Application.persistentDataPath + "/" + _fileSaveName);
        fs.Close();
    }
    void ResetOldTaskProgress()
    {
        TaskPool.DailyTaskPool.ForEach(task => 
        {
            task.IsCompleted = false;
            task.IsClaimed = false;
            task.AchivedValue = 0;
        });
    }
}
[System.Serializable]
public class DailyTaskData
{
    public List<string> DailyTasksInfo;
}
