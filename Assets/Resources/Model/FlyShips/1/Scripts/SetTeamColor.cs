using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class SetTeamColor : MonoBehaviourPunCallbacks
{
    public override void OnPlayerPropertiesUpdate(Player target, Hashtable changedProps)
    {
        if (changedProps.ContainsKey(GMManager.PLAYER_TEAMID))
        {
            if(target == transform.parent.parent.GetComponent<PhotonView>().Owner)
                transform.parent.parent.GetComponent<FlyShipState>().teamID = (int)changedProps[GMManager.PLAYER_TEAMID];
            if (transform.parent.parent.GetComponent<FlyShipState>().teamID == 1)
                GetComponent<MeshRenderer>().material.color = GMManager.TEAM1_COLOR;
            if (transform.parent.parent.GetComponent<FlyShipState>().teamID == 2)
                GetComponent<MeshRenderer>().material.color = GMManager.TEAM2_COLOR;
        }
    }
}
