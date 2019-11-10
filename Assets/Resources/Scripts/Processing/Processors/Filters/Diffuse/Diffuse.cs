using UnityEngine;
using System.Collections;

namespace ProTeGe{
	namespace TextureProcessors{
		namespace Filters {
			public sealed class ProcessorDiffuse : TextureProcessor {
				public override string name{ get{ return "Diffuse"; } }

				public override int inputsCount{ get{ return 1; } }

				Material diffuse;

				public ProcessorDiffuse() {
					diffuse = new Material (Shader.Find ("ProTeGe/Processors/Filters/Diffuse"));

					AddProperty (new ProcessorProperty_float ("Range", 8, 16));
				}

				protected override RenderTexture GenerateRenderTexture(int resolution){
					ProTeGe_Texture t = inputs [0].Generate (resolution);
					float step = 1.0f / Globals.instance.textureSize_preview;

					diffuse.SetFloat ("_step", step);
					diffuse.SetFloat ("_Range", this["Range"] + 1);
					t.ApplyMaterial (diffuse);

					return t.renderTexture;
				}
			}
		}
	}
}