using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    public Image img;
    public void SetEnergy(float energy) {
        img.fillAmount = energy / 100.00f;
    }
}
