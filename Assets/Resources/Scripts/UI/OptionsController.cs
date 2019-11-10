using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsController : MonoBehaviour {

	public GameObject warning8kRAM;
	public UnityEngine.UI.Dropdown resolutionSelector;
	public UnityEngine.UI.Toggle realtimeUpdatePreviewToogle;
	public GameObject workingFolderWarning;

	private int selectedResolutionIndex;

	public void ApplyOptions(){
		Globals.instance.resolutionIndex_preview = selectedResolutionIndex;
		Globals.instance.realtimeUpdatePreview = realtimeUpdatePreviewToogle.isOn;
	}
		
	void Update () {
		selectedResolutionIndex = resolutionSelector.value;
		if (selectedResolutionIndex > 3 && (realtimeUpdatePreviewToogle.isOn == true))
			warning8kRAM.SetActive (true);
		else
			warning8kRAM.SetActive (false);
	}

	void OnEnable(){
		resolutionSelector.value = Globals.instance.resolutionIndex_preview;
		realtimeUpdatePreviewToogle.isOn = Globals.instance.realtimeUpdatePreview;
	}
}
