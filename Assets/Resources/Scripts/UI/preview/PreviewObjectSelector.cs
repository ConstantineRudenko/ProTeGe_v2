using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewObjectSelector : MonoBehaviour {

	public GameObject cube;
	public GameObject cylinder;
	public GameObject sphere;

	public UnityEngine.UI.Text buttonText;

	private GameObject[] objects;

	public void Next (){
		Material m = Globals.instance.components.previewRenderer.material;

		for (int i = 0; i < objects.Length; i++)
			objects [i].SetActive (false);

		curIndex++;
		if (curIndex > objects.Length - 1)
			curIndex = 0;

		GameObject target = objects [curIndex];

		target.SetActive (true);
		MeshRenderer mr = target.GetComponent<MeshRenderer> ();
		Globals.instance.components.previewRenderer = mr;
		mr.material = m;

		if (target == cube)
			SetTextureScale (mr.material, new Vector2 (1, 1));
		else if (target == cylinder)
			SetTextureScale (mr.material, new Vector2 (2, 2));
		else if (target == sphere)
			SetTextureScale (mr.material, new Vector2 (4, 4));

		buttonText.text = target.name;
	}

	private int curIndex;

	// Use this for initialization
	void Start () {
		objects = new GameObject[3] {cube, cylinder, sphere};
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void SetTextureScale(Material m, Vector2 scale){
		m.mainTextureScale = scale;
		m.SetTextureScale ("_BumpTex", scale);
		m.SetTextureScale ("_SmoothnessTex", scale);
		m.SetTextureScale ("_MetallicTex", scale);
		m.SetTextureScale ("_OcclusionTex", scale);
		m.SetTextureScale ("_ParallaxTex", scale);
		m.SetTextureScale ("_EmissionTex", scale);
	}
}
