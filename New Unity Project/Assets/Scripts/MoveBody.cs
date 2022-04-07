using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Timers;

public class MoveBody : MonoBehaviour
{
    public MoveLeg[] Legs;
    public string axis = "X";
    public float transVelocity = 1f;
    public bool active;
    public bool reset = false;
    uint distance = 45;
    Mode state = Mode.Stop;
    bool wait = false;
    float[] startPosition;
    public enum M
    {
        Zero,
        StartUp,
        StartWait,
        Back,
        Up,
        Forward,
        Down
    };
    public enum Mode
    {
        Stop = 0,
        Forward = 1,
        Back = 2,
        Left = 3,
        Right = 4
    }
    public M m1 = M.Zero;
    float time;
    public string forwardKey = "up", backKey = "down", leftKey = "left",rightKey = "right"; 

    // Start is called before the first frame update
    void Start()
    {
        startPosition = new float[6];
        startPosition[0] = transform.localPosition.x;
        startPosition[1] = transform.localPosition.y;
        startPosition[2] = transform.localPosition.z;
        startPosition[3] = transform.localEulerAngles.x;
        startPosition[4] = transform.localEulerAngles.y;
        startPosition[5] = transform.localEulerAngles.z;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(forwardKey))
        {
            Foreward();
        }
        else if(Input.GetKeyDown(backKey))
        {
            Back();
        }
        else if (Input.GetKeyDown(leftKey))
        {
            Left();
        }
        else if (Input.GetKeyDown(rightKey))
        {
            Right();
        }
        if (Input.GetKeyUp(forwardKey)||Input.GetKeyUp(backKey)|| Input.GetKeyUp(leftKey) || Input.GetKeyUp(rightKey))
        {
            Stop();
        }
        
        if (state>0 &&!wait)
        {
            StateMachine();
            if (state == Mode.Forward || state == Mode.Back)
            {
                //Update positions
                UpdateLinear();
            }
            if(state == Mode.Left || state == Mode.Right)
            {
                updateRotation();
            }
        }
    }
    void UpdateLinear()
    {
        float z = Legs[0].transform.localEulerAngles.z;
        Legs[3].transform.localEulerAngles = new Vector3(Legs[3].transform.localEulerAngles.x, Legs[3].transform.localEulerAngles.y, z);
        Legs[4].transform.localEulerAngles = new Vector3(Legs[4].transform.localEulerAngles.x, Legs[4].transform.localEulerAngles.y, z);
        z = Legs[1].transform.localEulerAngles.z;
        Legs[2].transform.localEulerAngles = new Vector3(Legs[2].transform.localEulerAngles.x, Legs[2].transform.localEulerAngles.y, z);
        Legs[5].transform.localEulerAngles = new Vector3(Legs[5].transform.localEulerAngles.x, Legs[5].transform.localEulerAngles.y, z);

        z = Legs[0].slave.transform.localEulerAngles.z;
        Legs[3].slave.transform.localEulerAngles = new Vector3(Legs[3].slave.transform.localEulerAngles.x, Legs[3].slave.transform.localEulerAngles.y, z);
        Legs[4].slave.transform.localEulerAngles = new Vector3(Legs[4].slave.transform.localEulerAngles.x, Legs[4].slave.transform.localEulerAngles.y, z);
        z = Legs[1].slave.transform.localEulerAngles.z;
        Legs[2].slave.transform.localEulerAngles = new Vector3(Legs[2].slave.transform.localEulerAngles.x, Legs[2].slave.transform.localEulerAngles.y, z);
        Legs[5].slave.transform.localEulerAngles = new Vector3(Legs[5].slave.transform.localEulerAngles.x, Legs[5].slave.transform.localEulerAngles.y, z);
    }
    void updateRotation()
    {
        float z = Legs[0].transform.localEulerAngles.z;
        Legs[4].transform.localEulerAngles = new Vector3(Legs[4].transform.localEulerAngles.x, Legs[4].transform.localEulerAngles.y, z);
        z = Legs[1].transform.localEulerAngles.z;
        Legs[5].transform.localEulerAngles = new Vector3(Legs[5].transform.localEulerAngles.x, Legs[5].transform.localEulerAngles.y, z);

        z = Legs[0].slave.transform.localEulerAngles.z;
        Legs[4].slave.transform.localEulerAngles = new Vector3(Legs[4].slave.transform.localEulerAngles.x, Legs[4].slave.transform.localEulerAngles.y, z);
        z = Legs[1].slave.transform.localEulerAngles.z;
        Legs[5].slave.transform.localEulerAngles = new Vector3(Legs[5].slave.transform.localEulerAngles.x, Legs[5].slave.transform.localEulerAngles.y, z);
    }
    public void ResetPosition()
    {
        transform.localPosition = new Vector3(startPosition[0], startPosition[1], startPosition[2]);
        transform.localEulerAngles = new Vector3(startPosition[3], startPosition[4], startPosition[5]);
    }
    public void Foreward()
    {
        Stop();
        state = Mode.Forward;
        m1 = M.StartUp;
        GetComponent<Move>().axis = "Z";
        GetComponent<Move>().active = 0;
        //disable the Rotation2 script, it could be enabled after getting back from Left() or Right()
        foreach(MoveLeg leg in Legs)
        {
            leg.slave.GetComponent<RotateSlave>().enabled = false;
        }
        //We disable the legs, which just copy the moves of the first two legs
        for (int i = 2; i < Legs.Length; i++)
        {
            Legs[i].slave.GetComponent<Leg_up>().enabled = false;
        }
    }
    public void Back()
    {
        Stop();
        state = Mode.Back;
        m1 = M.StartUp;
        GetComponent<Move>().axis = "Z";
        GetComponent<Move>().active = 0;
        //disable the Rotation2 script, it could be enabled after getting back from Left() or Right()
        foreach (MoveLeg leg in Legs)
        {
            leg.slave.GetComponent<RotateSlave>().enabled = false;
        }
        //We disable the legs, which just copy the moves of the first two legs
        for (int i = 2; i < Legs.Length; i++)
        {
            Legs[i].slave.GetComponent<Leg_up>().enabled = false;
        }
    }
    public void Stop()
    {
        state = Mode.Stop;
        m1 = M.Zero;
        GetComponent<Move>().active = 0;
        foreach(MoveLeg leg in Legs)
        {
            leg.slave.GetComponent<Leg_up>().StateDown();
            leg.ControlledMove(M.Back, Mode.Back);
        }

        UpdateLinear();
    }
    public void Left()
    {
        state = Mode.Left;
        m1 = M.StartUp;
        GetComponent<Move>().axis = "X";
        GetComponent<Move>().active = 0;
        //We disable the last two legs, which just copy the moves of the first two legs
        for (int i = 4; i < Legs.Length; i++)
        {
            Legs[i].slave.GetComponent<Leg_up>().enabled = false;
        }
        //Enable middle legs, now they have different moves
        for (int i = 2; i < Legs.Length-2; i++)
        {
            Legs[i].slave.GetComponent<Leg_up>().enabled = true;
        }
    }
    public void Right()
    {
        state = Mode.Right;
        m1 = M.StartUp;
        GetComponent<Move>().axis = "X";
        GetComponent<Move>().active = 0;
        //We disable the last two legs, which just copy the moves of the first two legs
        for (int i = 4; i < Legs.Length; i++)
        {
            Legs[i].slave.GetComponent<Leg_up>().enabled = false;
        }
        //Enable middle legs, now they have different moves
        for (int i = 2; i < Legs.Length-2; i++)
        {
            Legs[i].slave.GetComponent<Leg_up>().enabled = true;
        }
    }
    private void StateMachine()
    {
        switch (state)
        {
            case Mode.Forward:
                {
                    switch (m1)
                    {
                        case M.Zero:
                            {

                                break;
                            }
                        case M.StartUp:
                            {
                                //L1 Up
                                Legs[0].slave.GetComponent<Leg_up>().StateUp();
                                m1 = M.StartWait;
                                break;
                            }
                        case M.StartWait:
                            {
                                //if L1 is up, go forward
                                if (Legs[0].slave.GetComponent<Leg_up>().done)
                                {
                                    m1 = M.Forward;
                                    Legs[0].ControlledMove(m1, state);
                                }
                                break;
                            }
                        case M.Forward:
                            {
                                //if (L1 forward is done and L2 back is done) 
                                //L1 down, L2 up
                                if (Legs[1].done)
                                    GetComponent<Move>().active = 0;
                                if (Legs[0].done == true && Legs[1].done == true)
                                {
                                    m1 = M.Down;
                                    Legs[0].slave.GetComponent<Leg_up>().StateDown();
                                    Legs[1].slave.GetComponent<Leg_up>().StateUp();
                                }
                                break;
                            }
                        case M.Down:
                            {
                                //If (L1 is down and L2 is up) 
                                //L1 Back L2 Front
                                if (Legs[0].slave.GetComponent<Leg_up>().done && Legs[1].slave.GetComponent<Leg_up>().done)
                                {
                                    m1 = M.Back;
                                    Legs[0].ControlledMove(m1, state);
                                    Legs[1].ControlledMove(M.Forward, state);
                                    GetComponent<Move>().active = 1;
                                }
                                break;
                            }
                        case M.Back:
                            {
                                //if (L1 back is done and L2 forward is done) 
                                //L1 up, L2 down
                                if (Legs[0].done)
                                    GetComponent<Move>().active = 0;
                                if (Legs[0].done == true && Legs[1].done == true)
                                {
                                    m1 = M.Up;
                                    Legs[0].slave.GetComponent<Leg_up>().StateUp();
                                    Legs[1].slave.GetComponent<Leg_up>().StateDown();
                                }
                                break;
                            }
                        case M.Up:
                            {
                                //If (L1 is up and L2 is down) 
                                //L1 Front L2 back
                                if (Legs[0].slave.GetComponent<Leg_up>().done == true && Legs[1].slave.GetComponent<Leg_up>().done)
                                {
                                    m1 = M.Forward;
                                    Legs[0].ControlledMove(m1, state);
                                    Legs[1].ControlledMove(M.Back, state);
                                    GetComponent<Move>().active = 1;
                                }
                                break;
                            }
                    }
                    break;
                }
            case Mode.Back:
                {
                    switch (m1)
                    {
                        case M.Zero:
                            {

                                break;
                            }
                        case M.StartUp:
                            {
                                //L1 Up
                                Legs[0].slave.GetComponent<Leg_up>().StateUp();
                                m1 = M.StartWait;
                                break;
                            }
                        case M.StartWait:
                            {
                                //if L1 is up, go back
                                if (Legs[0].slave.GetComponent<Leg_up>().done)
                                {
                                    m1 = M.Back;
                                    Legs[0].ControlledMove(m1, state);
                                }
                                break;
                            }
                        case M.Back:
                            {
                                //if (L1 back is done and L2 forward is done) 
                                //L1 down, L2 up
                                if (Legs[0].done)
                                    GetComponent<Move>().active = 0;
                                if (Legs[0].done == true && Legs[1].done == true)
                                {
                                    m1 = M.Down;
                                    Legs[0].slave.GetComponent<Leg_up>().StateDown();
                                    Legs[1].slave.GetComponent<Leg_up>().StateUp();
                                }
                                break;
                            }
                        case M.Down:
                            {
                                //If (L1 is down and L2 is up) 
                                //L1 Front L2 Back
                                if (Legs[0].slave.GetComponent<Leg_up>().done && Legs[1].slave.GetComponent<Leg_up>().done)
                                {
                                    m1 = M.Forward;
                                    Legs[0].ControlledMove(m1, state);
                                    Legs[1].ControlledMove(M.Back, state);
                                    GetComponent<Move>().active = 2;
                                }
                                break;
                            }
                        case M.Forward:
                            {
                                //if (L1 forward is done and L2 back is done) 
                                //L1 up, L2 down
                                if (Legs[1].done)
                                    GetComponent<Move>().active = 0;
                                if (Legs[0].done == true && Legs[1].done == true)
                                {
                                    m1 = M.Up;
                                    Legs[0].slave.GetComponent<Leg_up>().StateUp();
                                    Legs[1].slave.GetComponent<Leg_up>().StateDown();
                                }
                                break;
                            }
                        
                        case M.Up:
                            {
                                //If (L1 is up and L2 is down) 
                                //L1 back L2 frorward
                                if (Legs[0].slave.GetComponent<Leg_up>().done && Legs[1].slave.GetComponent<Leg_up>().done)
                                {
                                    m1 = M.Back;
                                    Legs[0].ControlledMove(m1, state);
                                    Legs[1].ControlledMove(M.Forward, state);
                                    GetComponent<Move>().active = 2;
                                }
                                break;
                            }

                    }
                    break;
                }
            case Mode.Left:
                {
                    //L1,L3 ([0,2]) moving forward
                    //L2,L4 ([1,3]) moving back
                    switch (m1)
                    {
                        case M.Zero:
                            {

                                break;
                            }
                        case M.StartUp:
                            {
                                //L1, L4 Up
                                Legs[0].slave.GetComponent<Leg_up>().StateUp();
                                Legs[3].slave.GetComponent<Leg_up>().StateUp();
                                m1 = M.StartWait;
                                break;
                            }
                        case M.StartWait:
                            {
                                //if L1 is up, go forward, L4 go back
                                if (Legs[0].slave.GetComponent<Leg_up>().done && 
                                    Legs[3].slave.GetComponent<Leg_up>().done)
                                {
                                    m1 = M.Forward;
                                    Legs[0].ControlledMove(m1, Mode.Forward);
                                    Legs[3].ControlledMove(M.Back, Mode.Back);
                                }
                                break;
                            }
                        case M.Forward:
                            {
                                //if (L1, L3 forward is done and L2 and L4 back is done ) 
                                //L1, L4 down, L2,L3 up
                                if (Legs[1].done)
                                    GetComponent<Move>().active = 0;
                                if (Legs[0].done == true && 
                                    Legs[1].done == true &&
                                    Legs[2].done == true &&
                                    Legs[3].done == true)
                                {
                                    m1 = M.Down;
                                    Legs[0].slave.GetComponent<Leg_up>().StateDown();
                                    Legs[3].slave.GetComponent<Leg_up>().StateDown();
                                    Legs[1].slave.GetComponent<Leg_up>().StateUp();
                                    Legs[2].slave.GetComponent<Leg_up>().StateUp();
                                }
                                break;
                            }
                        case M.Down:
                            {
                                //If (L1,L4 is down and L2,L3 is up) 
                                //L1 Back L2 Front
                                if (Legs[0].slave.GetComponent<Leg_up>().done && 
                                    Legs[1].slave.GetComponent<Leg_up>().done &&
                                    Legs[2].slave.GetComponent<Leg_up>().done &&
                                    Legs[3].slave.GetComponent<Leg_up>().done)
                                {
                                    m1 = M.Back;
                                    Legs[0].ControlledMove(m1, Mode.Forward);
                                    Legs[1].ControlledMove(M.Back, Mode.Back);
                                    Legs[3].ControlledMove(M.Forward, Mode.Back);
                                    Legs[2].ControlledMove(M.Forward, Mode.Forward);
                                    GetComponent<Move>().active = 3;
                                }
                                break;
                            }
                        case M.Back:
                            {
                                //if (L1 back is done and L2 forward is done) 
                                //L1 up, L2 down
                                if (Legs[0].done)
                                    GetComponent<Move>().active = 0;
                                if (Legs[0].done == true &&
                                    Legs[1].done == true &&
                                    Legs[2].done == true &&
                                    Legs[3].done == true)
                                {
                                    m1 = M.Up;
                                    Legs[0].slave.GetComponent<Leg_up>().StateUp();
                                    Legs[1].slave.GetComponent<Leg_up>().StateDown();
                                    Legs[3].slave.GetComponent<Leg_up>().StateUp();
                                    Legs[2].slave.GetComponent<Leg_up>().StateDown();
                                }
                                break;
                            }
                        case M.Up:
                            {
                                //If (L1 is up and L2 is down) 
                                //L1 Front L2 back
                                if (Legs[0].slave.GetComponent<Leg_up>().done &&
                                    Legs[1].slave.GetComponent<Leg_up>().done &&
                                    Legs[2].slave.GetComponent<Leg_up>().done &&
                                    Legs[3].slave.GetComponent<Leg_up>().done)
                                {
                                    m1 = M.Forward;
                                    Legs[0].ControlledMove(m1, Mode.Forward);
                                    Legs[1].ControlledMove(M.Forward, Mode.Back);
                                    Legs[3].ControlledMove(M.Back, Mode.Back);
                                    Legs[2].ControlledMove(M.Back, Mode.Back);
                                    GetComponent<Move>().active = 3;
                                }
                                break;
                            }
                    }
                    break;
                }
            case Mode.Right:
                {
                    //symmetrical to Left, I just switched 
                    //Legs[0] <=> Legs[1] 
                    //Legs[2] <=> Legs[3]
                    //and set active as 4 insted of 3 (rotation of the whole body is in the other direction)
                    //L2,L4 ([0,2]) moving forward
                    //L1,L3 ([1,3]) moving back
                    switch (m1)
                    {
                        case M.Zero:
                            {

                                break;
                            }
                        case M.StartUp:
                            {
                                Legs[1].slave.GetComponent<Leg_up>().StateUp();
                                Legs[2].slave.GetComponent<Leg_up>().StateUp();
                                m1 = M.StartWait;
                                break;
                            }
                        case M.StartWait:
                            {
                                if (Legs[1].slave.GetComponent<Leg_up>().done &&
                                    Legs[2].slave.GetComponent<Leg_up>().done)
                                {
                                    m1 = M.Forward;
                                    Legs[1].ControlledMove(m1, Mode.Forward);
                                    Legs[2].ControlledMove(M.Back, Mode.Back);
                                }
                                break;
                            }
                        case M.Forward:
                            {
                                if (Legs[1].done)
                                    GetComponent<Move>().active = 0;
                                if (Legs[1].done == true &&
                                    Legs[0].done == true &&
                                    Legs[3].done == true &&
                                    Legs[2].done == true)
                                {
                                    m1 = M.Down;
                                    Legs[1].slave.GetComponent<Leg_up>().StateDown();
                                    Legs[2].slave.GetComponent<Leg_up>().StateDown();
                                    Legs[0].slave.GetComponent<Leg_up>().StateUp();
                                    Legs[3].slave.GetComponent<Leg_up>().StateUp();
                                }
                                break;
                            }
                        case M.Down:
                            {
                                if (Legs[1].slave.GetComponent<Leg_up>().done &&
                                    Legs[0].slave.GetComponent<Leg_up>().done &&
                                    Legs[3].slave.GetComponent<Leg_up>().done &&
                                    Legs[2].slave.GetComponent<Leg_up>().done)
                                {
                                    m1 = M.Back;
                                    Legs[1].ControlledMove(m1, Mode.Forward);
                                    Legs[0].ControlledMove(M.Back, Mode.Back);
                                    Legs[2].ControlledMove(M.Forward, Mode.Back);
                                    Legs[3].ControlledMove(M.Forward, Mode.Forward);
                                    GetComponent<Move>().active = 4;
                                }
                                break;
                            }
                        case M.Back:
                            {
                                if (Legs[1].done)
                                    GetComponent<Move>().active = 0;
                                if (Legs[1].done == true &&
                                    Legs[0].done == true &&
                                    Legs[3].done == true &&
                                    Legs[2].done == true)
                                {
                                    m1 = M.Up;
                                    Legs[1].slave.GetComponent<Leg_up>().StateUp();
                                    Legs[0].slave.GetComponent<Leg_up>().StateDown();
                                    Legs[2].slave.GetComponent<Leg_up>().StateUp();
                                    Legs[3].slave.GetComponent<Leg_up>().StateDown();
                                }
                                break;
                            }
                        case M.Up:
                            {
                                if (Legs[1].slave.GetComponent<Leg_up>().done &&
                                    Legs[0].slave.GetComponent<Leg_up>().done &&
                                    Legs[3].slave.GetComponent<Leg_up>().done &&
                                    Legs[2].slave.GetComponent<Leg_up>().done)
                                {
                                    m1 = M.Forward;
                                    Legs[1].ControlledMove(m1, Mode.Forward);
                                    Legs[0].ControlledMove(M.Forward, Mode.Back);
                                    Legs[2].ControlledMove(M.Back, Mode.Back);
                                    Legs[3].ControlledMove(M.Back, Mode.Back);
                                    GetComponent<Move>().active = 4;
                                }
                                break;
                            }
                    }
                    break;
                }
        }
        
    }
    public void ResetPos()
    {
        //switchState(false);
        transform.localPosition = (Vector3.zero);
        reset = false;
    }

    public void switchState(bool state)
    {
        active = state;

    }

    public void switchSt()
    {
        if (active)
        {
            active = false;
        }
        else
        {
            active = true;
        }
    }
    public void changeVelocity(float vel)
    {
        transVelocity = vel;
    }
    //public void changeVelocityNegative(float vel)
    //{
    //    GetComponents<TranslateContinuously>()[1].transVelocity = -vel;
    //}
}
