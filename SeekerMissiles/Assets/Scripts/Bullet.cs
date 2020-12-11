using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject impactFX;
    private GameObject fx;
    private void OnCollisionEnter(Collision col)
    {
        fx = Instantiate(impactFX, transform.position, transform.rotation, transform);
        Destroy(gameObject, 0.05f);
    }
    private void OnDestroy()
    {
        fx.transform.parent = null;
    }
}
