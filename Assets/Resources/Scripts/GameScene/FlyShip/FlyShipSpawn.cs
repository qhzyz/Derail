using Photon.Pun;
using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.UI;

public class FlyShipSpawn : MonoBehaviour
{
    private Transform playerManager;
    private Transform cameraTarget;
    private FlyShipState myState;
    // Start is called before the first frame update
    void Start()
    {
        playerManager = GameObject.Find("PlayerManager").transform;
        cameraTarget = GameObject.Find("CameraTarget").transform;
        myState = GetComponent<FlyShipState>();
        //设置父物体并加入list
        transform.parent = playerManager;
        GameObject.Find("PlayerManager").GetComponent<PlayerManager>().allFlyShip.Add(gameObject);
        //更改飞船名
        gameObject.name = "Player" + GetComponent<PhotonView>().Owner.CustomProperties[GMManager.PLAYER_ID];
        //设置队伍
        if ((int)GetComponent<PhotonView>().Owner.CustomProperties[GMManager.PLAYER_ID] <= PhotonNetwork.CurrentRoom.PlayerCount / 2)
            myState.teamID = 1;
        else
            myState.teamID = 2;
        myState.id = (int)GetComponent<PhotonView>().Owner.CustomProperties[GMManager.PLAYER_ID];
        myState.lastDamageID = myState.id;
        myState.energy = 100;
        myState.hpBar = GameObject.Find("Canvas").transform.GetComponentInChildren<HpBar>();
        myState.captureBar = GameObject.Find("Canvas").transform.GetComponentInChildren<CaptureBar>();
        myState.energyBar = GameObject.Find("Canvas").transform.GetComponentInChildren<EnergyBar>();

        if(GetComponent<PhotonView>().IsMine)
            GameObject.Find("Canvas").transform.GetComponentInChildren<PhotoBar>().flyShipState = myState;

        myState.levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        if (GetComponent<PhotonView>().IsMine)
        {
            //设置相机跟随
            cameraTarget.parent = transform;
            cameraTarget.localPosition = Vector3.zero;
            //添加同步属性
            PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable {
                                                            { GMManager.PLAYER_HP, myState.hp },
                                                            { GMManager.PLAYER_LIVES, myState.lives },
                                                            { GMManager.PLAYER_TEAMID, myState.teamID }
                                                          });
        }
        //确定位置
        LevelManager lm = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        int startPointCount = lm.allStartPoint.Count;
        //team1
        if (myState.id <= PhotonNetwork.CurrentRoom.PlayerCount / 2)
            myState.bornPosition = lm.allStartPoint[myState.id - 1];
        //team2
        else
            myState.bornPosition = lm.allStartPoint[(startPointCount + 1) - (myState.id - PhotonNetwork.CurrentRoom.PlayerCount / 2) - 1];
        transform.position = myState.bornPosition;
    }
}
