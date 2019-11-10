using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class CreateFolderHelper : MonoBehaviour {

	public FileDialogController saveFileDialogController;
	public InputField inputFieldFolderName;
	public GameObject errorPanel;
	public Text errorText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnDisable(){
		inputFieldFolderName.text = "";
	}

	public void CreateFolder(){
		if (Directory.Exists (saveFileDialogController.curPath) == false) {
			errorText.text = "selected directory does not exist\n" + saveFileDialogController.curPath;
			errorPanel.SetActive (true);
			saveFileDialogController.Reset ();

			gameObject.SetActive (false);
		}

		if (inputFieldFolderName.text == "") {
			errorText.text = "folder name empty";
			errorPanel.SetActive (true);
		}

		string path = saveFileDialogController.curPath;
		path += @"\" + inputFieldFolderName.text;
		if (Directory.Exists (path)) {
			errorText.text = "folder already exists\n" + path;
			errorPanel.SetActive (true);

			gameObject.SetActive (false);
		}

		try{
			Directory.CreateDirectory(path);
		}
		catch{
			errorText.text = "could not create directory\n" + path;
			errorPanel.SetActive (true);
		}

		saveFileDialogController.Reset ();
		gameObject.SetActive (false);
	}
}
