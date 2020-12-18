using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightGimbleDisplay : MonoBehaviour
{
    public Transform target, gimble;
    public Vector3 mirrorAxis;
    
    void Update()
    {
        Vector3 tRot = target.rotation.eulerAngles;
        tRot = Vector3.Scale(tRot, mirrorAxis);
        gimble.localRotation = Quaternion.Euler(tRot);
    }
}
