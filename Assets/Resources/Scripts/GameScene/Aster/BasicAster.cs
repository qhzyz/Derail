using Photon.Pun;
using System.Threading;
using UnityEngine;

public struct AsterData
{
    public int parentID;
    public int ID;
    public int ownerTeamID;
    public string prefabName;
    public Vector3 myPosition;
    public bool isClockwise;
    public AsterData(int parentID, int ID,int ownerTeamID,string prefabName, Vector3 myPosition, bool isClockwise)
    {
        this.parentID = parentID;
        this.ID = ID;
        this.ownerTeamID = ownerTeamID;
        this.prefabName = prefabName;
        this.myPosition = myPosition;
        this.isClockwise = isClockwise;
    }
}

public class BasicAster : MonoBehaviour {
    #region MAP_EDIT
    public static int editCount = 0;
    #endregion
    public Transform asterParent;
    public string prefabName;
    public int id = 0;

    protected float w;
    public bool isClockwise;
    [SerializeField]
    protected bool isAroundParent;
    protected float provideArea;

    public void Awake()
    {
        GameObject tempManager = GameObject.Find("LevelManager");
        if (tempManager){ //游戏场景内
            tempManager.GetComponent<LevelManager>().allAster.Add(gameObject);
            SetAsterData();
        }
        else {//编辑场景时
            id = editCount++;
        }
    }

    public virtual void Start () {
    }
    public virtual void Update () {
        if (isAroundParent)
            AroundParent();
    }

    void AroundParent() {
        if (asterParent)
        {
            if (!isClockwise)
                transform.RotateAround(asterParent.position, GMManager.AXIS_Z, w / Mathf.PI * 180 * Time.deltaTime);
            else
                transform.RotateAround(asterParent.position, GMManager.AXIS_Z, -w / Mathf.PI * 180 * Time.deltaTime);
        }
    }

    public AsterData GetAsterData()
    {
        int temp = -1;
        if (asterParent) temp = transform.parent.GetComponent<BasicAster>().id;

        if (GetComponent<Capturable>() && GetComponent<Capturable>().isMainAster)
            return new AsterData(temp, id, GetComponent<Capturable>().ownerID, prefabName, transform.localPosition, isClockwise);
        return new AsterData(temp, id, -1, prefabName, transform.localPosition, isClockwise);
    }
    
    public void SetAsterData() {
        object[] temp = new object[6];
        temp = GetComponent<PhotonView>().InstantiationData;
        //
        AsterData myData = new AsterData();
        myData.parentID = (int)temp[0];
        myData.ID = (int)temp[1];
        myData.ownerTeamID = (int)temp[2];
        myData.prefabName = (string)temp[3];
        myData.myPosition = (Vector3)temp[4];
        myData.isClockwise = (bool)temp[5];
        //
        LevelManager lm = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        transform.parent = lm.transform;
        if (myData.parentID != -1)
        {
            transform.parent = lm.allAster[myData.parentID].transform;
        }
        transform.localPosition = myData.myPosition;
        GetComponent<BasicAster>().id = myData.ID;
        GetComponent<BasicAster>().isClockwise = myData.isClockwise;

        if (myData.ownerTeamID != -1) {
            GetComponent<Capturable>().Init(myData.ownerTeamID);
        }
    }
    [PunRPC]
    public void AddToList()
    {
        GameObject.Find("LevelManager").GetComponent<LevelManager>().allAster.Add(gameObject);
    }
}
