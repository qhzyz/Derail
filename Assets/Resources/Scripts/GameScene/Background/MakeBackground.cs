using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeBackground : MonoBehaviour
{
    public GameObject pbStar;
    public int startInter = 10;
    public int layerInter = 100;
    public int layerNum = 3;
    public int kLayer = 50;
    public int startNum = 50;
    // Start is called before the first frame update
    void Start()
    {
        //pbStar = Resources.Load("Prefabs/BKstar") as GameObject;
        for (int i = 0; i < layerNum; i++)
        {
            System.Random rd = new System.Random(GMManager.GetRandomSeed());
            for (int j = 0; j < startNum + i * kLayer; j++)
            {
                Instantiate(pbStar, transform).transform.position
                    = new Vector3((float)rd.NextDouble() * BKBorder.w*2- BKBorder.w, 
                                  (float)rd.NextDouble() * BKBorder.h*2 - BKBorder.h,
                                  startInter + layerInter * i);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
