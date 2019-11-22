using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ToneMapping : MonoBehaviour {

	public Shader shadertoneMapping;

	[Range(1,4)]
	public float contrast;
	[Range(1,4)]
	public float compression;
	[Range(1,4)]
	public float range;

	Material mat;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnRenderImage(RenderTexture src,RenderTexture dest)
	{
		float r = 1.0f / compression;
		float amid = Mathf.Pow(0.5f, contrast - 0.5f*contrast + 0.5f);
		float rfix = -amid * r / (-1.0f + amid - r + 2.0f*amid*r);

		if (mat == null)
			mat = new Material(shadertoneMapping);

		mat.SetFloat("contrast", contrast);
		mat.SetFloat("rfix", rfix);
		mat.SetFloat("range", range);

		Graphics.Blit(src, dest, mat);
	}
}
