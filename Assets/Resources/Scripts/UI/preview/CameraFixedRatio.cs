using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class CameraFixedRatio : MonoBehaviour {

	public float ratio = 1;

	private Camera cam;

	// Use this for initialization
	void Start () {
		cam = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
		float screenRatio = ((float)(Screen.width)) / Screen.height;
		Rect rect = cam.rect;
		rect.width = rect.height /  screenRatio * ratio;
		cam.rect = rect;
	}
}
