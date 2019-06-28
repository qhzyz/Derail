using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    float currentValue;
    float realValue;
    float decSpeed;
    public RectTransform Bar;
    public RectTransform BarReal;
    void Start()
    {
        decSpeed = 10;
        realValue = currentValue = 100;
    }
    
    void Update()
    {
        if (currentValue > realValue)
        {
            currentValue -= (decSpeed++ * Time.deltaTime);
            UpdateWidthWithCurrentValue();
        }
        else if (currentValue < realValue)
        {
            currentValue = realValue;
            UpdateWidthWithCurrentValue();
        }
        else
            decSpeed = 10;
    }

    void UpdateWidthWithCurrentValue() {
        Bar.GetComponent<Image>().fillAmount = currentValue / 100.00f;
    }

    public void SetHp(float hp) {
        realValue = hp;
        BarReal.GetComponent<Image>().fillAmount = hp / 100.00f;
        BarReal.GetComponent<Image>().color = new Color((255-hp*155/100)/255.00f,(100+hp*155/100) / 255.00f, 0);
    }
}
