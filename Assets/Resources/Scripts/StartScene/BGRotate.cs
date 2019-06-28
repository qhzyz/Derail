using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGRotate : MonoBehaviour
{
    [SerializeField]
    private float rotateV = 3;
    public Vector3 startAngle;
    private Vector3 currentAngle;

    // Start is called before the first frame update
    void Start()
    {
        currentAngle = startAngle;
    }

    // Update is called once per frame
    void Update()
    {
        currentAngle.z+=rotateV*Time.deltaTime;
        transform.localEulerAngles = currentAngle;
    }
}
