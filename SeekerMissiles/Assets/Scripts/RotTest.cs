using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotTest : MonoBehaviour
{
    public float angle;
    public Vector3 targetRot;
    public float speed;
    private bool on;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space") && !on)
        {
            on = true;
        }

        if(on)
        {
            Quaternion lookRot = Quaternion.LookRotation(targetRot);
            Quaternion stepRot = Quaternion.RotateTowards(transform.rotation, lookRot, speed * Time.deltaTime);
            transform.rotation = stepRot;
        }
    }

}
