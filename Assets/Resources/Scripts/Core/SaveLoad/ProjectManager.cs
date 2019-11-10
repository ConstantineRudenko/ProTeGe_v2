using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProTeGe {
	[System.Serializable()]
	public struct ProjectSaveInfo {

		public struct SavedNodeInfo{
			public Vector2 position;
			public string processorType;
			public ulong ID;
			public List<ulong> input_IDs;
			public Dictionary<string, float> properties;

			public static SavedNodeInfo FromNodeController(NodeController ctrl){
				SavedNodeInfo result = new SavedNodeInfo();
				result.input_IDs = new List<ulong> ();
				result.properties = new Dictionary<string, float> ();

				result.position = (ctrl.transform as RectTransform).anchoredPosition;
				result.processorType = ctrl.processor.GetType ().FullName;
				result.ID = ctrl.ID;
				foreach (InputButtonController input in ctrl.inputs) {
					if (input.connectedProcessor == null)
						result.input_IDs.Add (0);
					else
						result.input_IDs.Add (input.connectedOutputButton.nodeController.ID);
				}

				foreach (string pname in ctrl.processor.GetPropertyList ().Keys)
					result.properties.Add (pname, ctrl.processor [pname]);

				return result;
			}
		}

		public List<SavedNodeInfo> Nodes;
		public SortedSet<ulong> used_node_ids;
		public int resolutionIndex_preview;
		public int resolutionIndex_export;
		public bool realtime_update_preview;
		public ulong albedo_ID;
		public ulong normal_ID;
		public ulong smooth_id;
		public ulong metallic_ID;
		public ulong occlusion_id;
		public ulong parallax_id;
		public ulong emission_id;
	}
	public static class ProjectManager {
		public static ProjectSaveInfo Save(){
			var result = new ProjectSaveInfo ();

			result.Nodes = new List<ProjectSaveInfo.SavedNodeInfo> ();

			foreach (GameObject g in GameObject.FindGameObjectsWithTag("Node")) {
				NodeController ctrl = g.GetComponent<NodeController> ();

				if (ctrl.processor.isDead == false)
					result.Nodes.Add (ProjectSaveInfo.SavedNodeInfo.FromNodeController (ctrl));
			}

			result.used_node_ids = NodeController.used_ids;
			result.resolutionIndex_preview = Globals.instance.resolutionIndex_preview;
			result.resolutionIndex_export = Globals.instance.resolutionIndex_export;
			result.realtime_update_preview = Globals.instance.realtimeUpdatePreview;

			result.albedo_ID = 0;
			result.normal_ID = 0;
			result.smooth_id = 0;
			result.metallic_ID = 0;
			result.occlusion_id = 0;
			result.parallax_id = 0;
			result.emission_id = 0;

			if (Globals.instance.components.outputNode.inputButtonController_albedo.connectedOutputButton != null)
				result.albedo_ID = Globals.instance.components.outputNode.
					inputButtonController_albedo.connectedOutputButton.nodeController.ID;

			if (Globals.instance.components.outputNode.inputButtonController_normalMap.connectedOutputButton != null)
				result.normal_ID = Globals.instance.components.outputNode.
					inputButtonController_normalMap.connectedOutputButton.nodeController.ID;

			if (Globals.instance.components.outputNode.inputButtonController_smoothness.connectedOutputButton != null)
				result.smooth_id = Globals.instance.components.outputNode.
					inputButtonController_smoothness.connectedOutputButton.nodeController.ID;

			if (Globals.instance.components.outputNode.inputbuttonController_metallic.connectedOutputButton != null)
				result.metallic_ID = Globals.instance.components.outputNode.
					inputbuttonController_metallic.connectedOutputButton.nodeController.ID;

			if (Globals.instance.components.outputNode.inputbuttonController_occlusion.connectedOutputButton != null)
				result.occlusion_id = Globals.instance.components.outputNode.
					inputbuttonController_occlusion.connectedOutputButton.nodeController.ID;

			if (Globals.instance.components.outputNode.inputbuttonController_parallax.connectedOutputButton != null)
				result.parallax_id = Globals.instance.components.outputNode.
					inputbuttonController_parallax.connectedOutputButton.nodeController.ID;

			if (Globals.instance.components.outputNode.inputbuttonController_emission.connectedOutputButton != null)
				result.emission_id = Globals.instance.components.outputNode.
					inputbuttonController_emission.connectedOutputButton.nodeController.ID;


			return result;
		}

		public static void Load( ProjectSaveInfo projectSaveInfo ){
			Reset ();

			NodeController.used_ids = projectSaveInfo.used_node_ids;
			Globals.instance.resolutionIndex_preview = projectSaveInfo.resolutionIndex_preview;
			Globals.instance.resolutionIndex_export = projectSaveInfo.resolutionIndex_export;
			Globals.instance.realtimeUpdatePreview = projectSaveInfo.realtime_update_preview;

			Dictionary<ulong, NodeController> loaded_nodes = new Dictionary<ulong, NodeController> ();

			foreach (ProjectSaveInfo.SavedNodeInfo node in projectSaveInfo.Nodes) {
				NodeController ctrl = NodeSpawner.Spawn (
					(TextureProcessors.TextureProcessor)System.Activator.CreateInstance (System.Type.GetType(node.processorType)),
					node.ID
				);

				loaded_nodes.Add (node.ID, ctrl);
				(ctrl.gameObject.transform as RectTransform).anchoredPosition = node.position;

				foreach (string pname in node.properties.Keys)
					ctrl.processor [pname] = node.properties [pname];
			}

			foreach (ProjectSaveInfo.SavedNodeInfo node in projectSaveInfo.Nodes) {
				NodeController ctrl = loaded_nodes [node.ID];

				for (int i = 0; i < node.input_IDs.Count; i++) {
					ulong input_id = node.input_IDs [i];
					if (input_id == 0)
						continue;
					NodeController input_ctrl = loaded_nodes [input_id];
					ctrl.inputs [i].connectedOutputButton = input_ctrl.output;
				}
			}

			if (projectSaveInfo.albedo_ID != 0)
				Globals.instance.components.outputNode.inputButtonController_albedo.connectedOutputButton =
					loaded_nodes [projectSaveInfo.albedo_ID].output;
			
			if (projectSaveInfo.normal_ID != 0)
				Globals.instance.components.outputNode.inputButtonController_normalMap.connectedOutputButton =
					loaded_nodes [projectSaveInfo.normal_ID].output;
			
			if (projectSaveInfo.smooth_id != 0)
				Globals.instance.components.outputNode.inputButtonController_smoothness.connectedOutputButton =
					loaded_nodes [projectSaveInfo.smooth_id].output;

			if (projectSaveInfo.metallic_ID != 0)
				Globals.instance.components.outputNode.inputbuttonController_metallic.connectedOutputButton =
					loaded_nodes [projectSaveInfo.metallic_ID].output;

			if (projectSaveInfo.occlusion_id != 0)
				Globals.instance.components.outputNode.inputbuttonController_occlusion.connectedOutputButton =
					loaded_nodes [projectSaveInfo.occlusion_id].output;

			if (projectSaveInfo.parallax_id != 0)
				Globals.instance.components.outputNode.inputbuttonController_parallax.connectedOutputButton =
					loaded_nodes [projectSaveInfo.parallax_id].output;

			if (projectSaveInfo.emission_id != 0)
				Globals.instance.components.outputNode.inputbuttonController_emission.connectedOutputButton =
					loaded_nodes [projectSaveInfo.emission_id].output;
		}

		public static void Reset(){
			foreach (GameObject x in GameObject.FindGameObjectsWithTag ("Node"))
				GameObject.DestroyImmediate (x);
			Globals.instance.components.outputNode.UpdatePreview ();
		}

	}
}
