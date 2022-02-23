using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseOrbitImproved : MonoBehaviour {


    public  Transform target ;
public   Transform pivotR;
public  Transform[] view ;
public   Slider zoomSlider;
public float distance = 10.0f;
   public bool joystick  ;
public float xSpeed = 40;
    public float ySpeed = 250.0f;
    public float panSpeed = 0.001f;
    public float yMinLimit = -20f;
    public float yMaxLimit = 90f;

    public float distanceMin = 0.3f;
    public float distanceMax = 35f;
    public float zoomRange = 0f;
    public bool zoomSliderUsed = false;
    public bool zoomSliderInactive = false;


    public float x = 0.0f;
    public float y = 0.0f;
    public float x1 = 0.0f;
    public float y1 = 0.0f;
    public string resetkey = "Cancel";
    public string naviswitchkey = "Submit";
    public int stdViewIndex;

 public Vector2 touchDeltaPosition;

public bool rotating = false;
    public bool panning = false;
    public bool zooming = false;

    public float zoomspeed = -1f;
    // public mouseclick = Input.GetMouseButtonDown(0);

    public float panx;
    public float pany;
    public Quaternion rotation = new Quaternion(0,0,0,0);
    public Vector3 position = new Vector3(0.0f, 0.0f, 0.0f);
    public Vector3 pivot =new Vector3(0, 0, 0);
    public Vector3 pivotpan = new Vector3(0, 0, 0);

    public bool keepEyeOnTarget = false;

    // Use this for initialization
    void Start () {
        var angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
        pivot = target.position;
        pivotR.position = pivot;
        ResetPivot();
        // Make the rigid body not change rotation
        if (GetComponent< Rigidbody > ())
            GetComponent< Rigidbody > ().freezeRotation = true;
    }

    private void ResetPivot()
    {
        pivot = target.position;
        pivotpan = target.position;
        pivotR.position = pivotpan;
        position = rotation * new Vector3(0.0f, 0.0f, -distance) + pivot;
        transform.rotation = rotation;
        transform.position = position;
    }

    // Update is called once per frame
    void Update () {
        if (keepEyeOnTarget)
        {
            pivot = target.position;
            position = rotation * new Vector3(0.0f, 0.0f, -distance) + pivot;
            transform.position = position;
        }

        if (Input.touchCount > 0 &&
              Input.GetTouch(0).phase == TouchPhase.Moved)
        {

            // Get movement of the finger since last frame
            touchDeltaPosition = Input.GetTouch(0).deltaPosition;



        }




        zoomRange = distanceMax + distanceMin;

        if (Input.GetButtonDown(resetkey))
        {

            ResetPivot();

        }


        if (Input.GetKey("x"))
        {


            if (Input.GetKeyDown("1"))
            {
                PredefinedViewSave(0);
            }

            if (Input.GetKeyDown("2"))
            {
                PredefinedViewSave(1);
            }

            if (Input.GetKeyDown("3"))
            {
                PredefinedViewSave(2);
            }

            if (Input.GetKeyDown("4"))
            {
                PredefinedViewSave(3);
            }

            if (Input.GetKeyDown("5"))
            {
                PredefinedViewSave(4);
            }

            if (Input.GetKeyDown("6"))
            {
                PredefinedViewSave(5);
            }

        }


        if (Input.GetKeyDown("1"))
        {

            StartCoroutine(PredefinedViewSmooth(0, 1));
            //Debug.Log("view launched!");

        }

        if (Input.GetKeyDown("2"))
        {

            StartCoroutine(PredefinedViewSmooth(1, 1));

        }

        if (Input.GetKeyDown("3"))
        {

            StartCoroutine(PredefinedViewSmooth(2, 1));

        }

        if (Input.GetKeyDown("4"))
        {

            StartCoroutine(PredefinedViewSmooth(3, 1));

        }

        if (Input.GetKeyDown("5"))
        {

            StartCoroutine(PredefinedViewSmooth(4, 1));

        }

        if (Input.GetKeyDown("6"))
        {

            StartCoroutine(PredefinedViewSmooth(5, 1));

        }



        if (Input.GetButtonDown(naviswitchkey))
        {

            if (joystick)
            {
                joystick = false;
            }
            else
            {
                joystick = true;
            }
        }
    }
    private void LateUpdate()
    {
        if (target)
        {
            String axisx; 
             String axisy ;
            if (joystick)
            {
                axisx = "Horizontal";
                axisy = "Vertical";
            }
            else
            {
                axisx = "Mouse X";
                axisy = "Mouse Y";
            }


            if (Input.GetButton("Fire2") || zooming)
            {
                distance = Mathf.Clamp(distance - Input.GetAxis(axisy) * zoomspeed, distanceMin, distanceMax);
                RaycastHit hit  ;
                //if (Physics.Linecast (target.position, transform.position, hit)) {
                //if (Physics.Linecast (pivot, transform.position, hit)) {
                //		distance -=  hit.distance;
                //}

                if (zoomSliderUsed)
                {

                    zoomSliderInactive = true;
                    zoomSlider.value = -distance + zoomRange;
                    zoomSliderInactive = false;
                }


                rotation = Quaternion.Euler(y, x, 0);
                position = rotation * new Vector3(0.0f, 0.0f, -distance) + pivot;

                transform.position = position;
            }


            if (((Input.GetButton("Fire1") || rotating) && (!panning && !zooming)) || (joystick && !Input.GetButton("Fire2") && !Input.GetButton("Fire3")) || Input.touchCount > 0)
            //if ((rotating && (!panning && !zooming)) || (joystick && !Input.GetButton("Fire2") && !Input.GetButton("Fire3")) || Input.touchCount > 0)
            {

                if (Input.touchCount > 0)
                {
                    x += (float) (Input.touches[0].deltaPosition.x * xSpeed * distance * 0.02);
                    y -= (float)(Input.touches[0].deltaPosition.y * ySpeed * 0.02);
                    Debug.Log("touched");
                }


                x += (float)(Input.GetAxis(axisx) * xSpeed * distance * 0.02);
                y -= (float) (Input.GetAxis(axisy) * ySpeed * 0.02);



                y = ClampAngle(y, yMinLimit, yMaxLimit);
                rotation = Quaternion.Euler(y, x, 0);

                position = rotation * new Vector3(0.0f, 0.0f, -distance) + pivot;

                transform.rotation = rotation;
                transform.position = position;

                //position = rotation * Vector3(0.0, 0.0, -distance) + target.position;
            }
            //Input.GetMouseButton(2)
            if (Input.GetButton("Fire3") || panning)
            {

                if (Input.touchCount > 0)
                {
                    x1 = Input.touches[0].deltaPosition.x * xSpeed * panSpeed;
                    y1 = Input.touches[0].deltaPosition.y * xSpeed * panSpeed;
                }

                x1 = Input.GetAxis(axisx) * xSpeed * panSpeed;
                y1 = Input.GetAxis(axisy) * xSpeed * panSpeed;



                //if (x1 != panx || y1 != pany) {

                panx = x1;
                pany = y1;

                //y1 = ClampAngle(y, yMinLimit, yMaxLimit);
                //rotation = Quaternion.Euler(y1, x1, 0);
                //rotation = transform.rotation;
                //position = position + rotation * Vector3(-x,y,0);
                pivotpan = rotation * new Vector3(-x1, -y1, 0) + pivot;
                pivot = pivotpan;
                pivotR.position = pivotpan;
                //pivotpan = rotation * Vector3(-x, y, -distance);
                position = rotation * new Vector3(0.0f, 0.0f, -distance) + pivot;
                //rotation = Quaternion.Euler(y, x, 0);
                // position = rotation * Vector3(0.0, 0.0, -distance) + pivot;
                transform.position = position;
                //transform.rotation = rotation;
                //     }
            }
        }

    }
    private void PredefinedView(int nr, float movetime)
    {

        var flag = 0;
        if (joystick)
        {
            joystick = false;
            flag = 1;
        }

        x = view[nr].localScale.x;
        if (view[nr].localScale.y >= 180)
        {
            y = 180 - view[nr].localEulerAngles.y;
        }
        else
        {
            y = view[nr].localScale.y;
        }

        y = ClampAngle(y, yMinLimit, yMaxLimit);
        distance = view[nr].localScale.z;

        pivot = target.position + view[nr].localPosition;
        pivotpan = pivot;
        pivotR.position = pivotpan;

        rotation = Quaternion.Euler(y, x, 0);
        position = rotation * new Vector3(0.0f, 0.0f, -distance) + pivot;

        if (zoomSliderUsed)
        {

            zoomSliderInactive = true;
            zoomSlider.value = -distance + zoomRange;
            zoomSliderInactive = false;
        }

        //transform.rotation = rotation;
        //transform.position = position;

        var baserot = transform.rotation;
        var basepos = transform.position;

        var i = 0.0f;
        var rate = 1.0 / movetime;
        while (i < 1.0f)
        {

            i += (float) (Time.deltaTime * rate);

            transform.rotation = Quaternion.Lerp(baserot, rotation, i);
            transform.position = Vector3.Lerp(basepos, position, i);

            return;
        }

        if (flag == 1)
        {
            joystick = true;
        }

    }


    IEnumerator PredefinedViewSmooth(int nr, float movetime)
    {

        var flag = 0;
        if (joystick)
        {
            joystick = false;
            flag = 1;
        }

        x = view[nr].localScale.x;
        if (view[nr].localScale.y >= 180)
        {
            y = 180 - view[nr].localEulerAngles.y;
        }
        else
        {
            y = view[nr].localScale.y;
        }

        y = ClampAngle(y, yMinLimit, yMaxLimit);
        distance = view[nr].localScale.z;

        pivot = target.position + view[nr].localPosition;
        pivotpan = pivot;
        pivotR.position = pivotpan;

        rotation = Quaternion.Euler(y, x, 0);
        position = rotation * new Vector3(0.0f, 0.0f, -distance) + pivot;

        if (zoomSliderUsed)
        {

            zoomSliderInactive = true;
            zoomSlider.value = -distance + zoomRange;
            zoomSliderInactive = false;
        }

        //transform.rotation = rotation;
        //transform.position = position;

        var baserot = transform.rotation;
        var basepos = transform.position;

        var i = 0.0f;
        var rate = 1.0 / movetime;
        while (i < 1.0f)
        {

            i += (float)(Time.deltaTime * rate);

            transform.rotation = Quaternion.Lerp(baserot, rotation, i);
            transform.position = Vector3.Lerp(basepos, position, i);

            yield return new WaitForSeconds(0);
        }

        if (flag == 1)
        {
            joystick = true;
        }
        
    }


    private void PredefinedViewSave(int nr)
    {
        Debug.Log("view no: " + nr + " save done!");
        //view[nr].localEulerAngles = new Vector3(x,y,view[nr].localEulerAngles.z);
        //view[nr].localScale.z = distance;
        view[nr].localScale = new Vector3(x, y, distance);
        view[nr].localPosition = pivot - target.position;
    }

public float ClampAngle(float angle, float min, float max)
{
    if (angle < -360)
        angle += 360;
    if (angle > 360)
        angle -= 360;
    return Mathf.Clamp(angle, min, max);
}

  public void  joystickSwitch(bool val)
    {

        joystick = val;

    }
    public void ZoomDistance(float val)
    {

        if (zoomSliderUsed && !zoomSliderInactive)
        {

            distance = -val + zoomRange;
            rotation = Quaternion.Euler(y, x, 0);

            position = rotation * new Vector3(0.0f, 0.0f, -distance) + pivot;

            transform.position = position;

        }

    }
    public void RotateMode(  int val)
    {

        if (val == 1)
        {
            rotating = true;
        }
        else
        {
            rotating = false;
        }


    }

    public void PanMode(int val)
    {

        if (val == 1)
        {
            panning = true;
        }
        else
        {
            panning = false;
        }

    }

    public void ZoomMode(int val)
    {

        if (val == 1)
        {
            zooming = true;
        }
        else
        {
            zooming = false;
        }

    }
    public void PredViewStd(int nr)
    {

        PredefinedView(nr, 1);

    }

    public void StdView(int tim)
    {

        PredefinedView(stdViewIndex, tim);

    }


    public void PredViewSmooth(int nr)
    {
        StartCoroutine(PredefinedViewSmooth(nr, 1));
    }

    public void switchTarget(Transform newTarget)
    {
        target = newTarget;
        ResetPivot();
        
        
    }

    public void switchKeepEye()
    {
        keepEyeOnTarget = !keepEyeOnTarget;
    }

    public void switchKeepEye(bool state)
    {
        keepEyeOnTarget = state;
    }

}
