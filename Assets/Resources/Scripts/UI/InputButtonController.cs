using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputButtonController : MonoBehaviour {

	public ProTeGe.TextureProcessors.TextureProcessor connectedProcessor {
		get { 
			if (connectedOutputButton == null || connectedOutputButton.transform.parent.gameObject == null)
				_connectedProcessor = null;
			return _connectedProcessor;
		}
	}

	public OutputButtonController connectedOutputButton {
		get{ return _connectedTo; }
		set {
			_connectedTo = value;
			if (connectedOutputButton != null)
				RemoveCycles ();
			if (connectedOutputButton != null)
				_connectedProcessor = connectedOutputButton.transform.parent.parent.gameObject.GetComponent<NodeController> ().processor;
			else
				_connectedProcessor = null;
			NodeController ctrl = transform.parent.parent.gameObject.GetComponent<NodeController> ();
			if (ctrl != null)
				ctrl.UpdateInputs ();
			Globals.instance.components.outputNode.UpdatePreview ();
		}
	}

	public void ConnectWithOutput (OutputButtonController output)
	{
		Globals.instance.components.buttonPressedOutput = null;
		connectedOutputButton = output;
	}

	public void OnPressed ()
	{
		OutputButtonController temp = connectedOutputButton;
		connectedOutputButton = null;
		if (Globals.instance.components.buttonPressedOutput != null)
			if(Globals.instance.components.buttonPressedOutput.transform.parent.parent != transform.parent.parent)
				if(Globals.instance.components.buttonPressedOutput != temp)
					ConnectWithOutput (Globals.instance.components.buttonPressedOutput);
	}

	protected void Awake () {
		connectionLine = GetComponent<LineRenderer>();
	}

	protected void Update () {
		if (connectedOutputButton != null) {
			const int divisions = 20;
			connectionLine.enabled = true;
			connectionLine.positionCount = divisions;
			Vector3[] positions = new Vector3[divisions];
			for (int i = 0; i < divisions; i++) {
				positions [i].x = Mathf.Lerp (transform.position.x,
					connectedOutputButton.transform.position.x, (float)i / (divisions - 1));
				positions [i].y = Mathf.Lerp (transform.position.y,
					connectedOutputButton.transform.position.y, SmoothStep ((float)i / (divisions - 1)));
				positions [i].z = transform.position.z;
			}
			for (int i = 0; i < positions.Length; i++)
				positions [i] = Globals.instance.ScreenToWorld (positions [i]);
			connectionLine.SetPositions (positions);
		} else {
			connectionLine.SetPositions (new Vector3[] { Vector3.zero, Vector3.zero });
			connectionLine.enabled = false;
		}
		float w = 100f / Screen.height;
		connectionLine.startWidth = w;
		connectionLine.endWidth = w;
	}

	private float SmoothStep(float x){
		float x2 = x * x;
		float x4 = x2 * x2;
		return x4 * x * 6 - x4 * 15 + x2 * x * 10;
	}

	private void RemoveCycles ()
	{
		List<Transform> nodes = new List<Transform> ();
		List<Transform> nextNodes = new List<Transform> ();

		InputButtonController[] inputs = new InputButtonController[3];
		NodeController node = transform.parent.parent.GetComponent<NodeController> ();
		if (node == null)
			return;
		for (int i = 0; i < 3; i++)
			inputs [i] = node.inputs [i];

		foreach (InputButtonController x in inputs)
			if (x.connectedOutputButton != null)
			if (!nextNodes.Contains (x.connectedOutputButton.transform.parent.parent))
				nextNodes.Add (x.connectedOutputButton.transform.parent.parent);

		do {
			List<Transform> temp = nodes;
			nodes = nextNodes;
			nextNodes = temp;
			nextNodes.Clear ();

			foreach (Transform x in nodes) {
				node = x.GetComponent<NodeController> ();
				inputs = new InputButtonController[3];
				for (int i = 0; i < 3; i++)
					inputs [i] = node.inputs [i];

				foreach (InputButtonController y in inputs)
					if (y.connectedOutputButton != null)
					if (y.connectedOutputButton.transform.parent.parent == transform.parent.parent)
						y.connectedOutputButton = null;

				foreach (InputButtonController y in inputs)
					if (y.connectedOutputButton != null)
					if (!nextNodes.Contains (y.connectedOutputButton.transform.parent.parent))
						nextNodes.Add (y.connectedOutputButton.transform.parent.parent);
			}
		} while(nextNodes.Count != 0);
	}

	private ProTeGe.TextureProcessors.TextureProcessor _connectedProcessor;
	private OutputButtonController _connectedTo = null;
	private LineRenderer connectionLine;
}
