using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyShipControl : MonoBehaviour {
    [SerializeField]
    float selfForce;
    public Vector3 adjustModelAngle;
    private ParticleSystem weapon1;
    private float preFireTime;
    [SerializeField]
    private float fireInterTime = 0.05f;
    // Use this for initialization
    void Start () {
        preFireTime = 0;
        weapon1 = transform.Find("WeaponSystem").Find("weapon1").GetComponent<ParticleSystem>();
     }
	
	// Update is called once per frame
	void Update ()
    {
        if (!GetComponent<PhotonView>().IsMine)
        {
            return;
        }
        OperateFire();
        OperateMove();
    }
    private void FixedUpdate()
    {
    }
    void OperateMove()
    {
        Vector3 uf = Vector3.zero;
        if (Input.GetKey(KeyCode.A))
        {
            transform.GetComponent<Rigidbody>().AddForce(-GMManager.RIGHT_DIR * selfForce);
            uf += (-GMManager.RIGHT_DIR * selfForce);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.GetComponent<Rigidbody>().AddForce(GMManager.RIGHT_DIR  * selfForce);
            uf += (GMManager.RIGHT_DIR * selfForce);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.GetComponent<Rigidbody>().AddForce(-GMManager.UP_DIR * selfForce);
            uf += (-GMManager.UP_DIR * selfForce);
        }
        else if (Input.GetKey(KeyCode.W))
        {
            transform.GetComponent<Rigidbody>().AddForce(GMManager.UP_DIR * selfForce);
            uf += (GMManager.UP_DIR * selfForce);
        }
        AdjustHead(uf);
    }
    void OperateFire() {
        if (Input.GetMouseButton(0)) {
            GetComponent<PhotonView>().RPC("Fire",RpcTarget.All);
        }
    }
    void AdjustHead(Vector3 dir)
    {
        Vector2 direction = new Vector2(-dir.x, dir.y);
        if (direction != new Vector2(0, 0))
        {   
            float angle = 360 - Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(angle, 0, 0)+adjustModelAngle;
        }
    }

    [PunRPC]
    public void Fire() {
        if (Time.time - preFireTime > fireInterTime)
        {
            if(!weapon1.GetComponent<AudioSource>().isPlaying)
                weapon1.GetComponent<AudioSource>().Play();
            weapon1.Emit(1);
            preFireTime = Time.time;
        }
    }
}
