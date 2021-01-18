using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject impactFX;
    public bool flak;
    private bool hit;

    private void OnCollisionEnter(Collision col)
    {
        if(!hit)
        {
            hit = true;
            Instantiate(impactFX, col.contacts[0].point - col.contacts[0].normal, transform.rotation, col.transform);
            Destroy(gameObject);

        }
    }
    private void OnDestroy()
    {
        if(flak && !hit) Instantiate(impactFX, transform.position, transform.rotation);
    }
}
