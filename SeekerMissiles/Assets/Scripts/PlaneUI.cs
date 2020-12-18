using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaneUI : MonoBehaviour
{
    public Slider slider;
    private float cThrust;

    void Update()
    {
        cThrust = Mathf.Max(Input.GetAxis("Thrust"), 0);
        slider.value = cThrust;

    }
}
