using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FileDialogModeController : MonoBehaviour {

	public GameObject resolutionSelector;
	public GameObject inputFieldFileName;
	public GameObject button;
	public Text buttonText;

	public enum FileDialogMode { 
		Export,
		Import,
		Save,
		Load
	}

	public FileDialogMode mode {
		get{
			return _mode;
		}
		set{
			if (_mode != value) {
				_mode = value;
				GetComponent<FileDialogController>().Reset ();
			}
			switch (value) {
			case FileDialogMode.Export:
				button.SetActive (true);
				buttonText.text = "Export";
				resolutionSelector.SetActive (true);
				inputFieldFileName.SetActive (false);
				break;
			case FileDialogMode.Import:
				button.SetActive (false);
				resolutionSelector.SetActive (false);
				inputFieldFileName.SetActive (false);
				break;
			case FileDialogMode.Save:
				button.SetActive (true);
				buttonText.text = "Save";
				resolutionSelector.SetActive (false);
				inputFieldFileName.SetActive (true);
				break;
			case FileDialogMode.Load:
				button.SetActive (false);
				resolutionSelector.SetActive (false);
				inputFieldFileName.SetActive (false);
				buttonText.text = "Open";
				break;
			default:
				throw new System.ArgumentOutOfRangeException ();
			}
		}
	}

	private FileDialogMode _mode;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
