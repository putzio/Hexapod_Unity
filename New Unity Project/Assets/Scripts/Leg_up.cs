using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leg_up : MonoBehaviour
{    
    /*
     * state == 0 -> nothing
     * state == 1 -> knee up, disable knee moves
     * state == 2 -> knee down, enable knee moves
     */

    public int state = 0;
    public float speed = 70;
    public bool done =true;
    float startTime;
    float journeyLength;
    Vector3 positionForward;
    Vector3 positionStart;
    public int[] positions = { 0, 45 };

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {        
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
            if(state == 2)
                GetComponent<Rotate2>().enabled = true;
            state = 0;
            done = true;
        }
    }
    public void StateUp()
    {
        state = 1;
        GetComponent<Rotate2>().enabled = false;
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
