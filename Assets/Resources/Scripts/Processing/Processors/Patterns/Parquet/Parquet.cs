using UnityEngine;
using System.Collections;

namespace ProTeGe{
	namespace TextureProcessors{
		namespace Pattern{
			public sealed class ProcessorParquet : TextureProcessor {
				public override string name{ get{ return "herringbone"; } }

				public override int inputsCount{ get{ return 0; } }

				Material m;

				public ProcessorParquet () {
					m = new Material(Shader.Find("ProTeGe/Processors/Pattern/Parquet"));
					AddProperty (new ProcessorProperty_float("Length", 1.0f, 5.0f));
					AddProperty (new ProcessorProperty_float("Size", 1.0f, 5.0f));
					AddProperty (new ProcessorProperty_fixed("Thickness", 0.8f));
					AddProperty (new ProcessorProperty_fixed("Flatness", 1.0f));
				}

				protected override RenderTexture GenerateRenderTexture(int resolution){
					m.SetFloat ("_Thickness", 1.0f - this ["Thickness"]);
					m.SetFloat ("_Flatness", this ["Flatness"]);
					//m.SetFloat ("_Length", this ["Length"]);
					m.SetFloat ("_Length", (int)this ["Length"]*2 + 4.0f);
					m.SetFloat ("_Size", (int)this ["Size"] + 1.0f);
					ProTeGe_Texture t = new ProTeGe_Texture(resolution);
					t.ApplyMaterial(m);
					return t.renderTexture;
				}
			}
		}
	}
}