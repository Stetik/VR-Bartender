using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchGroundDestroy : MonoBehaviour
{
    public double deadzone = 0.5;
    void Update()
    {
        if (transform.position.y < deadzone)
        {
            Destroy(gameObject);
        }
    }
}
