using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitRotation : MonoBehaviour {
	public Vector3 boundariesUp;
	public Vector3 boundariesDown;
	public bool limitX=false;
	public bool limitY=false;
	public bool limitZ=false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (limitX) {
			if (transform.localEulerAngles.x > boundariesUp.x) {
				transform.localEulerAngles = new Vector3 (boundariesUp.x, transform.localEulerAngles.y, transform.localEulerAngles.z);
			}

			if (transform.localEulerAngles.x < boundariesDown.x) {
				transform.localEulerAngles = new Vector3 (boundariesDown.x, transform.localEulerAngles.y, transform.localEulerAngles.z);
			}

		}

		if (limitY) {

			if (transform.localEulerAngles.y > boundariesUp.y) {
				transform.localEulerAngles = new Vector3 (transform.localEulerAngles.x, boundariesUp.y, transform.localEulerAngles.z);
			}

			if (transform.localEulerAngles.y < boundariesDown.y) {
				transform.localEulerAngles = new Vector3 (transform.localEulerAngles.x, boundariesDown.y, transform.localEulerAngles.z);
			}
		}

		if (limitZ) {
			if (transform.localEulerAngles.z > boundariesUp.z) {
				transform.localEulerAngles = new Vector3 (transform.localEulerAngles.x, transform.localEulerAngles.y, boundariesUp.z);
			}


			if (transform.localEulerAngles.z < boundariesDown.z) {
				transform.localEulerAngles = new Vector3 (transform.localEulerAngles.x, transform.localEulerAngles.y, boundariesDown.z);
			}

		}
	}
}
