using System.Collections;
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
    public Transform[] barrels;
    public bool flak;
    [Range(0.0f, 0.75f)]
    public float flakFuseDeviation;

    public float bulletVel, bulletLife, bulletSpread, fireRate, maxRange, turretTurn, maxAngle, gravity = -9.85f;
    private Rigidbody RB;

    //Weapon stuff
    private float nextFire;
    private int barrelNum = 0;

    [Header("Tracking Quality")]
    public int checkCycles = 2;
    

    [Header("Pretty Stuff")]
    public float maxFlash;
    public bool drawTarget;
    private LineRenderer lr;
    private Light[] barrelLights;
    void Start()
    {
        tRotOld = target.rotation;
        lr = GetComponent<LineRenderer>();
        if (drawTarget) lr.positionCount = 3;
        barrelLights = new Light[barrels.Length];
        for(int i = 0; i < barrels.Length; i++)
        {
            barrelLights[i] = barrels[i].GetComponentInChildren<Light>(true);
            barrelLights[i].gameObject.SetActive(false);
        }
    }


    void Update()
    {
        if(Vector3.Distance(target.position, transform.position) <= maxRange)
        {
            GetTargetDeltas();
            Aim();
            Shoot();
        }
        

    }

    void GetTargetDeltas()
    {
        tVel = tRB.velocity * 1.1f;
        tVelMag = tVel.magnitude;
        distance = Vector3.Distance(transform.position, target.position);
        ttt = distance / bulletVel;
        tRot = target.rotation * Quaternion.Inverse(tRotOld);
        tRotOld = target.rotation;

        tVelNew = Quaternion.LerpUnclamped(Quaternion.identity, tRot, ttt / Time.deltaTime) * tVel;
        gravOffset = (Vector3.up + (target.forward * .25f)) * gravity * ttt;

        tPos = target.position + (tVelNew * ttt) + gravOffset;

        for (int i = 0; i < checkCycles; i++)
        {
            distance = Vector3.Distance(transform.position, tPos);
            ttt = distance / bulletVel;
            tVelNew = Quaternion.LerpUnclamped(Quaternion.identity, tRot, ttt / Time.deltaTime) * tVel;
            gravOffset = Vector3.down * gravity * ttt;

            tPos = target.position + (tVelNew * ttt) + gravOffset *1.1f;
        }

        if (drawTarget)
        {
            lr.SetPosition(0, target.position);
            lr.SetPosition(1, tPos);
            lr.SetPosition(2, transform.position);
        }
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
            Transform activeBarrel = barrels[barrelNum];
            barrelNum = barrelNum < barrels.Length - 1 ? barrelNum + 1 : 0;
            StartCoroutine(MuzzleFlash(barrelNum));
            GameObject b = Instantiate(bullet, activeBarrel.position, activeBarrel.rotation);
            Rigidbody bRB = b.GetComponent<Rigidbody>();
            Vector3 spread = Vector3.zero;
            spread.x = Random.Range(-bulletSpread, bulletSpread);
            spread.y = Random.Range(-bulletSpread, bulletSpread);
            b.transform.Rotate(spread);
            bRB.AddForce(b.transform.forward * bulletVel, ForceMode.VelocityChange);

            Destroy(b, flak ? ttt * Random.Range(1-flakFuseDeviation, 1+ flakFuseDeviation) : bulletLife);
        }
        

    }
    IEnumerator MuzzleFlash(int bNum)
    {
        barrelLights[bNum].gameObject.SetActive(true);
        yield return new WaitForSeconds(Mathf.Min(fireRate / 2, maxFlash));
        barrelLights[bNum].gameObject.SetActive(false);
    }

}
