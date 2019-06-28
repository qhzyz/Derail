using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetAster : BasicAster
{
	// Use this for initialization
	public override void Start () {
        base.Start();
        asterParent = transform.parent;
        if (transform.parent)
        {
            w = Mathf.Sqrt(asterParent.GetComponent<Rigidbody>().mass * GMManager.G
           / Mathf.Pow((transform.position - asterParent.transform.position).magnitude, 3f));
        }
    }

    // Update is called once per frame
    public override void Update () {
        base.Update();
	}
}
