using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProTeGe{
	namespace TextureProcessors{
		namespace Other {
			public class ProcessorRGB : TextureProcessor {

				private Material matRGB;

				public override string name { get { return "RGB"; } }
				public override int inputsCount { get { return 1; } }

				public ProcessorRGB() {
					matRGB = new Material(Shader.Find("ProTeGe/Processors/Other/RGB"));
					AddProperty (new ProcessorProperty_float("R", 1, 1));
					AddProperty (new ProcessorProperty_float("G", 1, 1));
					AddProperty (new ProcessorProperty_float("B", 1, 1));
				}

				protected override RenderTexture GenerateRenderTexture (int resolution){
					matRGB.SetFloat ("R", this ["R"]);
					matRGB.SetFloat ("G", this ["G"]);
					matRGB.SetFloat ("B", this ["B"]);
					return inputs [0].Generate (resolution).ApplyMaterial (matRGB).renderTexture;
				}

				protected override void OnKill (){}
			}
		}
	}
}
