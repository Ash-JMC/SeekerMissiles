using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFlight : MonoBehaviour
{
    public float thrust, maxThrust, pitchSpeed, yawSpeed, rollSpeed, gravModNoLift, liftAngleMax;
    [Range(0f, 1f)]
    public float alignVelocity;
    public Rigidbody rb;
    private float cThrust;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float pitch, yaw, roll;
        pitch = Input.GetAxis("Pitch") * pitchSpeed;
        roll = -Input.GetAxis("Roll") * rollSpeed;
        yaw = Input.GetAxis("Yaw") * yawSpeed;
        cThrust = Mathf.Max(Input.GetAxis("Thrust"), 0);
        Vector3 turn = new Vector3(pitch, yaw, roll) * Time.deltaTime;
        transform.Rotate(turn);
            
    }
    void FixedUpdate()
    {
        float liftAngle = Vector3.Angle(transform.up, Vector3.up);
        print(liftAngle);
        float liftMod = 0;
        float liftMod2 = Mathf.PingPong(liftAngle/90,1);
        print(liftMod2);

        if (liftAngle > liftAngleMax && liftAngle < 180 - liftAngleMax) liftMod = gravModNoLift;

        //rb.AddForce(transform.forward * thrust, ForceMode.Acceleration);
        Vector3 vel = Vector3.Lerp(rb.velocity, transform.forward * thrust * cThrust, alignVelocity * Time.fixedDeltaTime);
        vel.y -= 9.8f * Time.fixedDeltaTime;
        rb.velocity = vel;
        rb.AddForce(Vector3.up * -9.81f * Time.fixedDeltaTime * (liftMod * liftMod2), ForceMode.Impulse);
        // max forward vs max total etc

    }




}
