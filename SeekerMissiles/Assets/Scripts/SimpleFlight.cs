using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFlight : MonoBehaviour
{
    public float thrust, pitchSpeed, yawSpeed, rollSpeed;
    [Range(0f, 1f)]
    public float alignVelocity;
    public Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float pitch, yaw, roll;
        pitch = Input.GetAxis("Vertical") * pitchSpeed;
        roll = -Input.GetAxis("Horizontal") * rollSpeed;
        yaw = Input.GetAxis("Yaw") * yawSpeed;
        Vector3 turn = new Vector3(pitch, yaw, roll) * Time.deltaTime;
        transform.Rotate(turn);
            
    }
    void FixedUpdate()
    {
        //rb.AddForce(transform.forward * thrust, ForceMode.Acceleration);
        Vector3 vel = Vector3.Lerp(rb.velocity, transform.forward * thrust, alignVelocity);
        vel.y -= 9.8f * 2 * Time.fixedDeltaTime;
        rb.velocity = vel;
        
    }




}
