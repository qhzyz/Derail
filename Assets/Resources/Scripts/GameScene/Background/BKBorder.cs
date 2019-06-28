using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BKBorder : MonoBehaviour
{
    static public int w = 60;
    static public int h = 50;
    static public int stick = 5;
    // Start is called before the first frame update
    void Start()
    {
        BoxCollider temp = gameObject.AddComponent<BoxCollider>();
        temp.size = new Vector3(w, stick, stick); temp.center = new Vector3(0, -h / 2, 0);
        temp = gameObject.AddComponent<BoxCollider>();
        temp.size = new Vector3(w, stick, stick); temp.center = new Vector3(0, h / 2, 0);
        temp = gameObject.AddComponent<BoxCollider>();
        temp.size = new Vector3(stick, h, stick); temp.center = new Vector3(-w / 2, 0, 0);
        temp = gameObject.AddComponent<BoxCollider>();
        temp.size = new Vector3(stick, h, stick); temp.center = new Vector3(w / 2, 0, 0);

        LineRenderer borderLine = GetComponent<LineRenderer>();
        borderLine.positionCount = 5;
        Vector3[] borderPoints = new Vector3[5];
        borderPoints[0] = new Vector3(w/2 - stick / 2, h/2 - stick / 2);
        borderPoints[1] = new Vector3(-w/2 + stick / 2, h/2 - stick / 2);
        borderPoints[2] = new Vector3(-w/2 + stick / 2, -h/2 + stick / 2);
        borderPoints[3] = new Vector3(w/2 - stick / 2, -h/2 + stick / 2);
        borderPoints[4] = new Vector3(w/2 - stick / 2, h/2 - stick / 2);
        borderLine.SetPositions(borderPoints);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
