﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFlight : MonoBehaviour
{
    public float thrust, maxThrust, pitchSpeed, yawSpeed, rollSpeed, gravModNoLift, liftAngleMax, dragAngle, rollMod;
    [Range(0f, 1f)]
    public float alignVelocity;
    public Rigidbody rb;
    private float cThrust;
    private bool auto;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) auto = !auto;
        float pitch, yaw, roll;
        pitch = Input.GetAxis("Pitch") * pitchSpeed;
        roll = -Input.GetAxis("Roll") * rollSpeed;
        yaw = Input.GetAxis("Yaw") * yawSpeed;
        cThrust = auto  ? 1 : Mathf.Max(Input.GetAxis("Thrust"), 0);
        //new
        float velAngle = Vector3.Angle(rb.velocity, transform.forward);
        float turnDrag = Mathf.Max(0.1f, 1 - velAngle / dragAngle);
        print(turnDrag);
        Vector3 turn = new Vector3(pitch * turnDrag, yaw * turnDrag, roll * Mathf.Min(turnDrag * 2, 1)) * Time.deltaTime;
        transform.Rotate(turn);
            
    }
    void FixedUpdate()
    {
        float liftAngle = Vector3.Angle(transform.up, Vector3.up);

        float liftMod = 0;
        float liftMod2 = Mathf.PingPong(liftAngle/90,1);
        float boostMod = Input.GetKey(KeyCode.LeftShift) ? 3 : 1;
        


        if (liftAngle > liftAngleMax && liftAngle < 180 - liftAngleMax) liftMod = gravModNoLift;

        //rb.AddForce(transform.forward * thrust, ForceMode.Acceleration);
        Vector3 vel = Vector3.Lerp(rb.velocity, transform.forward * thrust * cThrust * boostMod, alignVelocity * Time.fixedDeltaTime);
        vel.y -= 9.8f * Time.fixedDeltaTime;
        rb.velocity = vel;
        rb.AddForce(Vector3.up * -9.81f * Time.fixedDeltaTime * (liftMod * liftMod2) + (transform.right * -Input.GetAxis("Roll") * rollSpeed * rollMod) * Mathf.Max(Input.GetAxis("Thrust"), 0), ForceMode.Impulse);
        // max forward vs max total etc

    }




}
