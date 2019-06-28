using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BalancePanel : MonoBehaviourPunCallbacks
{
    public Button exitButton;
    public Text resultText;
    public GameObject head;
    public GameObject team1Info;
    public GameObject team2Info;
    const float posY = 225;
    private float currentPosY=posY;
    // Start is called before the first frame update
    void Start()
    {
        exitButton.onClick.AddListener(delegate () {
            Time.timeScale = 1;
            PhotonNetwork.LeaveRoom();
            SceneManager.LoadScene("StartScene");
        });
    }

    public void InitPlayerList(List<GameObject> allFlyShip, string result) {
        SetGameResult(result);
        AddHead();
        AddTeam(1, allFlyShip);
        AddTeam(2, allFlyShip);
    }

    public void SetGameResult(string result) {
        resultText.text = result;
    }

    public void AddHead() {
        GameObject temp = Instantiate(head,transform);
        temp.GetComponent<RectTransform>().localPosition = new Vector2(0,currentPosY);
        currentPosY -= head.GetComponent<RectTransform>().sizeDelta.y;
        
    }
    public void AddTeam(int teamID,List<GameObject> allFlyShip) {
        foreach (GameObject i in allFlyShip) {
            FlyShipState state = i.GetComponent<FlyShipState>();
            if (state == null)
                continue;
            if (teamID == state.teamID) {
                AddLine(teamID,state);
            }
        }
    }
    public void AddLine(int teamID,FlyShipState state) {
        GameObject temp = null ;
        if (state.GetComponent<PhotonView>().Owner == null)
            return;
        if (teamID == 1) {
            temp = Instantiate(team1Info, transform);
            temp.GetComponent<RectTransform>().localPosition = new Vector2(0, currentPosY);
        }
        if (teamID == 2) {
            temp = Instantiate(team2Info, transform);
            temp.GetComponent<RectTransform>().localPosition = new Vector2(0, currentPosY);
        }
        if (!temp)
            return;
        currentPosY -= head.GetComponent<RectTransform>().sizeDelta.y;
        Text[] texts = temp.GetComponentsInChildren<Text>();
        texts[0].text = " " + state.GetComponent<PhotonView>().Owner.NickName;
        texts[1].text = state.killCount.ToString();
        texts[2].text = state.deathCount.ToString();
    }
}
