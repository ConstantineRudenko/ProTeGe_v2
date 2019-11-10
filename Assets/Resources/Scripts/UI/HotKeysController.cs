using UnityEngine;
using System.Collections;

public class HotKeysController : MonoBehaviour {

	void Update () {
		if (Input.GetKeyDown (KeyCode.Delete))
			Globals.instance.components.DeleteNode ( Globals.instance.components.nodeGuiCtrl.selectedNode );
	}
}
