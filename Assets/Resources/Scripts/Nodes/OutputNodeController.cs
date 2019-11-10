using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ProTeGe.TextureProcessors;
using ProTeGe;

public class OutputNodeController : MonoBehaviour {
	public InputButtonController inputButtonController_albedo;
	public InputButtonController inputButtonController_normalMap;
	public InputButtonController inputButtonController_smoothness;
	public InputButtonController inputbuttonController_metallic;
	public InputButtonController inputbuttonController_occlusion;
	public InputButtonController inputbuttonController_parallax;
	public InputButtonController inputbuttonController_emission;

	public TextureProcessor processor_albedo;
	public TextureProcessor processor_normalMap;
	public TextureProcessor processor_smoothness;
	public TextureProcessor processor_metallic;
	public TextureProcessor processor_occlusion;
	public TextureProcessor processor_parallax;
	public TextureProcessor processor_emission;

	public void UpdatePreview ()
	{
		WantUpdate = true;	
	}

	void DoUpdate ()
	{
		WantUpdate = false;

		processor_albedo.inputs[0].connectedProcessor = inputButtonController_albedo.connectedProcessor;
		processor_normalMap.inputs[0].connectedProcessor = inputButtonController_normalMap.connectedProcessor;
		processor_smoothness.inputs [0].connectedProcessor = inputButtonController_smoothness.connectedProcessor;
		processor_metallic.inputs [0].connectedProcessor = inputbuttonController_metallic.connectedProcessor;
		processor_occlusion.inputs [0].connectedProcessor = inputbuttonController_occlusion.connectedProcessor;
		processor_parallax.inputs [0].connectedProcessor = inputbuttonController_parallax.connectedProcessor;
		processor_emission.inputs [0].connectedProcessor = inputbuttonController_emission.connectedProcessor;

		UpdateTexture ("_MainTex", processor_albedo);
		UpdateTexture ("_BumpTex", processor_normalMap);
		UpdateTexture ("_SmoothnessTex", processor_smoothness);
		UpdateTexture ("_MetallicTex", processor_metallic);
		UpdateTexture ("_OcclusionTex", processor_occlusion);
		UpdateTexture ("_ParallaxTex", processor_parallax);
		UpdateTexture ("_EmissionTex", processor_emission);
	}

	void UpdateTexture(string name, TextureProcessor processor){
		RenderTexture old = Globals.instance.components.previewRenderer.material.GetTexture (name) as RenderTexture;

		Globals.instance.components.previewRenderer.material.SetTexture (name, null);

		if (old != null)
			RenderTexture.ReleaseTemporary (old);

		Globals.instance.components.previewRenderer.material.SetTexture (
			name,
			processor.Generate (Globals.instance.textureSize_preview).renderTexture
		);
	}

	void LateUpdate ()
	{
		if (Globals.instance.clickBlockerActive == false) {
			if (WantUpdate)
				DoUpdate ();
			else if (Input.GetMouseButtonUp (0) )
				if(Globals.instance.realtimeUpdatePreview == false)
					DoUpdate ();
		}
	}

	void Start ()
	{
		processor_albedo = new ProTeGe.TextureProcessors.Other.ProcessorRemoveAlpha ();
		processor_normalMap = new ProTeGe.TextureProcessors.Other.ProcessorRemoveAlpha ();
		processor_smoothness = new ProTeGe.TextureProcessors.Other.ProcessorRemoveAlpha ();
		processor_metallic = new ProTeGe.TextureProcessors.Other.ProcessorRemoveAlpha ();
		processor_occlusion = new ProTeGe.TextureProcessors.Other.ProcessorRemoveAlpha ();
		processor_parallax = new ProTeGe.TextureProcessors.Other.ProcessorRemoveAlpha ();
		processor_emission = new ProTeGe.TextureProcessors.Other.ProcessorRemoveAlpha ();

		processor_normalMap.inputs [0].emptyTextureType = InputHandler.EmptyTextureType.NormalMap;
		processor_smoothness.inputs [0].emptyTextureType = InputHandler.EmptyTextureType.Grey;
		processor_metallic.inputs [0].emptyTextureType = InputHandler.EmptyTextureType.Black;
		processor_parallax.inputs [0].emptyTextureType = InputHandler.EmptyTextureType.Black;
		processor_emission.inputs [0].emptyTextureType = InputHandler.EmptyTextureType.Black;

		WantUpdate = true;
	}

	private bool WantUpdate = false;

}
