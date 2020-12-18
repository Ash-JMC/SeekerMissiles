using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CamSmoothFollowPlane : MonoBehaviour
{
    
    public float followSpeed, alignSpeed;
    public bool lerpFollow;
    public float lerpBias;
    public float pitchBias;
    public float maxPitch;

    public Transform target;
    private int flip;

    void Start()
    {
        target = transform.parent;
        transform.parent = null;
    }
    void Update()
    {
       if(Input.GetKeyDown(KeyCode.C))
        {
            flip = flip == 1 ? -1 : 1;
            transform.Rotate(Vector3.up * 90f * flip);
        }

        

    }
    void FixedUpdate()
    {
        Vector3 targetPos = lerpFollow ? Vector3.Slerp(transform.position, target.position, lerpBias) : Vector3.MoveTowards(transform.position, target.position, followSpeed * Time.deltaTime);
        transform.position = targetPos;

        Quaternion targetRot = Quaternion.LookRotation( target.forward, target.up);
        Quaternion easedRot = Quaternion.Euler(targetRot.eulerAngles.x, targetRot.eulerAngles.y, 0);
        //print(targetRot.eulerAngles.x);
        Quaternion stepRot = Quaternion.RotateTowards(transform.rotation, easedRot, alignSpeed * Time.deltaTime);
        transform.rotation = stepRot;
        //Mathf.LerpAngle(transform.rotation.x, targetRot.x, pitchBias)
    }
}
