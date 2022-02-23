using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Rotate2 : MonoBehaviour
{
    /*
     * Bazowa forma:
     * S1 -> 60 (a = 30)
     * S2 -> 45 (b = 45)
     * l = 5cm
     * h1 = cos(a) * l
     * h2 = sin(a+ b) * l
     * H = l * (cos(a) + sin (a+b)) = l * 1.83
     * b = asin(H/l - cos(a)) - a
     * S2 = b
     */

    public Transform Master;
    Vector3 b;//base position
    public float h = 1.7f;


    // Start is called before the first frame update
    void Start()
    {
        b = transform.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if(Master.hasChanged)
        {
            Master.hasChanged = false;
            float z = Calculate(Master.localEulerAngles.z);
            transform.localEulerAngles = new Vector3(b.x,b.y,z);
        }
    }
    public float Calculate(double s)
    {
        double b, rad = 180 / Math.PI,delta,cos,h2;
        s -= 90;
        cos = Math.Cos(s / rad);
        h2 = h - cos;//h2/l = sin(a+b)
        b = Math.Asin(h2) * rad - s;
        delta = cos + Math.Sin((b + s) / rad) - h;
        if(delta>0.01)
        {
            b = Math.Asin(h2 + delta) * rad - s;
        }
        return (float)b;
    }
}
