using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class NodeController : MonoBehaviour {

    public InputButtonController[] inputs;
    public OutputButtonController output;
    public ProTeGe.TextureProcessors.TextureProcessor processor;
	public ulong ID;
	public static SortedSet<ulong> used_ids = new SortedSet<ulong>();

    public void UpdateInputs ()
	{
		for (int i = 0; i < processor.inputsCount; i++)
			processor.inputs [i].connectedProcessor = inputs [i].connectedProcessor;
	}

	public void SelectMe ()
	{
		Globals.instance.components.nodeGuiCtrl.selectedNode = GetComponent<NodeController> ();
		Globals.instance.components.SelectedNodeNameDisplay.text = processor.name.ToLower ();
		Globals.instance.components.buttonPressedOutput = output;
	}

	public void AcquireID(){
		for (ID = 1; ID < ulong.MaxValue; ID++) {
			if (used_ids.Contains (ID) == false) {
				used_ids.Add (ID);
				return;
			}
		}
		throw new System.IndexOutOfRangeException ("all possible IDs used");
	}

	protected void Start () {
		this.inputs = new InputButtonController[3];
		Transform inputs = transform.Find ("inputs");
		this.inputs[0] = inputs.Find("input 1").gameObject.GetComponent<InputButtonController>();
		this.inputs[1] = inputs.Find("input 2").gameObject.GetComponent<InputButtonController>();
		this.inputs[2] = inputs.Find("input 3").gameObject.GetComponent<InputButtonController>();
		this.inputs[0].gameObject.SetActive(processor.inputsCount > 0);
		this.inputs[1].gameObject.SetActive(processor.inputsCount > 1);
		this.inputs[2].gameObject.SetActive(processor.inputsCount > 2);
		if (processor.inputsCount == 0)
			inputs.gameObject.SetActive (false);
		UpdateInputs();
		transform.Find ("node name").GetComponent<Text> ().text = processor.name.ToLower ();

		for(int i = 0; i < 3; i++)
			this.inputs[i].gameObject.SetActive(processor.inputsCount > i);
	}

	protected void OnDestroy(){
		if(processor != null)
			processor.Kill ();
		Globals.instance.nodes.Remove (this);
		used_ids.Remove (ID);
	}

}
