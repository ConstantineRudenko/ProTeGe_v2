using UnityEngine;
using System.Collections;

namespace ProTeGe{
	namespace TextureProcessors{
		namespace Pattern{
			public sealed class ProcessorKnitting : TextureProcessor {
				public override string name{ get{ return "knitting"; } }

				public override int inputsCount{ get{ return 0; } }

				Material m;

				public ProcessorKnitting () {
					m = new Material(Shader.Find("ProTeGe/Processors/Pattern/Knitting" ));
					AddProperty (new ProcessorProperty_fixed("Width", 0.1f));
					AddProperty (new ProcessorProperty_fixed("Height", 0.2f));
					AddProperty (new ProcessorProperty_fixed("Size", 0.95f));
					AddProperty (new ProcessorProperty_fixed("Flatness", .8f));
				}

				protected override RenderTexture GenerateRenderTexture(int resolution){
					m.SetFloat ("_Frequency_H", this ["Width"]);
					m.SetFloat ("_Frequency_V", this ["Height"]);
					m.SetFloat ("_Size", this ["Size"]);
					m.SetFloat ("_Flatness", this ["Flatness"]);

					ProTeGe_Texture t = new ProTeGe_Texture(resolution);
					t.ApplyMaterial(m);
					return t.renderTexture;
				}
			}
		}
	}
}