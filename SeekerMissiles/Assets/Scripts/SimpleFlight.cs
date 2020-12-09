using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFlight : MonoBehaviour
{
    public float thrust, pitchSpeed, yawSpeed, rollSpeed;
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
        rb.velocity = transform.forward * thrust;
    }
}
