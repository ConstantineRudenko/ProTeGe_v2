using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiskShortcutButtonController : MonoBehaviour {

	public string path;
	public FileDialogController saveFileDialogCtrl;
	public Text buttonText;

	public void SetPath(){
		saveFileDialogCtrl.curPath = path;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
