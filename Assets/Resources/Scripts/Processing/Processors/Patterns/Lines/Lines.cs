using UnityEngine;
using System.Collections;

namespace ProTeGe{
	namespace TextureProcessors{
		namespace Pattern{
			public sealed class ProcessorLines : TextureProcessor {
				public override string name{ get{ return "lines"; } }

				public override int inputsCount{ get{ return 1; } }

				Material m;

				public ProcessorLines () {
					m = new Material(Shader.Find("ProTeGe/Processors/Pattern/Lines" ));
					AddProperty (new ProcessorProperty_float("Noise", 1, 2));
					AddProperty (new ProcessorProperty_float("Frequency", 0.2f, 1));
					AddProperty (new ProcessorProperty_fixed("Thickness", 1));
					AddProperty (new ProcessorProperty_fixed("Flatness", 0));
					AddProperty (new ProcessorProperty_bool ("Horizontal"));
				}

				protected override RenderTexture GenerateRenderTexture(int resolution){
					m.SetFloat ("_Noise", this ["Noise"]);
					m.SetFloat ("_Frequency", this ["Frequency"]);
					m.SetFloat ("_Thickness", this ["Thickness"]);
					m.SetFloat ("_Flatness", this ["Flatness"]);
					m.SetInt ("_Horizontal", this ["Horizontal"] == 0 ? 0 : 1);
					ProTeGe_Texture t = inputs [0].Generate (resolution);
					t.ApplyMaterial(m);
					return t.renderTexture;
				}
			}
		}
	}
}