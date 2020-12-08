using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public Rigidbody motorRB;

    private float moveInput;
    private float turnInput;
    private bool isCarGrounded;

    public float fwdSpeed;
    public float revSpeed;
    public float turnSpeed;
    public float alignSpeed;

    public Transform tyre_FL, tyre_FR, tyre_RL, tyre_RR;
    public float maxTyreTurn;

    public LayerMask groundLayer;

    public float airDrag;
    public float groundDrag;

    private TrailRenderer[] tyreTrails;
    private bool trails;

    void Start()
    {
        motorRB.transform.parent = null;
        tyreTrails = GetComponentsInChildren<TrailRenderer>();
    }


    void Update()
    {
        //Inputs
        moveInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");
        moveInput *= moveInput > 0 ? fwdSpeed : revSpeed;

        //Update Pos
        transform.position = motorRB.transform.position;

        //Steering
        float newRotation = turnInput * turnSpeed * Time.deltaTime * Input.GetAxis("Vertical");
        transform.Rotate(0, newRotation, 0, Space.World);
        //Turn tyres
        float tyreTurn = turnInput * maxTyreTurn;
        tyre_FL.localRotation = Quaternion.Euler(0, tyreTurn, 90);
        tyre_FR.localRotation = Quaternion.Euler(0, tyreTurn, 90);

        //Ground Check
        RaycastHit hit;
        isCarGrounded = Physics.Raycast(transform.position, -transform.up, out hit, 1f, groundLayer);
        //transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;

        //Align to surface
        Quaternion targetRot = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        Quaternion stepRot = Quaternion.RotateTowards(transform.rotation, targetRot, alignSpeed * Time.deltaTime);
        transform.rotation = stepRot;
        motorRB.drag = isCarGrounded ? groundDrag : airDrag;

        //Get angle between velocity and transform forward;
        TyreTrails();
        
    }
    void FixedUpdate()
    {
        if(isCarGrounded)
        {
            motorRB.AddForce(transform.forward * moveInput, ForceMode.Acceleration);
        }
        else
        {
            motorRB.AddForce(Vector3.up * -60f, ForceMode.Acceleration);
        }

    }

    void TyreTrails()
    {
        float driftAngle = Vector3.Angle(transform.forward, motorRB.velocity.normalized);


        if (isCarGrounded && driftAngle > 30)
        {
            if (!trails)
            {
                foreach (TrailRenderer tr in tyreTrails)
                {
                    tr.emitting = true;
                }
                trails = true;
            }
        }
        else
        {
            if (trails)
            {
                foreach (TrailRenderer tr in tyreTrails)
                {
                    tr.emitting = false;
                }
                trails = false;
            }
        }
    }
}
