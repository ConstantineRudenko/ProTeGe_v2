using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class DialogManager : MonoBehaviour {

	public Transform display;
	public Transform headerText;
	public Transform contentPanel;

	public Transform dialogFile;
	public Transform dialogOptions;
	public Transform dialogHelp;
	public Transform dialogAbout;

	public FileDialogModeController fileDialogModeController;

	// Use this for initialization
	void OnEnable () {
		//CloseDialog ();
		// this causes a bug x_X
		// though we don't need it
	}

	void Start(){
		
	}

	public enum Dialog {
		Export,
		Import,
		Save,
		Load,
		Options,
		Help,
		About
	}

	public void ShowDialog(Dialog dialog){
		switch (dialog) {
		case Dialog.Export:
			ShowDialog (dialogFile, "Export textures");
			fileDialogModeController.mode = FileDialogModeController.FileDialogMode.Export;
			break;
		case Dialog.Import:
			ShowDialog (dialogFile, "Import texture");
			fileDialogModeController.mode = FileDialogModeController.FileDialogMode.Import;
			break;
		case Dialog.Save:
			ShowDialog (dialogFile, "Save project");
			fileDialogModeController.mode = FileDialogModeController.FileDialogMode.Save;
			break;
		case Dialog.Load:
			ShowDialog (dialogFile, "Open project");
			fileDialogModeController.mode = FileDialogModeController.FileDialogMode.Load;
			break;
		case Dialog.Options:
			ShowDialog (dialogOptions, "Options");
			break;
		case Dialog.Help:
			ShowDialog (dialogHelp, "Help");
			break;
		case Dialog.About:
			ShowDialog (dialogAbout, "About");
			break;
		default:
			throw new System.ArgumentOutOfRangeException ();
		}
	}

	private void ShowDialog(Transform dialog, string name){
		CloseDialog ();

		Globals.instance.clickBlockerActive = true;
		headerText.GetComponent<Text>().text = name.Substring (0, 1).ToUpper () + name.Substring (1).ToLower ();

		dialog.gameObject.SetActive (true);

		display.gameObject.SetActive (true);
	}

	public void CloseDialog(){
		
		Globals.instance.clickBlockerActive = false;

		foreach (Transform t in contentPanel)
			t.gameObject.SetActive (false);

		display.gameObject.SetActive (false);
	}

	// Update is called once per frame
	void Update () {
		
	}

}
