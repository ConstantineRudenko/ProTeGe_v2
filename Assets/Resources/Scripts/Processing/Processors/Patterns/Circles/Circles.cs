using UnityEngine;
using System.Collections;
namespace ProTeGe{
	namespace TextureProcessors{
		namespace Pattern{
			public sealed class ProcessorCircles : TextureProcessor {
				public override string name{ get{ return "circles"; } }

				public override int inputsCount{ get{ return 1; } }

				Material m;

				public ProcessorCircles () {
					m = new Material(Shader.Find("ProTeGe/Processors/Pattern/Circles" ));
					AddProperty (new ProcessorProperty_float("Noise", 1, 2));
					AddProperty (new ProcessorProperty_fixed("Frequency", 0.2f));
					AddProperty (new ProcessorProperty_fixed("Thickness", 1));
					AddProperty (new ProcessorProperty_fixed("Flatness", 0));
				}

				protected override RenderTexture GenerateRenderTexture(int resolution){
					m.SetFloat ("_Noise", this ["Noise"]);
					m.SetFloat ("_Frequency", this ["Frequency"]);
					m.SetFloat ("_Thickness", this ["Thickness"]);
					m.SetFloat ("_Flatness", this ["Flatness"]);

					ProTeGe_Texture t = inputs [0].Generate (resolution);
					t.ApplyMaterial(m);
					return t.renderTexture;
				}
			}
		}
	}
}