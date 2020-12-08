using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            StartCoroutine("CD");
        }
    }

    IEnumerator CD()
    {
        print(3);
        yield return new WaitForSeconds(1);
        print(2);
        yield return new WaitForSeconds(1);
        print(1);
        yield return new WaitForSeconds(1);
        print(0);
    }
}
