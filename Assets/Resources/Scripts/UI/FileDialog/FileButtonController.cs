using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileButtonController : MonoBehaviour {

	public FileDialogController FileDialogController;
	public string filePath;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SelectFile(){
		FileDialogController.SelectFile (filePath);
	}
}
