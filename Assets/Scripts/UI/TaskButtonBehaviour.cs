using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskButtonBehaviour : MonoBehaviour
{
    public Image TaskButtonImg;
    public Sprite TaskCanClaimSprite;
    public Sprite TaskCantClaimSprite;
    private void OnEnable()
    {
        TaskButtonImg.sprite = TaskCantClaimSprite;
        foreach (var item in DailyTaskObserver.DailyTaskToObserver)
        {
            if(item.IsCompleted && !item.IsClaimed)
            {
                TaskButtonImg.sprite = TaskCanClaimSprite;
                break;
            }
        }
    }
}
