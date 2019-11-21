using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PreviewMaximizer : MonoBehaviour {
	public float ratio = 1;

	Rect nonMaximizedRect;
	Rect maximizedRect;
	bool maximized = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float screenRatio = ((float)(Screen.width)) / Screen.height;

		if (Input.GetButtonDown("Maximize preview") && Application.isPlaying)
		{
			maximized = !maximized;

			if (!maximized)
				Globals.instance.components.previewCamera.rect = nonMaximizedRect;
		}

		if (maximized)
		{
			maximizedRect.yMax = 0.95f;
			maximizedRect.yMin = 0.05f;

			maximizedRect.xMin = (float)((Screen.width - Screen.height) * 0.5 / Screen.width);
			maximizedRect.xMax = maximizedRect.xMin + maximizedRect.height /  screenRatio * ratio;

			Globals.instance.components.previewCamera.rect = maximizedRect;
		}
		else
		{
			nonMaximizedRect = Globals.instance.components.previewCamera.rect;
			nonMaximizedRect.width = nonMaximizedRect.height /  screenRatio * ratio;
			Globals.instance.components.previewCamera.rect = nonMaximizedRect;
		}
	}
}
