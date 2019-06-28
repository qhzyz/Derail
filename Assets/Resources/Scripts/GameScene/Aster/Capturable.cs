using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
public class Capturable : MonoBehaviour, IPunObservable
{
    public int ownerID;
    public int currentValueOwnerID;
    public int capturingTeam;
    public float currentValue;
    public float maxValue = 100;
    public Material mat0;
    public Material mat1;
    public Material mat2;

    public bool isMainAster;
    public bool isMainAsterCapturable;

    void Start() {
        if(ownerID!=0&&PhotonNetwork.IsMasterClient)
            InvokeRepeating("UpdateScore", 0, 0.5f);
    }

    void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
        if (isMainAster && !isMainAsterCapturable)
            return;
        if (!GameObject.Find("PlayerManager"))
            return;

        capturingTeam = 0;
        float tempValue = 0;
        PlayerManager pm = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        for (int i = 0; i < pm.allFlyShip.Count; i++)
        {
            if (!pm.allFlyShip[i].activeSelf)
                continue;
            if (capturingTeam == -1)
                break;
            float captureDistance = pm.allFlyShip[i].GetComponent<FlyShipState>().captureDistance;
            float currentDistance = (transform.position - pm.allFlyShip[i].transform.position).magnitude - gameObject.GetComponent<SphereCollider>().radius;
            if (pm.allFlyShip[i] && captureDistance > currentDistance)
            {
                if (capturingTeam == 0)
                {
                    capturingTeam = pm.allFlyShip[i].GetComponent<FlyShipState>().teamID;
                    tempValue += pm.allFlyShip[i].GetComponent<FlyShipState>().captureSpeed;
                }
                else if (pm.allFlyShip[i].GetComponent<FlyShipState>().teamID != capturingTeam)
                {
                    capturingTeam = -1;
                    break;
                }
                else
                {
                    tempValue += pm.allFlyShip[i].GetComponent<FlyShipState>().captureSpeed;
                }
            }
        }
        //
        if (capturingTeam != 0 && capturingTeam != -1)
        {
            if (capturingTeam != currentValueOwnerID)
            {
                currentValue -= tempValue*Time.deltaTime;
                if (currentValue <= 0)
                {
                    currentValue = -currentValue;
                    ChangeCurrentOwner(capturingTeam);
                    GetComponent<PhotonView>().RPC("ChangeCurrentOwner", RpcTarget.Others, capturingTeam);
                    ChangeOwner(0);
                    GetComponent<PhotonView>().RPC("ChangeOwner", RpcTarget.Others, 0);
                    GameObject.Find("LevelManager").GetComponent<LevelManager>().MessageRequir("资源点部署已被破坏");
                }
                if (currentValue >= maxValue)
                {
                    currentValue = maxValue;
                    if (capturingTeam != ownerID)
                    {
                        ChangeOwner(capturingTeam);
                        GetComponent<PhotonView>().RPC("ChangeOwner", RpcTarget.Others, capturingTeam);
                    }
                }
            }
            else
            {
                currentValue += tempValue * Time.deltaTime;
                if (currentValue >= maxValue)
                {
                    currentValue = maxValue;
                    if (capturingTeam != ownerID)
                    {
                        ChangeOwner(capturingTeam);
                        GetComponent<PhotonView>().RPC("ChangeOwner", RpcTarget.Others, capturingTeam);
                        GameObject.Find("LevelManager").GetComponent<LevelManager>().MessageRequir("资源点已部署");
                    }
                }
            }
        }
    }

    public void Init(int ownerID) {
        this.ownerID = ownerID;
        currentValue = 0;
        capturingTeam = 0;
        isMainAster = false;
        isMainAsterCapturable = false;
        currentValueOwnerID = ownerID;
        if (ownerID != 0)
        {
            currentValue = maxValue;
            isMainAster = true;
            capturingTeam = ownerID;
        }
    }
    void UpdateScore() {
        GameObject lm = GameObject.Find("LevelManager");
        if (!lm)
            return;

        if(isMainAster)
            lm.GetComponent<LevelManager>().UpdateScore(ownerID, 2);
        else
            lm.GetComponent<LevelManager>().UpdateScore(ownerID, 1);
    }

    [PunRPC]
    void SetMainAsterCapturable(bool v) {
        isMainAsterCapturable = v;
    }

    [PunRPC]
    void ChangeOwner(int id)
    {
        if (!isMainAster) {
            if (id == 0 && GetComponent<MeshRenderer>().material != mat0)
                GetComponent<MeshRenderer>().material = mat0;
            else if (id == 1 && GetComponent<MeshRenderer>().material != mat1)
                GetComponent<MeshRenderer>().material = mat1;
            else if (id == 2 && GetComponent<MeshRenderer>().material != mat2)
                GetComponent<MeshRenderer>().material = mat2;
        }
        ownerID = id;
        if (id != 0)
            ChangeCurrentOwner(id);
        if (id != 0 && PhotonNetwork.IsMasterClient)
        {
            CancelInvoke("UpdateScore");
            InvokeRepeating("UpdateScore", 0, 0.5f);
        }
    }
    [PunRPC]
    void ChangeCurrentOwner(int id)
    {
        currentValueOwnerID = id;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(currentValue);
            stream.SendNext(capturingTeam);
        }
        else
        {
            currentValue = (float)stream.ReceiveNext();
            capturingTeam = (int)stream.ReceiveNext();
        }
    }
}
