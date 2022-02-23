using UnityEngine;
using System.Collections;

public class RotateContinuously : MonoBehaviour {

	public string axis = "X";
	public float angVelocity = 180f;
	public bool active;
	public bool reset=false;
	public string keycode = "x";


	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (keycode)) {
			active = true;
		} 

		if (Input.GetKeyUp (keycode)) {
			active = false;
		}


		if (active) {
			switch (axis) {
			case "X":
				transform.Rotate (Vector3.up * Time.deltaTime * angVelocity);
				break;

			case "Y":
				transform.Rotate (Vector3.right * Time.deltaTime * angVelocity);
				break;

			case "Z":
				transform.Rotate (Vector3.forward* Time.deltaTime * angVelocity);
				break;
			
			default:
				break;
			}
		}

		if (reset) {
			ResetPos ();
		}
	}

	public void ResetPos(){
		//switchState(false);
		transform.localEulerAngles = (Vector3.zero);
		reset = false;
	}

	public void switchState(bool state) {
		active = state;

	}

	public void switchSt(){
		if (active) {
			active = false;
		} else {
			active = true;
		}
	}

}
