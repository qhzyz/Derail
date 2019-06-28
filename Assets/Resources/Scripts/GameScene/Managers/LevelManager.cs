using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameOverCause { TEAMX_WIN, TIME_OUT,DRAW }
public class LevelManager : MonoBehaviour, IPunObservable
{
    public static int winnerTeam;
    public string mapName="map1";
    int asterCount;
    int startCount;
    public List<GameObject> allAster;
    public List<AsterData> asterData;    
    public List<Vector3> allStartPoint;

    public int maxScore = 1000;
    public int team1Score = 0;
    public int team2Score = 0;
    public float mainAsterCpScore = 400;
    public GameObject team1MainAster;
    public GameObject team2MainAster;

    public GameObject balancePanel;
    public GameObject messagePanel;


    void Awake () {
        allAster = new List<GameObject>();
        asterData = new List<AsterData>();
        allStartPoint = new List<Vector3>();
        team1Score = team2Score = 0;

        gameObject.name = "LevelManager";
        ReadFromFile(mapName);
        if (PhotonNetwork.IsMasterClient)
            InstantiateMapByMaster();
        foreach (GameObject i in allAster)
        {
            Capturable cp = i.GetComponent<Capturable>();
            if (!cp)
                continue;
            if (cp.ownerID == 1)
                team1MainAster = i;
            else if (cp.ownerID == 2)
                team2MainAster = i;
        }
        balancePanel = GameObject.Find("Canvas").transform.Find("BalancePanel").gameObject;
        messagePanel = GameObject.Find("Canvas").transform.Find("MessagePanel").gameObject;
        if (PhotonNetwork.IsMasterClient) {
            MessageRequir("目标：抢夺资源星球");
        }
    }
    
    public void GameOver(GameOverCause Cause)
    {
        if (!PhotonNetwork.IsMasterClient||balancePanel.activeSelf)
            return;
        Time.timeScale = 0;
        switch (Cause)
        {
            case GameOverCause.TEAMX_WIN:
                GetComponent<PhotonView>().RPC("WinBalance", RpcTarget.All, winnerTeam);
                break;
            case GameOverCause.DRAW:
                GetComponent<PhotonView>().RPC("DrawBalance", RpcTarget.All);
                break;
            case GameOverCause.TIME_OUT:
                GetComponent<PhotonView>().RPC("TimeOutBalance", RpcTarget.All);
                break;
        }
    }
    public void UpdateScore(int teamID, int value) {
        if (teamID == 1)
            team1Score += value;
        if (teamID == 2)
            team2Score += value;

        if (team1Score >= maxScore && team2Score >= maxScore)
            GameOver(GameOverCause.DRAW);
        else if (team1Score >= maxScore) {
            winnerTeam = 1;
            GameOver(GameOverCause.TEAMX_WIN);
        }
        else if (team2Score >= maxScore) {
            winnerTeam = 2;
            GameOver(GameOverCause.TEAMX_WIN);
        }


        if (team1Score >= mainAsterCpScore && !team1MainAster.GetComponent<Capturable>().isMainAsterCapturable) {
            MessageRequir("蓝方主星球已开放占领");
            team1MainAster.GetComponent<PhotonView>().RPC("SetMainAsterCapturable", RpcTarget.All, true);
        }
        if (team2Score >= mainAsterCpScore && !team2MainAster.GetComponent<Capturable>().isMainAsterCapturable) {
            MessageRequir("红方主星球已开放占领");
            team2MainAster.GetComponent<PhotonView>().RPC("SetMainAsterCapturable", RpcTarget.All, true);
        }
    }

    public void ReadFromFile(string fileName) {
        BinaryReader br = new BinaryReader(new FileStream(fileName + ".map", FileMode.Open));

        asterCount = br.ReadInt32();
        startCount = br.ReadInt32();

        for (int i = 0; i < asterCount; i++)
        {
            AsterData tempData = new AsterData();
            tempData.parentID = br.ReadInt32();
            tempData.ID = br.ReadInt32();
            tempData.ownerTeamID = br.ReadInt32();
            tempData.prefabName = br.ReadString();

            tempData.myPosition.x = (float)br.ReadDouble();
            tempData.myPosition.y = (float)br.ReadDouble();
            tempData.myPosition.z = (float)br.ReadDouble();
            tempData.isClockwise = br.ReadBoolean();
            asterData.Add(tempData);
        }
        for (int i = 0; i < startCount; i++)
        {
            Vector3 tempVec = new Vector3();
            tempVec.x = (float)br.ReadDouble();
            tempVec.y = (float)br.ReadDouble();
            tempVec.z = (float)br.ReadDouble();
            allStartPoint.Add(tempVec);
        }
        br.Close();
    }
    public void InstantiateMapByMaster()
    {
        object[] temp = new object[6];
        for (int i = 0; i < asterCount; i++)
        {
            temp[0] = asterData[i].parentID;
            temp[1] = asterData[i].ID;
            temp[2] = asterData[i].ownerTeamID;
            temp[3] = asterData[i].prefabName;
            temp[4] = asterData[i].myPosition;
            temp[5] = asterData[i].isClockwise;
            PhotonNetwork.Instantiate("Prefabs/Asters/" + asterData[i].prefabName, Vector3.zero, Quaternion.identity, 0, temp);
        }
    }

    public void MessageRequir(string content) {
        GetComponent<PhotonView>().RPC("SetMessage", RpcTarget.All, content);
    }
    [PunRPC]
    public void SetMessage(string content) {
        messagePanel.GetComponent<MessagePanel>().SetMessage(content);
    }
    [PunRPC]
    public void WinBalance(int winTeam)
    {
        winnerTeam = winTeam;
        balancePanel.SetActive(true);
        balancePanel.GetComponent<BalancePanel>().InitPlayerList(GameObject.Find("PlayerManager").GetComponent<PlayerManager>().allFlyShip,"Team "+winnerTeam+" Win");
    }
    [PunRPC]
    public void DrawBalance() {
        balancePanel.SetActive(true);
        balancePanel.GetComponent<BalancePanel>().InitPlayerList(GameObject.Find("PlayerManager").GetComponent<PlayerManager>().allFlyShip, "Draw");
    }
    [PunRPC]
    public void TimeOutBalance() {
        balancePanel.SetActive(true);
        balancePanel.GetComponent<BalancePanel>().InitPlayerList(GameObject.Find("PlayerManager").GetComponent<PlayerManager>().allFlyShip, "Time Out");
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(team1Score);
            stream.SendNext(team2Score);
        }
        else
        {
            team1Score= (int)stream.ReceiveNext();
            team2Score = (int)stream.ReceiveNext();
        }
    }
}
