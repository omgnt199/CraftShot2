using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public static class DailyTaskObserver
{
    public static List<VSDailyTask> DailyTaskToObserver;

    public static UnityAction AnyTaskClaimed;
    public static void SetDailyTaskToObserver(List<VSDailyTask> taskList)
    {
        DailyTaskToObserver = new List<VSDailyTask>();
        taskList.ForEach(task => 
        {
            if (!task.IsCompleted)
            {
                DailyTaskToObserver.Add(task);
                task.StartObserverTask();
            }
        });
    }
    public static void OnAnyDailyTaskClaimed(VSDailyTask task)
    {
        task.IsClaimed = true;
        DailyTaskToObserver.Remove(task);
        AnyTaskClaimed?.Invoke();
    }
}
