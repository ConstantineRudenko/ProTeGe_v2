using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public static class NodeSpawner{
	
	public static NodeController Spawn(ProTeGe.TextureProcessors.TextureProcessor processor){
		GameObject prefab = Globals.instance.components.nodePrefab;
		NodeController node = ((GameObject)GameObject.Instantiate(prefab)).GetComponent<NodeController>();
		node.transform.SetParent(Globals.instance.components.graphPanel.transform,false);
		node.transform.position = prefab.transform.position;
		node.gameObject.SetActive(true);
		node.processor = processor;
		processor.updatePreview = true;
		Globals.instance.nodes.Add (node);

		node.AcquireID ();

		return node;
	}

	public static NodeController Spawn(ProTeGe.TextureProcessors.TextureProcessor processor, ulong ID){
		GameObject prefab = Globals.instance.components.nodePrefab;
		NodeController node = ((GameObject)GameObject.Instantiate(prefab)).GetComponent<NodeController>();
		node.transform.SetParent(Globals.instance.components.graphPanel.transform,false);
		node.transform.position = prefab.transform.position;
		node.gameObject.SetActive(true);
		node.processor = processor;
		processor.updatePreview = true;
		Globals.instance.nodes.Add (node);

		node.ID = ID;
	
		return node;
	}
}

