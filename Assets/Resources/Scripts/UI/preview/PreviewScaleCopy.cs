using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PreviewScaleCopy : MonoBehaviour {

	public float test;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		RectTransform rt = (RectTransform)transform;
		Vector2 szD;
		if (Globals.instance.components.previewCamera != null) {
			szD.x = Globals.instance.components.previewCamera.rect.width * Screen.width;
			szD.y = Globals.instance.components.previewCamera.rect.height * Screen.height;
			rt.sizeDelta = szD;
		}
	}
}
