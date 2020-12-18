using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotTest : MonoBehaviour
{
    public Vector3 eulerRot;   // Some Vector3 euler rotation you set in the inspector
    public float speed;
    private bool on;
    
    void Update()
    {
        if (Input.GetKeyDown("space") && !on) on = true;

        if(on)
        {
            Quaternion targetRot = Quaternion.Euler(eulerRot);
            // OR
            //Quaternion targetRot = Quaternion.FromToRotation(Vector3.up, Vector3.right);
            //Rotate from an upright position, to 90 degrees (right)

            Quaternion stepRot = Quaternion.RotateTowards(transform.rotation, targetRot, speed * Time.deltaTime);
            transform.rotation = stepRot;
        }
        print("Update");
    }
    private void FixedUpdate()
    {
        print("Fixed Update");
    }

}
