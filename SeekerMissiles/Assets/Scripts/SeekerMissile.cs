using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekerMissile : MonoBehaviour
{
    private Rigidbody rb;

    public Transform target;
    public Transform targetMesh;
    public Transform mesh;
    public float speed, turnSpeed, approachRange, approachMod;
    public Vector3 targetOffset;
    public GameObject boomLight;

    [Header("Target Prediction Guidance")]
    public bool pathPrediction;
    public float predictRange;
    public float turnPredict;
    public Rigidbody rbTarget;
    private LineRenderer lr;

    [Header("Ground Avoidance")]
    public bool avoidGround;
    public LayerMask groundLayers;
    public float checkDistance;
    public float minHeight;
    public float focusTargetRange;

    [Header("Payload")]
    public bool goBoom;
    public float boomRadius;
    public float boomPower;


    void Start()
    {
        if(mesh == null) mesh = transform.Find("_Mesh");
        rb = GetComponent<Rigidbody>();
        rbTarget = target.GetComponent<Rigidbody>();
        lr = GetComponent<LineRenderer>();

    }

    void FixedUpdate()
    {
        Vector3 moveDir = mesh.transform.forward * speed;
        rb.AddForce(moveDir, ForceMode.Acceleration);
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position + targetOffset, target.position);
        Vector3 intercept = pathPrediction ? GetIntercept(distance) : Vector3.zero;
        Vector3 groundOffset = avoidGround ? CheckGround(distance) : Vector3.zero;

        float turnMod = Mathf.Clamp(1 - (distance / approachRange), 0f, 1f);
        Vector3 dir = ((target.position + targetOffset + intercept) - (transform.position - groundOffset)).normalized;
        Quaternion lookRot = Quaternion.LookRotation(dir, Vector3.up);
        Quaternion stepRot = Quaternion.RotateTowards(transform.rotation, lookRot, Mathf.Lerp(turnSpeed, turnSpeed * approachMod, turnMod) * Time.deltaTime);
        transform.rotation = stepRot;
    }

    Vector3 GetIntercept(float distance)
    {
        float angle = Vector3.Angle(transform.forward, (target.position - transform.position).normalized);
        //float angleMod = Mathf.Clamp(angle / 90, 0f, 1f);

        Vector3 intercept = rbTarget.velocity / 2  + (Input.GetAxis("Horizontal") * transform.right * (turnPredict * rbTarget.velocity.magnitude));
        float interceptMod = angle < 130f ? Mathf.Clamp(distance / predictRange, 0f, 1f) : 0;
        //print(interceptMod);

        intercept = Vector3.Lerp(Vector3.zero, intercept, interceptMod);
        //intercept += Input.GetAxis("Horizontal") * targetMesh.right * (5 * interceptMod);
        lr.SetPosition(0, transform.position + transform.forward * 3);
        lr.SetPosition(1, target.position + intercept);

        
        return intercept;

    }

    void OnCollisionEnter(Collision hitInfo)
    {
        if(hitInfo.gameObject.tag == "Player")
        {
            StartCoroutine("Boom", hitInfo.contacts[0].point);
            print("Pew");
        }
    }

    IEnumerator Boom(Vector3 hitPos)
    {
        boomLight.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        //Vector3 boomDir = 
        if (goBoom) rbTarget.AddExplosionForce(boomPower, hitPos, boomRadius, 55);
        boomLight.SetActive(false);
    }

    Vector3 CheckGround(float distance)
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit, checkDistance, groundLayers))
        {
            Debug.DrawRay(transform.position, Vector3.down * hit.distance, Color.gray);
            float groundDist = hit.distance;
            float offsetHeight = Mathf.Clamp(groundDist, minHeight, groundDist);
            //float groundMod = Mathf.Clamp(distance / focusTargetRange , 0f, 1f);
            float groundMod = distance > focusTargetRange ? 1 : 0;

            print("GM: " + groundMod);
            offsetHeight *= groundMod;
            return Vector3.up * offsetHeight;
        }
        else return Vector3.zero;

    }
}
