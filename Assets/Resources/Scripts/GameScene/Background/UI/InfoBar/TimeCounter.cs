using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Photon.Pun;

public class TimeCounter : MonoBehaviour
{
    int minute;
    int second;
    float timeSpend = 0.0f;   
    
    public void UpdateTimer()
    {
        timeSpend += Time.deltaTime;
        int hour = (int)timeSpend / 3600;
        minute = ((int)timeSpend - hour * 3600) / 60;
        second = (int)timeSpend - hour * 3600 - minute * 60;
        GetComponentInChildren<Text>().text = string.Format("{0:D2}:{1:D2}", minute, second);

        if (minute >= 5)
        {
            GameObject lm = GameObject.Find("LevelManager");
            if(lm)
                lm.GetComponent<LevelManager>().GameOver(GameOverCause.TIME_OUT);
        }
    }
}