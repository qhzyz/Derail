using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectPanel : MonoBehaviourPunCallbacks
{
    private byte roomPlayerCount;
    [SerializeField]
    private Text stateText;
    [SerializeField]
    private Text playerCountText;
    private bool isConnected;
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        roomPlayerCount = 2;
    }

    public void SetConnect()
    {
        isConnected = true;
        stateText.text = "Connecting Server...";
        PhotonNetwork.LocalPlayer.NickName = "d1fficult" + Random.Range(0, 100);
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            Debug.Log(PhotonNetwork.LocalPlayer.NickName);
        }
        else
        {
            Debug.Log("already connected");
            StartCoroutine(ConnectInReady());
        }
    }
    IEnumerator ConnectInReady() {
        while (!PhotonNetwork.IsConnectedAndReady) {
            yield return new WaitForEndOfFrame();
            Debug.Log("Not Ready");
        }
        Debug.Log("In Ready");
        PhotonNetwork.JoinRoom("derail");
    }

    private void UpdatePlayerCountText()
    {
        byte currentNum = PhotonNetwork.CurrentRoom.PlayerCount;
        byte maxNum = PhotonNetwork.CurrentRoom.MaxPlayers;
        playerCountText.text = currentNum + "/" + maxNum;
        if (currentNum == maxNum) { StartGame(); }
    }

    private void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            Debug.Log("CallStart");
            PhotonNetwork.LoadLevel("MainGame");
        }
    }
    #region PUN_CALLBACK

    public override void OnConnectedToMaster()
    {
        Debug.Log("connected success"+isConnected);
        if (isConnected)
        {
            stateText.text = "Searching Game...";
            PhotonNetwork.JoinRoom("derail");
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Create failed");
        PhotonNetwork.JoinRoom("derail");
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Join failed");
        PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = roomPlayerCount }, null);
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Join failed");
        PhotonNetwork.CreateRoom("derail", new RoomOptions() { MaxPlayers = roomPlayerCount }, null);
    }
    public override void OnCreatedRoom()
    {
        Debug.Log("Create Room");
    }
    public override void OnJoinedRoom()
    {
        stateText.text = "Waiting for Others... ("+PhotonNetwork.CloudRegion+")";
        UpdatePlayerCountText();
        Debug.Log("Joined");
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerCountText();
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerCountText();
    }
    #endregion
}
