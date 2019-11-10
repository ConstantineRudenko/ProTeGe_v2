using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPan : MonoBehaviour {

	public float sensivity = 0.2f;

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

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (mouseOver && Input.GetMouseButton (2))
			dragging = true;

		if (Input.GetMouseButton (2) == false)
			dragging = false;

		if (dragging) {
			float x = Input.GetAxis ("Mouse X") * sensivity;
			float y = Input.GetAxis ("Mouse Y") * sensivity;
			transform.Translate (-x, -y, 0, Space.Self);
		}
	}
}
