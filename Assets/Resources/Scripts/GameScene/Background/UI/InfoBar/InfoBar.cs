using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoBar : MonoBehaviour
{
    public TeamKill team1Kill;
    public TeamKill team2Kill;
    public TeamScore team1Score;
    public TeamScore team2Score;
    public TimeCounter timeCounter;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        team1Kill.UpdateKillCount();
        team2Kill.UpdateKillCount();
        team1Score.UpdateScore();
        team2Score.UpdateScore();
        timeCounter.UpdateTimer();
    }
}
