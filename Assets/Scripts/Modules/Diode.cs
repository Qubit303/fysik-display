using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diode : MonoBehaviour
{
    public void SetLightRadius(float radius)
    {
        transform.localScale = new Vector3(radius, radius, 1);
    }
}
