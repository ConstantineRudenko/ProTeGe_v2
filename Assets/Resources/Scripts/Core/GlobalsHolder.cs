using UnityEngine;
using System.Collections.Generic;

public class Globals {
	public GlobalsHolder components = GameObject.FindGameObjectWithTag("Globals").GetComponent<GlobalsHolder>();
	public bool lowMemoryMode { get{ return realtimeUpdatePreview == false; } }
	public bool realtimeUpdatePreview = true;
	public List<NodeController> nodes = new List<NodeController> ();
	public int resolutionIndex_export = 2;

	private Globals () {
		
	}

	public static Globals instance{
		get{
			if (_instance == null)
				_instance = new Globals ();
			return _instance;
		}
	}

	private int ResolutionIndexToSize(int index){
		return (int)(Mathf.Pow (2, index - 1) * 1024);
	}

	public bool clickBlockerActive{
		get{
			if (components.clickBlocker == null)
				return false;
			return components.clickBlocker.activeSelf;
		}
		set{
			if (components.clickBlocker == null)
				return;
			components.clickBlocker.SetActive (value);
		}
	}

	public long NextCacheID(){
		curCacheID ++;
		if (curCacheID < 0)
			curCacheID = 0;
		return curCacheID;
	}
	private long curCacheID;

	public int resolutionIndex_preview{
		get { 
			return _selectedResolutionIndex;
		}
		set{ 
			foreach (NodeController node in nodes)
				node.processor.OnUpdateResolution (textureSize_preview, (int)(Mathf.Pow (2, value - 1) * 1024));
			_selectedResolutionIndex = value;

			components.nodeGuiCtrl.UpdateGuiPanel ();
		}
	}

	public Vector3 WorldToScreen (Vector3 pos)
	{
		return components.mainCamera.WorldToScreenPoint (pos);
	}

	public Vector3 ScreenToWorld (Vector3 pos)
	{
		pos.z = 50;	//canvas plane distance
		return components.mainCamera.ScreenToWorldPoint (pos);
	}

	public MeshRenderer previewRenderer {
		get { return components.previewRenderer; }
	}

	public Texture2D RenderTextureToTexture2D (RenderTexture rt)
	{
		Texture2D t2d = new Texture2D (rt.width, rt.height, TextureFormat.ARGB32, false);
		RenderTexture.active = rt;
		t2d.ReadPixels (new Rect (0, 0, rt.width, rt.height), 0, 0);
		t2d.Apply ();
		RenderTexture.active = null;
		return t2d;
	}

	public int textureSize_preview {
		get {
			return ResolutionIndexToSize (resolutionIndex_preview);
		}
	}

	public int textureSize_export {
		get {
			return ResolutionIndexToSize (resolutionIndex_export);
		}
	}

	private static Globals _instance;
	private int _selectedResolutionIndex = 1;
}

public class GlobalsHolder  : MonoBehaviour {
	public Camera mainCamera;
	public Camera previewCamera;

	public GameObject graphPanel;
	public OutputNodeController outputNode;
	public NodeGuiController nodeGuiCtrl;
	public DialogManager dialogManager;

	public UnityEngine.UI.Text SelectedNodeNameDisplay;

	public MeshRenderer previewRenderer;
	public GameObject nodePrefab;
	public GameObject clickBlocker;
	public FileDialogController fileDialogController;

	[HideInInspector]
	public OutputButtonController buttonPressedOutput;

	public Font font;

	public void DeleteNode (NodeController node)
	{
		Globals.instance.components.SelectedNodeNameDisplay.text = "none";
		Globals.instance.components.nodeGuiCtrl.selectedNode = null;
		if (node == null)
			return;
		if (node.processor == null)
			throw new System.Exception ("processor null");
		node.processor.Kill ();
		if (node != null)
			Destroy (node.gameObject);
		Globals.instance.components.outputNode.UpdatePreview ();
	}

}
