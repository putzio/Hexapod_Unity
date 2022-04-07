using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class RotateSlave : MonoBehaviour
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

    /*
    * state == 0 -> nothing
    * state == 1 -> knee up, disable knee moves
    * state == 2 -> knee down, enable knee moves
    */

    public int state = 0;
    public float speed = 70;
    public bool done = true;
    float startTime;
    float journeyLength;
    Vector3 positionForward;
    Vector3 positionStart;
    public int[] positions = { 0, 45 };

    // Start is called before the first frame update
    void Start()
    {
        b = transform.localEulerAngles;
        startTime = Time.time;
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
        float fracJourney = 0f;

        if (journeyLength == 0)
        {
            journeyLength = 0.001f;
        }

        if (state > 0)
        {

            float distCovered = (Time.time - startTime) * speed;
            fracJourney = distCovered / journeyLength;
            transform.localEulerAngles = Vector3.Lerp(positionStart, positionForward, fracJourney);

        }

        if (fracJourney > 0.9999)
        {
            if (state == 2)
                enabled = true;
            state = 0;
            done = true;
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

    public void StateUp()
    {
        state = 1;
        enabled = false;
        StateInit();
    }
    public void StateDown()
    {
        state = 2;
        StateInit();
    }
    void StateInit()
    {
        done = false;
        startTime = Time.time;
        positionStart = transform.localEulerAngles;
        positionForward = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, positions[state - 1]);
        journeyLength = Vector3.Distance(transform.localEulerAngles, positionForward);
    }
}
