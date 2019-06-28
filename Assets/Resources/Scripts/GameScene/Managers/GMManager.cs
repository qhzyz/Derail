using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine;

public class GMManager : MonoBehaviour
{
    [SerializeField]
    private string pbLevelManager;
    [SerializeField]
    private string pbPlayerManager;
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
            Init();
    }
    private void Init()
    {
        InitLevelManager();
        InitPlayerManager();
    }
    private void InitLevelManager()
    {
        PhotonNetwork.Instantiate(pbLevelManager, Vector3.zero, Quaternion.identity);
        //Instantiate(Resources.Load(pbLevelManager), Vector3.zero, Quaternion.identity);
    }
    private void InitPlayerManager()
    {
        int count = 1;
        foreach (Player i in PhotonNetwork.PlayerList)
            i.SetCustomProperties(new Hashtable{ {PLAYER_ID,count++ } });
        PhotonNetwork.Instantiate(pbPlayerManager, Vector3.zero, Quaternion.identity);
        //Instantiate(Resources.Load(pbPlayerManager), Vector3.zero, Quaternion.identity);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    #region static
    public static float G = 10;
    public static int MAX_PLAYER_NUM = 4;
    public static readonly Vector3 RIGHT_DIR = new Vector3(1, 0, 0);
    public static readonly Vector3 UP_DIR = new Vector3(0, 1, 0);
    public static readonly Vector3 AXIS_Z = new Vector3(0, 0, 1);
    public static readonly Color TEAM1_COLOR = new Color(0,0,1,1);
    public static readonly Color TEAM2_COLOR = new Color(1,0,0,1);
    public static int GetRandomSeed()
    {
        byte[] bytes = new byte[4];
        System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
        rng.GetBytes(bytes);
        return BitConverter.ToInt32(bytes, 0);
    }
    #endregion

    #region const
    public const string PLAYER_HP = "PlayerHp";
    public const string PLAYER_LIVES = "PlayerLives"; 
    public const string PLAYER_TEAMID = "PlayerTeamID";
    public const string PLAYER_ID = "PlayerID";

    public const float ENERGY_RESTORE_SPEED = 1;
    public const float ENERGY_WASTE_SPEED = 1;
    #endregion
}
