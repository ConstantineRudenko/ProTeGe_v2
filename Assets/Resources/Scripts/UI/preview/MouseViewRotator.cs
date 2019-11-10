using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseViewRotator : MonoBehaviour {

	private float x_rotation;
	private float y_rotation;

	public float scaleSensivity = 0.1f;
	public float rotationSensivity = 2;

	// Use this for initialization
	void Start () {
	}

	private bool mouseOver {
		get { 
			Vector3 pos = Globals.instance.components.previewCamera.ScreenToViewportPoint (Input.mousePosition);
			if (pos.x > 1)
				return false;
			if (pos.x < 0)
				return false;
			if (pos.y > 1)
				return false;
			if (pos.y < 0)
				return false;
			return true;
		}
	}

	private bool dragging = false;

	// Update is called once per frame
	void Update () {
		if (mouseOver) {
			Vector3 scale = transform.localScale;

			float scaleInput = Input.GetAxis ("Mouse ScrollWheel");

			if (scaleInput > 0)
				scale.x -= scaleSensivity;
			if (scaleInput < 0)
				scale.x += scaleSensivity;

			if (scale.x < 0)
				scale.x = 0;

			scale.y = scale.z = scale.x;

			transform.localScale = scale;
		}

		if (mouseOver && Input.GetMouseButton (0))
			dragging = true;

		if (Input.GetMouseButton (0) == false)
			dragging = false;

		if (dragging) {
			x_rotation += Input.GetAxis ("Mouse X") * rotationSensivity;
			y_rotation += Input.GetAxis ("Mouse Y") * rotationSensivity;
			transform.rotation = Quaternion.identity;
			transform.Rotate (0, x_rotation, 0);
			transform.Rotate (-y_rotation, 0, 0);
		}
	}
}
