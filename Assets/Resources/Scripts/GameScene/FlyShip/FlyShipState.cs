using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlyShipState : MonoBehaviourPunCallbacks
{
    public int id;
    public bool isCapBarSeen;
    public float hp = 100;
    public float energy = 100;
    public float decEnergySpeed = 2.5f;
    public float addEnergySpeed = 10f;
    public bool isCharge = true;
    public int lives = 10000;
    public int teamID;
    public Vector3 bornPosition;
    PhotonView photonV;
    public LevelManager levelManager;
    public HpBar hpBar;
    public EnergyBar energyBar;
    public CaptureBar captureBar;
    public float captureDistance = 5f;
    public float captureSpeed = 10f;
    //记录KD和伤害来源
    public int lastDamageID = 0;
    public int killCount = 0;
    public int deathCount = 0;

    public float maxDeathInter = 5;
    public float deathCounter = 0;

    #region UNITY
    void Awake() {
        photonV = GetComponent<PhotonView>();
    }
    void Update()
    {
        if (photonV.IsMine)
        {
            UpdateUI();
            UpdateEnergy();
        }
    }
    #endregion

    #region UNITY_CALLBACK
    private void OnCollisionEnter(Collision collision)
    {
        if (!photonV.IsMine)
        {
            return;
        }
        if ((collision.transform.tag == "Aster"))
        {
            DecHpByAster();
        }
    }
    private void OnParticleCollision(GameObject other)
    {
        if (photonV.IsMine)
        {
            DecHp(other.GetComponent<BasicWeapon>().damage, other.transform.GetComponentInParent<FlyShipState>().id);
        }
    }
    #endregion

    #region PHOTON_CALLBACK
    public override void OnPlayerPropertiesUpdate(Player target, Hashtable changedProps)
    {
        if (target == photonV.Owner)
        {
            if (changedProps.ContainsKey(GMManager.PLAYER_HP))
            {
                hp = (float)changedProps[GMManager.PLAYER_HP];
            }
            if (changedProps.ContainsKey(GMManager.PLAYER_LIVES))
            {
                lives = (int)changedProps[GMManager.PLAYER_LIVES];
                if (lives <= 0)
                {
                    //PlayerManager.Killed();
                }
            }
            if (changedProps.ContainsKey(GMManager.PLAYER_ID))
            {
                id = (int)changedProps[GMManager.PLAYER_ID];
            }
            if (changedProps.ContainsKey(GMManager.PLAYER_TEAMID))
            {
                teamID = (int)changedProps[GMManager.PLAYER_TEAMID];
            }
        }
    }
    #endregion

    #region METHOD

    string GetContent(string killer, string killed) {
        return killer + "    kill    " + killed;
    }

    void AddEnergy(float num)
    {
        energy += Mathf.Abs(num);
        if (energy > 100)
            energy = 100;
    }
    void DecEnergy(float num)
    {
        energy -= Mathf.Abs(num);
        if (energy < 0)
        {
            DecHp(energy, 0);
            energy = 0;
        }
    }
    void DecHp(float num, int damageID)
    {
        hp = (float)photonV.Owner.CustomProperties[GMManager.PLAYER_HP];
        float temphp = hp;
        temphp -= Mathf.Abs(num);
        if (temphp <= 0)
        {
            if (DecLive() <= 0)
            {
               // PlayerManager.Killed();
            }
            hp = 100;
        }
        else
        {
            hp = temphp;
        }
        photonV.Owner.SetCustomProperties(new Hashtable { { GMManager.PLAYER_HP, hp } });
        if (damageID != 0)
            lastDamageID = damageID;
    }
    void DecHpByAster()
    {
        DecHp(100, 0);
    }
    int DecLive()
    {
        lives = (int)photonV.Owner.CustomProperties[GMManager.PLAYER_LIVES];
        lives = lives - 1;
        //保存KD
        Killed(lastDamageID);
        photonV.RPC("Killed", RpcTarget.Others, lastDamageID);
        photonV.Owner.SetCustomProperties(new Hashtable { { GMManager.PLAYER_LIVES, lives } });
        photonV.RPC("SetDeath", RpcTarget.All);
        return lives;
    }
    [PunRPC]
    public void Killed(int ldID)
    {
        PlayerManager pm = GetComponentInParent<PlayerManager>();
        deathCount++;
        Player killer = null;
        foreach (GameObject i in pm.allFlyShip)
        {
            if (i.GetComponent<FlyShipState>().id == ldID)
            {
                i.GetComponent<FlyShipState>().killCount++;
                killer = i.GetComponent<PhotonView>().Owner;
            }
        }
        if (photonV.IsMine && killer != null) {
            levelManager.MessageRequir(GetContent(killer.NickName, photonV.Owner.NickName));
        }
    }

    [PunRPC]
    void SetDeath() {
        gameObject.SetActive(false);
        deathCounter = maxDeathInter;
        Invoke("Reborn", maxDeathInter);
    }
    void Reborn()
    {
        if (lives > 0)
        {
            gameObject.SetActive(true);
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            transform.position = bornPosition;
            hp = energy = 100;
            lastDamageID = id;
        }
        else { gameObject.SetActive(false); }
    }

    void UpdateUI()
    {
        if (hpBar)
            hpBar.SetHp(hp);
        if (energyBar)
            energyBar.SetEnergy(energy);
        if (captureBar)
            captureBar.SetByPlanet(gameObject);
    }
    void UpdateEnergy()
    {
        if (isCharge)
            AddEnergy(addEnergySpeed * Time.deltaTime);
        else
            DecEnergy(decEnergySpeed * Time.deltaTime);
    }
    #endregion
}
