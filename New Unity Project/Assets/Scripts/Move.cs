using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{   
    public float transVelocity = 0.01f;
    public float angVelocity = -20f;
    public int active = 0;
    public string axis = "X";

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {      
        if (active == 1)
        {
            switch (axis)
            {
                case "X":
                    transform.Translate(Vector3.up * Time.deltaTime * transVelocity);
                    break;

                case "Y":
                    transform.Translate(Vector3.right * Time.deltaTime * transVelocity);
                    break;

                case "Z":
                    transform.Translate(Vector3.forward * Time.deltaTime * transVelocity);
                    break;

                default:
                    break;
            }
        }
        if (active == 2)
        {
            switch (axis)
            {
                case "X":
                    transform.Translate(Vector3.down * Time.deltaTime * transVelocity);
                    break;

                case "Y":
                    transform.Translate(Vector3.left * Time.deltaTime * transVelocity);
                    break;

                case "Z":
                    transform.Translate(Vector3.back * Time.deltaTime * transVelocity);
                    break;

                default:
                    break;
            }
        }
        if (active == 3)
        {
            switch (axis)
            {
                case "X":
                    transform.Rotate(Vector3.up * Time.deltaTime * angVelocity);
                    break;

                case "Y":
                    transform.Rotate(Vector3.right * Time.deltaTime * angVelocity);
                    break;

                case "Z":
                    transform.Rotate(Vector3.forward * Time.deltaTime * angVelocity);
                    break;

                default:
                    break;
            }
        }
        if (active == 4)
        {
            switch (axis)
            {
                case "X":
                    transform.Rotate(Vector3.down * Time.deltaTime * angVelocity);
                    break;

                case "Y":
                    transform.Rotate(Vector3.left * Time.deltaTime * angVelocity);
                    break;

                case "Z":
                    transform.Rotate(Vector3.back * Time.deltaTime * angVelocity);
                    break;

                default:
                    break;
            }
        }
    }
    public void Forward()
    {
        active = 1;
    }
    public void Back()
    {
        active = 2;
    }
    public void Left()
    {
        active = 3;
    }
    public void Right()
    {
        active = 4;
    }
    public void Stop()
    {
        active = 0;
    }
}
