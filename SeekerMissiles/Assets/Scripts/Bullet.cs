using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject impactFX;
    private void OnCollisionEnter(Collision col)
    {
        Instantiate(impactFX, col.contacts[0].point - col.contacts[0].normal, transform.rotation, col.transform);
        Destroy(gameObject);
    }
}
