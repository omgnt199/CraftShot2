using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScoreRow : MonoBehaviour
{
    public GameObject ScoreText;
    public int Score;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateScore(int score)
    {
        this.Score = score;
        ScoreText.GetComponent<Text>().text = this.Score.ToString();
    }
}
