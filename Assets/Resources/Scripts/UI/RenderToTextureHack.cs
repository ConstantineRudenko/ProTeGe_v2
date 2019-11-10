using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class RenderToTextureHack : MonoBehaviour {

	public RawImage rawImage;
	public bool createNewRenderTexture = false;

	private Camera cam;

	// Use this for initialization
	void Start () {
		cam = this.GetComponent<Camera> ();

		RenderTexture rt;
		if (createNewRenderTexture)
			rt = RenderTexture.GetTemporary (
				Screen.width, Screen.height, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Default, 1
			);
		else
			rt = rawImage.texture as RenderTexture;

		if (rt == null)
			return;

		cam.targetTexture = rt;
		rawImage.texture = rt;
	}
	
	// Update is called once per frame
	void Update () {
		if (rawImage.texture is RenderTexture)
			cam.targetTexture = (RenderTexture)rawImage.texture;
		else
			return;

		if(createNewRenderTexture){
			bool needNewRt = false;
			if (rawImage.texture == null)
				needNewRt = true;
			else if (Screen.width != rawImage.texture.width || Screen.height != rawImage.texture.height) {
				needNewRt = true;
				cam.targetTexture = null;
				RenderTexture.ReleaseTemporary (cam.targetTexture);
			}
			if (needNewRt) {
				RenderTexture rt = RenderTexture.GetTemporary (Screen.width, Screen.height);
				cam.targetTexture = rt;
				rawImage.texture = rt;
			}
			
		}
			
	}
}
