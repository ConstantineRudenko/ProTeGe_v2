using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProTeGe{
	namespace TextureProcessors{
		namespace Other {
			public class ProcessorHSB : TextureProcessor {

				private Material matHSB;

				public override string name { get { return "HSB"; } }
				public override int inputsCount { get { return 1; } }

				public ProcessorHSB() {
					matHSB = new Material(Shader.Find("ProTeGe/Processors/Other/HSB"));
					AddProperty (new ProcessorProperty_float("H", 0.5f, 1));
					AddProperty (new ProcessorProperty_float("S", 1, 2));
					AddProperty (new ProcessorProperty_float("B", 1, 2));
					AddProperty (new ProcessorProperty_bool ("Colorize"));
				}

				protected override RenderTexture GenerateRenderTexture (int resolution){
					matHSB.SetFloat ("_H", this ["H"]);
					matHSB.SetFloat ("_S", this ["S"]);
					matHSB.SetFloat ("_B", this ["B"]);
					matHSB.SetInt ("_Colorize", this ["Colorize"] == 0 ? 0 : 1 );
					return inputs [0].Generate (resolution).ApplyMaterial (matHSB).renderTexture;
				}

				protected override void OnKill (){}
			}
		}
	}
}