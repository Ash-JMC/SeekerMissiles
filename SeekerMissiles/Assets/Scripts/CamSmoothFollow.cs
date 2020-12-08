using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamSmoothFollow : MonoBehaviour
{
    
    public float followSpeed, alignSpeed;
    public bool lerpFollow;
    public float lerpBias;
    public float pitchBias;

    public Transform target;

    void Start()
    {
        target = transform.parent;
        transform.parent = null;
    }
    void LateUpdate()
    {
        Vector3 targetPos = lerpFollow ? Vector3.Slerp(transform.position, target.position, lerpBias) : Vector3.MoveTowards(transform.position, target.position, followSpeed * Time.deltaTime);
        transform.position = targetPos;

        Quaternion targetRot = Quaternion.LookRotation(target.forward, Vector3.up);
        Quaternion easedRot = Quaternion.Euler(Mathf.LerpAngle(targetRot.eulerAngles.x, transform.rotation.eulerAngles.x, pitchBias), targetRot.eulerAngles.y, targetRot.eulerAngles.z);
        Quaternion stepRot = Quaternion.RotateTowards(transform.rotation, easedRot, alignSpeed * Time.deltaTime);
        transform.rotation = stepRot;

    }
}
