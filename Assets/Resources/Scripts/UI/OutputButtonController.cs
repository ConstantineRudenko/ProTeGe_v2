using UnityEngine;
using System.Collections;

public class OutputButtonController : MonoBehaviour {

	public void OnPressed(){
		Globals.instance.components.buttonPressedOutput = this;
	}

	public NodeController nodeController { get; private set; }

	void Start(){
		nodeController = transform.parent.parent.GetComponent<NodeController> ();
	}
}
