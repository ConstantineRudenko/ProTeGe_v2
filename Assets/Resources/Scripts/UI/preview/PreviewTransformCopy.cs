using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PreviewTransformCopy : MonoBehaviour {

	public RectTransform target;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (target == null)
			return;
		
		Vector3 position = Globals.instance.components.previewCamera.rect.position;
		position.x *= Screen.width;
		position.y *= Screen.height;

		Vector3 scale = Globals.instance.components.previewCamera.rect.size;
		scale.x *= Screen.width;
		scale.y *= Screen.height;

		target.position = position;
		target.sizeDelta = scale;
	}
}
