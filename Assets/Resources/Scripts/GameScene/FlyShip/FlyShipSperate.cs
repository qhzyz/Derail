using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyShipSperate : MonoBehaviour
{
    [SerializeField]
    private static float sperateForce = 2.5f;
    private static float enableR = 2;
    private Vector3 sperateDir;
    private bool isEnable;
    [SerializeField]
    private float maxPower;
    [SerializeField]
    private float power;
    public float debug;
    
    void Start()
    {
        power = maxPower;
        sperateDir = new Vector3(0, 0, 0);
        isEnable = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!transform.parent.GetComponent<PhotonView>().IsMine)
        {
            return;
        }
        RestorePower();
        operateSeprate();
    }
    void operateSeprate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            List<Vector3> tempL = new List<Vector3>();
            foreach (GameObject i in GameObject.Find("LevelManager").GetComponent<LevelManager>().allAster)
                tempL.Add((transform.position - i.transform.position)*(1-i.GetComponent<SphereCollider>().radius/ 
                    (transform.position - i.transform.position).magnitude));
            tempL.Sort((left, right) =>
            {
                if (left.magnitude > right.magnitude)
                    return 1;
                else if (left.magnitude == right.magnitude)
                    return 0;
                else
                    return -1;
            });
            if (tempL[0].magnitude < enableR && tempL[0].magnitude != 0) { isEnable = true; sperateDir = tempL[0].normalized; }
            else { isEnable = false; }
        }
        if (Input.GetKey(KeyCode.Space))
        {
            if (isEnable)
            {
                transform.parent.GetComponent<Rigidbody>().AddForce(sperateDir * sperateForce);
                if (power > 0) {
                    power -= 1;
                    if (power <= 0) { power = 0;isEnable = false; }
                }
            }
        }
        if(Input.GetKeyUp(KeyCode.Space))
        {
            isEnable = false;
        }
    }
    void RestorePower()
    {
        if (!isEnable && power < maxPower)
        {
            power += 0.5f;
            if (power > maxPower)
                power = maxPower;
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        debug += 1;
    }
}
