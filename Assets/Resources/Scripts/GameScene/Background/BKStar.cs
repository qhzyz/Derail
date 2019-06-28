using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BKStar : MonoBehaviour
{
    public static float maxInterTime=10;
    public static float minInterTime=5;
    float interTime =5;
    System.Random rd;
    // Start is called before the first frame update
    void Start()
    {
        //System.Random 
        rd = new System.Random(GMManager.GetRandomSeed());
        interTime = rd.Next(5, 10);
    }

    // Update is called once per frame
    void Update()
    {
        if ((interTime -= Time.deltaTime) <= 0) {
           // System.Random rd = new System.Random(MakeBackground.GetRandomSeed());
            transform.position
                    = new Vector3((float)rd.NextDouble() * 100 - 50, (float)rd.NextDouble() * 100 - 50, transform.position.z);
            interTime = rd.Next(5, 10);
        }
    }
}
//forbid