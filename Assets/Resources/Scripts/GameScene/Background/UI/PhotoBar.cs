using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoBar : MonoBehaviour
{
    public Image img;
    public FlyShipState flyShipState;
    public void SetDeath(float v) {
        img.fillAmount = v;
    }
    private void Update() {
        if (!flyShipState)
            return;
        
        SetDeath(flyShipState.deathCounter / flyShipState.maxDeathInter);

        flyShipState.deathCounter -= Time.deltaTime;
        if (flyShipState.deathCounter <= 0) {
            flyShipState.deathCounter = 0;
        }
    }
}
