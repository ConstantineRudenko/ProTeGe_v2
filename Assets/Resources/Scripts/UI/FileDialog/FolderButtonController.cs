using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class FolderButtonController : MonoBehaviour {

	public FileDialogController fileDialogCtrl;
	public string folderPath;

	public void SelectFolder(){
		fileDialogCtrl.curPath = folderPath;
	}

	public void LeaveFolder(){
		string path = fileDialogCtrl.curPath;
		DirectoryInfo dirInfo = new DirectoryInfo (path);
		if (dirInfo.Parent != null)
			fileDialogCtrl.curPath = dirInfo.Parent.FullName;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
