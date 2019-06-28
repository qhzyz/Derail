using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CapturingLine : MonoBehaviour
{
    LineRenderer lineRenderer;
    public List<FlyShipState> nearbyShip;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameObject.Find("PlayerManager"))
            return;
        nearbyShip.Clear();
        PlayerManager pm = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        for (int i = 0; i < pm.allFlyShip.Count; i++)
        {
            float captureDistance = pm.allFlyShip[i].GetComponent<FlyShipState>().captureDistance;
            float currentDistance = (transform.position - pm.allFlyShip[i].transform.position).magnitude - gameObject.GetComponent<SphereCollider>().radius;
            if (pm.allFlyShip[i] && captureDistance > currentDistance&&pm.allFlyShip[i].activeSelf)
            {
                nearbyShip.Add(pm.allFlyShip[i].GetComponent<FlyShipState>());
            }
        }
        lineRenderer.positionCount = nearbyShip.Count * 2+1;
        for (int i = 0; i < nearbyShip.Count; i++)
        {
            lineRenderer.SetPosition(2 * i, transform.position);
            lineRenderer.SetPosition(2 * i+1, nearbyShip[i].transform.position);
        }
        lineRenderer.SetPosition(nearbyShip.Count * 2, transform.position);
    }
}
