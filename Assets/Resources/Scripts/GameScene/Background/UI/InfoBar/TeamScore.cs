using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamScore : MonoBehaviour
{
    private int totalScore;
    private float maxScore;
    private Text text;
    private Image bar;
    public int teamID;

    void Start()
    {
        text = GetComponentInChildren<Text>();
        bar = transform.Find("BarReal").GetComponent<Image>();
    }

    public void UpdateScore()
    {
        totalScore = 0;
        GameObject lm = GameObject.Find("LevelManager");
        if (!lm)
            return;
        maxScore = lm.GetComponent<LevelManager>().maxScore;
        if (teamID == 1)
            totalScore = lm.GetComponent<LevelManager>().team1Score;
        if (teamID == 2)
            totalScore = lm.GetComponent<LevelManager>().team2Score;
        UpdateText();
        UpdateBar();
    }
    void UpdateText()
    {
        text.text = totalScore.ToString();
    }
    void UpdateBar() {
        bar.fillAmount = totalScore / maxScore;
    }
}
