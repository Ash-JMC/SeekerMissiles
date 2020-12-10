using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekerMissile_2 : MonoBehaviour
{
    [Header("Target")]
    public Transform target;
    public Rigidbody tRB;

    //Tracking stuff
    private Vector3 tVel, tVelNew, tPos;
    private Quaternion tRot, tRotOld;
    private float tVelMag, distance, ttt, ttt2;


    [Header("Engine Specs")]
    public float thrust, turn;
    private Rigidbody RB;

    [Header("Tracking Quality")]
    public int checkCycles = 2;

    [Header("Pretty Stuff")]
    public GameObject pointLight;
    public ParticleSystem boomFX;
    public float boomPow, boomFallOff, lightIntensity;
    private Vector3 boomVec;
    private LineRenderer lr;

    void Start()
    {
        RB = GetComponent<Rigidbody>();
        tRotOld = target.rotation;
        lr = GetComponent<LineRenderer>();
    }

    void Update()
    {
        GetTargetDeltas();
        GetIntercept();
        Turn();
    }

    void FixedUpdate()
    {
        //RB.AddForce(transform.forward * thrust, ForceMode.Acceleration);
        RB.velocity = transform.forward * thrust + boomVec;
    }

    void GetTargetDeltas()
    {
        tVel = tRB.velocity;
        tVelMag = tVel.magnitude;
        distance = Vector3.Distance(transform.position, target.position);
        ttt = distance / RB.velocity.magnitude;
        tRot = target.rotation * Quaternion.Inverse(tRotOld);
        tRotOld = target.rotation;

        if (boomVec != Vector3.zero) boomVec = Vector3.MoveTowards(boomVec, Vector3.zero, boomFallOff * Time.deltaTime);
        
    }

    void GetIntercept()
    {
        tVelNew = Quaternion.LerpUnclamped(Quaternion.identity, tRot, ttt / Time.deltaTime) * tVel;
        tPos = target.position + (tVelNew * ttt);

        for (int i = 0; i < checkCycles; i++)
        {
            distance = Vector3.Distance(transform.position, tPos);
            ttt = distance / RB.velocity.magnitude;
            tVelNew = Quaternion.LerpUnclamped(Quaternion.identity, tRot, ttt / Time.deltaTime) * tVel;
            tPos = target.position + (tVelNew * ttt);
        }


        //Draw Intercept
        lr.SetPosition(0, target.position);
        lr.SetPosition(1, target.position + tVel * ttt);
        lr.SetPosition(2, tPos);
        lr.SetPosition(3, transform.position);


    }

    void Turn()
    {
        Vector3 dir = ((tPos) - (transform.position)).normalized;
        Quaternion lookRot = Quaternion.LookRotation(dir, Vector3.up);
        Quaternion stepRot = Quaternion.RotateTowards(transform.rotation, lookRot, turn * Time.deltaTime);
        transform.rotation = stepRot;
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "Player")
        {
            StopCoroutine("Boom");
            print("Boom");
            StartCoroutine("Boom");
        }
    }
    IEnumerator Boom()
    {
        pointLight.SetActive(true);
        Light l = pointLight.GetComponent<Light>();
        l.intensity = lightIntensity;
        boomVec += Random.onUnitSphere * boomPow;
        boomFX.Play();

        while (l.intensity > 0)
        {
            l.intensity = Mathf.MoveTowards(l.intensity, 0, lightIntensity / .6f * Time.deltaTime);
            yield return null;
        }
        
        pointLight.SetActive(false);
    }
}
