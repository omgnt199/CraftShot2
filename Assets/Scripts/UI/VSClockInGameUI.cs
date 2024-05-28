using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VSClockInGameUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI clockText;
    public int GameTime = 180;
    private float timer = 0f;
    private int m, s;
    private string sT, mT;
    bool isTimeout = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!VSGameManager.Instance.IsEndGame)
        {
            if (GameTime == 0)
            {
                VSGameManager.Instance.OnEndGame();
                isTimeout = true;
            }
            if (!isTimeout)
            {
                timer += Time.deltaTime;
                if (timer >= 1f)
                {
                    GameTime--;
                    m = GameTime / 60;
                    s = GameTime % 60;
                    mT = "00";
                    sT = "00";
                    if (m / 10 < 1) mT = "0" + m.ToString();
                    else mT = m.ToString();
                    if (s / 10 < 1) sT = "0" + s.ToString();
                    else sT = s.ToString();
                    clockText.text = mT + ":" + sT;
                    timer = 0;
                }
            }
        }
    }
}
