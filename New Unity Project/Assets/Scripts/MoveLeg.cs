using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeg : MonoBehaviour
{
    public int[] positions = { 80, 120 };
    public GameObject slave;
    public float speed = 70;
    public int state = 0;
    int lastPosition = 0;       
    float startTime;
    float journeyLength;
    Vector3 positionForward;
    Vector3 positionStart;
    bool wait = false;
    public int[] ForBackRanges = { 0, 26, 10, 55 };
    bool localControl = true;
    public bool done = true;
    int controlledPosition = 80;


    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        
        ForBackRanges[1] = (int)slave.GetComponent<RotateSlave>().Calculate(positions[0]);
        ForBackRanges[3] = (int)slave.GetComponent<RotateSlave>().Calculate(positions[1]);
        ForBackRanges[0] = ForBackRanges[1] - 35;
        ForBackRanges[2] = ForBackRanges[3] - 35;
        for(int i = 0; i< 4; i++)
        {
            if (ForBackRanges[i] < 0)
                ForBackRanges[i] = 0;
        }        
    }

    // Update is called once per frame
    void Update()
    {
        if(wait)
        {
            if (slave.GetComponent<Leg_up>().done)
            {
                wait = false;
                StateInit();
            }
        }
        else
        {
            float fracJourney = 0f;

            if (journeyLength == 0)
            {
                journeyLength = 0.001f;
            }

            if (state > 0 || done == false)
            {

                float distCovered = (Time.time - startTime) * speed;
                fracJourney = distCovered / journeyLength;
                transform.localEulerAngles = Vector3.Lerp(positionStart, positionForward, fracJourney);

            }

            if (fracJourney > 0.9999)
            {
                if (!localControl)
                {
                    done = true;
                    state = 0;
                }

                if (localControl)
                    OnPosition();                
            }
        }        
    }
    public void StateSwFor()
    {
        state = 1;
        localControl = true;
        slave.GetComponent<Leg_up>().positions[0] = 0;
        slave.GetComponent<Leg_up>().positions[1] = 26;
        StateInit();
    }
    public void StateSwBack()
    {
        state = 2;
        localControl = true;
        slave.GetComponent<Leg_up>().positions[0] = 10;
        slave.GetComponent<Leg_up>().positions[1] = 55;
        StateInit();
    }
    public void ControlledMove(MoveBody.M step, MoveBody.Mode mode)
    {
        if (mode == MoveBody.Mode.Forward)
        {
            slave.GetComponent<Leg_up>().positions[0] = ForBackRanges[0];
            slave.GetComponent<Leg_up>().positions[1] = ForBackRanges[1];

            if (step == MoveBody.M.Forward)
                controlledPosition = positions[0];
            else if (step == MoveBody.M.Back)
                controlledPosition = positions[1];
            else
                return;
        }
        else if (mode == MoveBody.Mode.Back)
        {
            slave.GetComponent<Leg_up>().positions[0] = ForBackRanges[2];
            slave.GetComponent<Leg_up>().positions[1] = ForBackRanges[3];

            if (step == MoveBody.M.Forward)
                controlledPosition = positions[0];
            else if (step == MoveBody.M.Back)
                controlledPosition = positions[1];
            else
                return;
        }
        wait = false;
        done = false;
        localControl = false;
        ControlledStateInit();
    }
    void ControlledStateInit()
    {
        startTime = Time.time;
        positionStart = transform.localEulerAngles;
        positionForward = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, controlledPosition);
        journeyLength = Vector3.Distance(transform.localEulerAngles, positionForward);
    }
    public void StateSwStop()
    {
        state = 0;
    }
    void StateInit()
    {
        startTime = Time.time;
        positionStart = transform.localEulerAngles;
        positionForward = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, positions[lastPosition]);
        journeyLength = Vector3.Distance(transform.localEulerAngles, positionForward);
    }
    void OnPosition()
    {
        if (state == 1)//F
        {
            if (lastPosition == 1)//max(120)
            {
                slave.GetComponent<Leg_up>().StateDown();
            }
            if (lastPosition == 0)//min(80)
            {
                slave.GetComponent<Leg_up>().StateUp();
            }
        }
        else//B
        {
            if (lastPosition == 1)//max(120)
            {
                slave.GetComponent<Leg_up>().StateUp();
            }
            if (lastPosition == 0)//min(80)
            {
                slave.GetComponent<Leg_up>().StateDown();
            }
        }
        if (lastPosition == 0)
            lastPosition = 1;
        else
            lastPosition = 0;
        if (!slave.GetComponent<Leg_up>().done)
        {
            wait = true;
        }
        if (slave.GetComponent<Leg_up>().done)
        {
            wait = false;
            StateInit();
        }
    }

}
