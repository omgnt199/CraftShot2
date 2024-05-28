using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[CreateAssetMenu(menuName = "VoxelStrikeTask/DailyTaskPool")]
public class VSDailyTaskPool : ScriptableObject
{
    public List<VSDailyTask> DailyTaskPool;
    private void Awake()
    {
        
    }
    public List<VSDailyTask> GetDailyTaskNewDay(int amount)
    {
        var taskNewDayList = new List<VSDailyTask>();
        for(int i = 0; i < amount; i++)
        {
            List<VSDailyTask> taskList = new List<VSDailyTask>();
            taskList = new List<VSDailyTask>(DailyTaskPool.Where(task => !taskNewDayList.Contains(task)));
            taskNewDayList.Add(taskList[new System.Random().Next(taskList.Count)]);
        }
        return taskNewDayList;
    }
    public VSDailyTask GetDailyTaskByInfo(string info) => DailyTaskPool.Find(task => task.TaskInfo.Equals(info));

}
