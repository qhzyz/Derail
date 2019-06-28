using System;
using UnityEngine;
using UnityEngine.UI;

public class CaptureBar : MonoBehaviour
{
    public Text textMessage;
    public Image img;
    public bool isCapBarSeen;
    // Start is called before the first frame update
    void Start()
    {
        textMessage = GetComponentInChildren<Text>();
        img = GetComponent<Image>();
    }

    //go is a flyship
    public void SetByPlanet(GameObject go)
    {
        if (!GameObject.Find("LevelManager"))
            return;
        int temp = 0;
        float captureDistance = go.GetComponent<FlyShipState>().captureDistance;
        float minDistance = captureDistance;
        isCapBarSeen = false;
        go.GetComponent<FlyShipState>().isCharge = false;
        for (int i = 0; i < GameObject.Find("LevelManager").GetComponent<LevelManager>().allAster.Count; i++)
        {
            Capturable ad = GameObject.Find("LevelManager").GetComponent<LevelManager>().allAster[i].GetComponent<Capturable>();
            if (!ad)
                continue;
            float currentDistance = (go.transform.position - GameObject.Find("LevelManager").GetComponent<LevelManager>().allAster[i].transform.position).magnitude
                - GameObject.Find("LevelManager").GetComponent<LevelManager>().allAster[i].GetComponent<SphereCollider>().radius;
            if (currentDistance < captureDistance)
            {
                if (currentDistance < minDistance)
                {
                    minDistance = currentDistance;
                    temp = i;
                    isCapBarSeen = true;
                }
                //
                if (go.GetComponent<FlyShipState>().teamID == ad.ownerID)
                    go.GetComponent<FlyShipState>().isCharge = true;
            }
        }
        if (isCapBarSeen)
        {
            Capturable ad = GameObject.Find("LevelManager").GetComponent<LevelManager>().allAster[temp].GetComponent<Capturable>();
            img.fillAmount = ad.currentValue / ad.maxValue;
            if (ad.currentValueOwnerID == 1)
                img.color = GMManager.TEAM1_COLOR;
            else
                img.color = GMManager.TEAM2_COLOR;
            if (ad.capturingTeam == -1)
                textMessage.text = "Fighting  " + Math.Round(img.fillAmount, 4) * 100 + "%";
            else if (ad.capturingTeam == ad.ownerID && ad.currentValue == ad.maxValue)
                textMessage.text = "Captured";
            else if (ad.capturingTeam == ad.ownerID && ad.currentValue != ad.maxValue)
                textMessage.text = "Repairing  " + Math.Round(img.fillAmount, 4) * 100 + "%";
            else
                textMessage.text = "Capturing  " + Math.Round(img.fillAmount, 4) * 100 + "%";
        }
        else
        {
            img.color = new Color(0, 0, 0, 0);
            textMessage.text = "";
        }
    }
}
