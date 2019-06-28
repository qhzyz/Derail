using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScale : MonoBehaviour
{
    [SerializeField]
    private float scaleV=3;
    [SerializeField]
    private Vector3 startScale;
    private Vector3 currentScale;

    // Start is called before the first frame update
    void Start()
    {
        startScale = new Vector3(0.6f, 0.6f, 0.6f);
        currentScale = startScale;
    }

    // Update is called once per frame
    void Update()
    {
        scaleV += (0.01f * Time.deltaTime);
        currentScale.x += scaleV * Time.deltaTime;
        currentScale.y += scaleV * Time.deltaTime;
        transform.localScale = currentScale;
    }
}
