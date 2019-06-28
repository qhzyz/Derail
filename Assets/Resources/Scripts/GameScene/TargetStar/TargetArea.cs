using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetArea : MonoBehaviour
{
    private float time;
    private float winTime;
    private bool isClock;
    // Start is called before the first frame update
    void Start()
    {
        time = 0f;
        winTime = 5f;
        isClock = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isClock)
            time += Time.deltaTime;
        if (time > winTime)
        {
            //LevelManager.SetWin();
        }
    }
    private void OnTriggerStay(Collider other)
    {

        if(other.transform.tag=="Target")
        {
            isClock = true;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        time = 0;
        isClock = false;
    }
}
