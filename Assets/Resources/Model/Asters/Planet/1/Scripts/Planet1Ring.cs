using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet1Ring : MonoBehaviour
{
    public float rotateSpeed = 1;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(transform.parent.parent.position,GMManager.AXIS_Z,rotateSpeed);
    }
}
