using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarSetting : MonoBehaviour
{
    [SerializeField] GameObject BarColor;
    public int Value, MaxValue;
    private float widthBar, heighBar, widthBarPerValue;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Value > MaxValue) Value = MaxValue;
        else if (Value < 0) Value = 0; 
        BarColor.GetComponent<RectTransform>().sizeDelta = new Vector2(widthBarPerValue * Value, heighBar);
    }
    public void SetBarValue(int maxValue)
    {
        this.MaxValue = maxValue;
        widthBar = gameObject.GetComponent<RectTransform>().rect.width;
        heighBar = gameObject.GetComponent<RectTransform>().rect.height;
        widthBarPerValue = widthBar / MaxValue;
        BarColor.GetComponent<RectTransform>().sizeDelta = new Vector2(widthBarPerValue * Value, heighBar);
    }
    public void UpdateBarValue(int value)
    {
        this.Value = value;
    }
}
