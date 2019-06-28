using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    public List<GameObject> allFlyShip = new List<GameObject>();
    void Start()
    {
        gameObject.name = "PlayerManager";
        GameObject playerpb = Resources.Load("Prefabs/FlyShips/FlyShip1") as GameObject;
        PhotonNetwork.Instantiate("Prefabs/FlyShips/FlyShip1",GameObject.Find("LevelManager").GetComponent<LevelManager>().allStartPoint[0],
            Quaternion.Euler(playerpb.GetComponent<FlyShipControl>().adjustModelAngle));
    }
    
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("Player" + otherPlayer.CustomProperties[GMManager.PLAYER_ID]+"离开了");
        otherPlayer.SetCustomProperties(new Hashtable{ {GMManager.PLAYER_LIVES,0 } });
        for (int i = 0; i < allFlyShip.Count; i++) {
            if (allFlyShip[i].GetComponent<PhotonView>().Owner == null) {
                allFlyShip.RemoveAt(i);
                break;
            }
        } ;
        CheckGameOver();
    }

    public void CheckGameOver() {
        bool team1Remain = false;
        bool team2Remain = false;
        foreach (Player i in PhotonNetwork.PlayerList)
        {
            if (i == null)
                continue;
            if ((int)i.CustomProperties[GMManager.PLAYER_TEAMID] == 1)
                team1Remain = true;
            else if ((int)i.CustomProperties[GMManager.PLAYER_TEAMID] == 2)
                team2Remain = true;
        }
        if (!team1Remain || !team2Remain)
        {
            LevelManager levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
            if (team1Remain)
            {
                LevelManager.winnerTeam = 1;
                levelManager.GameOver(GameOverCause.TEAMX_WIN);
            }
            else if (team2Remain)
            {
                LevelManager.winnerTeam = 2;
                levelManager.GameOver(GameOverCause.TEAMX_WIN);
            }
            else
                levelManager.GameOver(GameOverCause.DRAW);
        }
    }
}
