using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyShipGravaty : MonoBehaviour
{
    [SerializeField]
    static float forceSrcNum;
    Transform asterParent;
    GameObject[] forceSrcArr;

    private float w;
    // Start is called before the first frame update
    void Start()
    {

    }
    private void FixedUpdate()
    {
        if (!GetComponent<PhotonView>().IsMine)
        {
            return;
        }
        foreach (GameObject i in GameObject.Find("LevelManager").GetComponent<LevelManager>().allAster)
            SetGravity(i.transform);
    }
    void SetGravity(Transform who)
    {
        if (who.position == transform.position) { return; }
        GetComponent<Rigidbody>().AddForce(
            GMManager.G * who.GetComponent<Rigidbody>().mass * transform.GetComponent<Rigidbody>().mass /
            (transform.position - who.position).magnitude / (transform.position - who.position).magnitude *
            (who.position - transform.position).normalized
            );
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
