using UnityEngine;
using System.Collections;

namespace ProTeGe{
	namespace TextureProcessors{
		namespace Filters{
			public sealed class ProcessorNormalMap : TextureProcessor {
				public override string name{ get{ return "Normal map"; } }

				public override int inputsCount{ get{ return 1; } }

				Material m;

				public ProcessorNormalMap() {
					m = new Material(Shader.Find("ProTeGe/Processors/Filters/Normal map" ));
					AddProperty (new ProcessorProperty_float("Intensity", 1));
					AddProperty (new ProcessorProperty_float("Noise", 0, 1));
				}

				protected override RenderTexture GenerateRenderTexture(int resolution){
					m.SetInt ("_Size", Globals.instance.textureSize_preview);
					m.SetFloat ("_Intensity", 0.01f * this["Intensity"]);
					m.SetFloat ("_Noise", this ["Noise"]);

					ProTeGe_Texture t = inputs [0].Generate (resolution);
					t.ApplyMaterial(m);

					return t.renderTexture;
				}
			}
		}
	}
}