using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGLife : MonoBehaviour
{
    [SerializeField]
    private float lifeTime = 5;
    private bool isDie;
    private float dieCount;
    private float inCount;
    // Start is called before the first frame update
    void Start()
    {
        isDie = false;
        dieCount = 100;
        inCount = 100;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Color temp = GetComponent<SpriteRenderer>().color;
        temp.a = dieCount / 100.0000f - inCount / 100.0000f;
        GetComponent<SpriteRenderer>().color = temp;

        if (inCount > 0)
            inCount--;

        if ((lifeTime -= Time.deltaTime) <= 0)
            isDie = true;
        if (isDie)
            dieCount--;
        if (dieCount <= 0)
            Destroy(gameObject);
    }
}
