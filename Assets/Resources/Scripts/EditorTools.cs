using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR

[UnityEditor.CustomEditor(typeof(GlobalsHolder))]
public class ObjectBuilderEditor : UnityEditor.Editor{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		if(GUILayout.Button("Apply font everywhere"))
		{
			UnityEngine.UI.Text[] textComponents = Resources.FindObjectsOfTypeAll<UnityEngine.UI.Text>();
			foreach (UnityEngine.UI.Text component in textComponents)
				component.font = Globals.instance.components.font;
		}
	}
}



[UnityEditor.CustomEditor(typeof(DialogManager))]
public class DialogManagerEditor : UnityEditor.Editor{


	public override void OnInspectorGUI()
	{
		
		DrawDefaultInspector();

		DialogManager dialogManager = Globals.instance.components.dialogManager;

		foreach (DialogManager.Dialog x in System.Enum.GetValues(typeof( DialogManager.Dialog)))
			if (GUILayout.Button (x.ToString()))
				dialogManager.ShowDialog (x);

		if (GUILayout.Button ("Close [X]"))
			dialogManager.CloseDialog ();

	}
}

[UnityEditor.CustomEditor(typeof(FileDialogModeController))]
public class FileDialogModeControllerEditor : UnityEditor.Editor{


	public override void OnInspectorGUI()
	{

		DrawDefaultInspector();

		foreach (
			FileDialogModeController.FileDialogMode x
			in
			System.Enum.GetValues(typeof(FileDialogModeController.FileDialogMode))
		) {
			if (GUILayout.Button (x.ToString ())) {
				FileDialogModeController ctrl = (FileDialogModeController)target;
				ctrl.mode = x;
				ctrl.buttonText.enabled = false;
				ctrl.buttonText.enabled = true;
			}
		}
	}
}
#endif
