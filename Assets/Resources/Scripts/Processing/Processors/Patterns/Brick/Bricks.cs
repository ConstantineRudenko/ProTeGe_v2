using UnityEngine;
using System.Collections;

namespace ProTeGe{
	namespace TextureProcessors{
		namespace Pattern{
			public sealed class ProcessorBricks : TextureProcessor {
				public override string name{ get{ return "bricks"; } }

				public override int inputsCount{ get{ return 0; } }

				Material m;

				public ProcessorBricks () {
					m = new Material(Shader.Find("ProTeGe/Processors/Pattern/Bricks" ));
					AddProperty (new ProcessorProperty_fixed("Width", 0.9f));
					AddProperty (new ProcessorProperty_fixed("Height",0.8f));
					AddProperty (new ProcessorProperty_fixed("Size", 0.95f));
					AddProperty (new ProcessorProperty_fixed("Flatness", 0.2f));
				}

				protected override RenderTexture GenerateRenderTexture(int resolution){
					m.SetFloat ("_Frequency_H", 1.0f - this ["Width"]);
					m.SetFloat ("_Frequency_V", 1.0f - this ["Height"]);
					m.SetFloat ("_Size", this ["Size"]);
					m.SetFloat ("_Flatness", this ["Flatness"]);

					ProTeGe_Texture t = new ProTeGe_Texture();
					t.ApplyMaterial(m);
					return t.renderTexture;
				}
			}
		}
	}
}