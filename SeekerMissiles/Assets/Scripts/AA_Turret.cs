﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AA_Turret : MonoBehaviour
{
    [Header("Target")]
    public Transform target;
    public Rigidbody tRB;

    //Tracking stuff
    private Vector3 tVel, tVelNew, tPos, gravOffset;
    private Quaternion tRot, tRotOld;
    private float tVelMag, distance, ttt, cAngle;

    [Header("Weapon Specs")]
    public GameObject bullet;
    public Transform barrel;
    public float bulletVel, bulletLife, gravity = -9.85f, fireRate, turretTurn, maxAngle;
    private Rigidbody RB;

    //Weapon stuff
    private float nextFire;

    [Header("Tracking Quality")]
    public int checkCycles = 2;

    [Header("Pretty Stuff")]
    private LineRenderer lr;


    void Start()
    {
        tRotOld = target.rotation;
        lr = GetComponent<LineRenderer>();
    }


    void Update()
    {
        GetTargetDeltas();
        Aim();
        Shoot();

    }

    void GetTargetDeltas()
    {
        tVel = tRB.velocity;
        tVelMag = tVel.magnitude;
        distance = Vector3.Distance(transform.position, target.position);
        ttt = distance / bulletVel;
        tRot = target.rotation * Quaternion.Inverse(tRotOld);
        tRotOld = target.rotation;

        tVelNew = Quaternion.LerpUnclamped(Quaternion.identity, tRot, ttt / Time.deltaTime) * tVel;
        gravOffset = Vector3.up * gravity * ttt;

        tPos = target.position + (tVelNew * ttt) + gravOffset;

        for (int i = 0; i < checkCycles; i++)
        {
            distance = Vector3.Distance(transform.position, tPos);
            ttt = distance / bulletVel;
            tVelNew = Quaternion.LerpUnclamped(Quaternion.identity, tRot, ttt / Time.deltaTime) * tVel;
            gravOffset = Vector3.down * gravity * ttt;

            tPos = target.position + (tVelNew * ttt) + gravOffset;
        }

        lr.SetPosition(0, target.position);
        lr.SetPosition(1, target.position + tVel * ttt);
        lr.SetPosition(2, tPos);
        lr.SetPosition(3, transform.position);
    }

    void Aim()
    {
        Vector3 dir = ((tPos) - (transform.position)).normalized;
        Quaternion lookRot = Quaternion.LookRotation(dir, Vector3.up);
        Quaternion stepRot = Quaternion.RotateTowards(transform.rotation, lookRot, turretTurn * Time.deltaTime);
        transform.rotation = stepRot;
        cAngle = Quaternion.Angle(transform.rotation, lookRot);
    }

    void Shoot()
    {
        if (Time.time > nextFire && cAngle <= maxAngle)
        {
            nextFire = Time.time + fireRate;
            GameObject b = Instantiate(bullet, barrel.position, barrel.rotation);
            Rigidbody bRB = b.GetComponent<Rigidbody>();
            bRB.AddForce(b.transform.forward * bulletVel, ForceMode.VelocityChange);
        }
        

    }

}