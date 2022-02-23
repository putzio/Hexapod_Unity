using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Text;
using UnityEngine.UI;

public class ColorLerp : MonoBehaviour {

	public Material[] changedMats;
	public Color desiredColor;
	float startTime;
	float journeyTime;
	public float speed = 1f;
	public int state = -1;
	public string key = "c";
	Color baseColor;
	float baseAlpha;
    public float r=1, g=1, b=1;
    public Button button;

	// Use this for initialization
	void Start () {
		startTime = Time.time;
		journeyTime = 1f;
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (key)) {
			stateSw ();
		}



		if (state == 1) {
			float distCovered = (Time.time - startTime) * speed;
			float fracJourney = distCovered / journeyTime;
			foreach (Material m in changedMats) {
				m.color = Color.Lerp (baseColor, desiredColor, fracJourney);

			}
			if ((changedMats[0].color.r == desiredColor.r && changedMats[0].color.g == desiredColor.g && changedMats[0].color.b == desiredColor.b) || fracJourney == 1) {
				state = 0;
			}            
        }
        button.image.color = new Color(r, g, b);
        desiredColor = new Color(r, g, b);
    }

    public void updateRed(float red) { r = red; }
    public void updateGreen(float green) { g = green; }
    public void updateBlue(float blue) { b = blue; }

    public void stateSw(){
		state = 1;
		startTime = Time.time;
		baseColor = changedMats [0].color;
		baseAlpha = baseColor.a;
		desiredColor = new Color (desiredColor.r, desiredColor.g, desiredColor.b, baseAlpha);
	}



}
