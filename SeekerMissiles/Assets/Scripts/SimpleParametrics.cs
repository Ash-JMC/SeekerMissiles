using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleParametrics : MonoBehaviour
{
    public float p, t;
    public Vector3 p3;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        t = Time.time * 2;
        //p3 = new Vector3(t,  Mathf.Cos(t),  Mathf.Sin(t));
        p3 = new Vector3(0, t * Mathf.Sin(t), t * Mathf.Cos(t));
        transform.position = p3;
    }
}
