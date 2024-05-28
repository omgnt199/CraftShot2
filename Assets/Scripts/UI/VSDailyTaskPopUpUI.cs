using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class VSDailyTaskPopUpUI : MonoBehaviour
{
    public static VSDailyTaskPopUpUI Instance;
    private List<VSDailyTask> _dailyTaskList;
    public GameObject TaskUIPrefab;
    //UI
    public RectTransform TaskContent;
    private void OnEnable()
    {
        Instance = this;
        LoadTaskUI();
    }

    public void LoadTaskUI()
    {
        Debug.Log("LoadTaskUI");
        _dailyTaskList = new List<VSDailyTask>(GlobalData.Instance.DailyTaskManager.DailyTaskToday.Where(task => !task.IsClaimed));
        ClearTask();
        foreach(var task in _dailyTaskList)
        {
            GameObject temp = Instantiate(TaskUIPrefab, TaskContent.gameObject.transform);
            temp.GetComponent<VSTaskUI>().Set(task);
        }
        int spaceRow = Mathf.Max(_dailyTaskList.Count - 1, 1);
        TaskContent.sizeDelta
            = new Vector2(TaskContent.sizeDelta.x
            , TaskUIPrefab.GetComponent<RectTransform>().sizeDelta.y * _dailyTaskList.Count
            + TaskContent.GetComponent<VerticalLayoutGroup>().spacing * spaceRow + TaskContent.GetComponent<VerticalLayoutGroup>().padding.top);
    }
    public void ClearTask()
    {
        foreach (Transform task in TaskContent.transform) Destroy(task.gameObject);
    }
}
