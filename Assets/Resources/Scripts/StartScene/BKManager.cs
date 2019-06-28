using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BKManager : MonoBehaviour
{
    [SerializeField]
    private float interTime = 5;
    [SerializeField]
    private static float existCount = 3;
    private float countTime;
    private List<GameObject> pbNear; 
    private List<GameObject> pbMid; 
    private List<GameObject> pbFar;

    void Start()
    {
        countTime = 0;
        pbNear = new List<GameObject>();
        pbMid = new List<GameObject>();
        pbFar = new List<GameObject>();
        pbNear.Add(Resources.Load("Prefabs/StartScene/near01") as GameObject);
        pbNear.Add(Resources.Load("Prefabs/StartScene/near02") as GameObject);
        pbNear.Add(Resources.Load("Prefabs/StartScene/near03") as GameObject);
        pbMid.Add(Resources.Load("Prefabs/StartScene/mid01") as GameObject);
        pbMid.Add(Resources.Load("Prefabs/StartScene/mid02") as GameObject);
        pbMid.Add(Resources.Load("Prefabs/StartScene/mid03") as GameObject);
        pbFar.Add(Resources.Load("Prefabs/StartScene/far01") as GameObject);
        pbFar.Add(Resources.Load("Prefabs/StartScene/far02") as GameObject);
        pbFar.Add(Resources.Load("Prefabs/StartScene/far03") as GameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if ((countTime += Time.deltaTime) >= interTime)
        {
            System.Random rd = new System.Random(GMManager.GetRandomSeed());
            int temp = (int)(rd.NextDouble() * existCount); 
            Instantiate(pbNear[temp], transform).GetComponent<BGRotate>().startAngle=new Vector3(0,0,(float)rd.NextDouble()*360);


            //rd = new System.Random(GMManager.GetRandomSeed());
            temp = (int)(rd.NextDouble() * existCount);
            Instantiate(pbMid[temp], transform).GetComponent<BGRotate>().startAngle = new Vector3(0, 0, (float)rd.NextDouble() * 360); ;


            //rd = new System.Random(GMManager.GetRandomSeed());
            temp = (int)(rd.NextDouble() * existCount);
            Instantiate(pbFar[temp], transform).GetComponent<BGRotate>().startAngle = new Vector3(0, 0, (float)rd.NextDouble() * 360); ;

            countTime = 0;
        }
    }
}
