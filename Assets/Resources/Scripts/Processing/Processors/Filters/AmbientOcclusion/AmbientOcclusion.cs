using UnityEngine;
using System.Collections;

namespace ProTeGe{
	namespace TextureProcessors{
		namespace Filters{
			public sealed class ProcessorAmbientOcclusion : TextureProcessor {
				
				public override string name{ get{ return "Ambient Occlusion"; } }

				public override int inputsCount{ get{ return 1; } }

				TextureProcessor blur;
				TextureProcessor AO;

				public ProcessorAmbientOcclusion() {
					blur = new ProcessorGaussianBlur ();
					AO = new ProcessorAOInternal ();

					inputs [0].emptyTextureType = InputHandler.EmptyTextureType.White;

					AddProperty (new ProcessorProperty_float("Range", 16, 32));
					AddProperty (new ProcessorProperty_float("Quality", 4, 32));
					AddProperty (new ProcessorProperty_float("Intensity", 0.5f, 1));

					AddProperty (new ProcessorProperty_float("Range Blur", 0f, 4f));
					AddProperty (new ProcessorProperty_fixed("Quality Blur", 0f));
					AddProperty (new ProcessorProperty_fixed("Undersampling Blur", 0.75f));
				}

				protected override RenderTexture GenerateRenderTexture (int resolution){
					AO ["Range"] = this ["Range"];
					AO ["Quality"] = this ["Quality"];
					AO ["Intensity"] = this ["Intensity"];

					AO.inputs [0].connectedProcessor = this.inputs[0].connectedProcessor;

					blur ["Range"] = this ["Range Blur"];
					blur ["Quality"] = this ["Quality Blur"];
					blur ["Undersampling"] = this ["Undersampling Blur"];

					blur.inputs [0].connectedProcessor = AO;

					ProTeGe_Texture t2 = blur.Generate (resolution);

					return t2.renderTexture;
				}

				protected override void OnKill ()
				{
					base.OnKill ();
					blur.Kill ();
					AO.Kill ();
				}
			}
		}
	}
}