using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamKill : MonoBehaviour
{
    private int totalKillCount;
    public int teamID;
    private Text text;

    void Start()
    {
        text = GetComponentInChildren<Text>();
    }

    public void UpdateKillCount()
    {
        totalKillCount = 0;
        GameObject pm = GameObject.Find("PlayerManager");
        if (!pm)
            return;
        foreach(GameObject i in pm.GetComponent<PlayerManager>().allFlyShip)
        {
            if(i.GetComponent<FlyShipState>().teamID==teamID)
            {
                totalKillCount += i.GetComponent<FlyShipState>().killCount;
            }
        }
        UpdateText();
    }
    void UpdateText()
    {
        text.text = totalKillCount.ToString();
    }
}
