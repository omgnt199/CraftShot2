using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    [SerializeField] GameObject Text;
    public int StartNum;
    private int countDown;
    private float counter;
    private void OnEnable()
    {
        Text.GetComponent<Text>().text = StartNum.ToString();
        countDown = StartNum;
        counter = 0f;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.unscaledDeltaTime;
        if (counter >= 1f)
        {
            counter = 0;
            countDown--;
            Text.GetComponent<Text>().text = countDown.ToString();
        }
    }
}
