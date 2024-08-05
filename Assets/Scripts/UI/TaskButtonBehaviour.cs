using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskButtonBehaviour : MonoBehaviour
{
    public VSDailyTaskManager DailyTaskManager;
    public Image TaskButtonImg;
    public Sprite TaskCanClaimSprite;
    public Sprite TaskCantClaimSprite;
    private void OnEnable()
    {
        ButtonBehaviour();
        DailyTaskObserver.AnyTaskClaimed += ButtonBehaviour;
    }
    private void OnDisable()
    {
        DailyTaskObserver.AnyTaskClaimed -= ButtonBehaviour;
    }
    void ButtonBehaviour()
    {
        TaskButtonImg.sprite = TaskCantClaimSprite;
        foreach (var item in DailyTaskManager.DailyTaskToday)
        {
            Debug.Log(item.TaskInfo);
            if (item.IsCompleted && !item.IsClaimed)
            {
                TaskButtonImg.sprite = TaskCanClaimSprite;
                Debug.Log("TaskButton Can Claim");
                break;
            }
        }
    }

}
