using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProTeGe{
	namespace TextureProcessors{
		namespace Adjustment {
			public class ProcessorTwirl : TextureProcessor {

				private Material matTwirl;

				public override string name { get { return "twirl"; } }
				public override int inputsCount { get { return 1; } }

				public ProcessorTwirl(){
					matTwirl = new Material(Shader.Find("ProTeGe/Processors/Transform/Twirl"));
					AddProperty (new ProcessorProperty_float("Strenght", 0.5f, 1));
					AddProperty (new ProcessorProperty_bool("Direction"));
				}

				protected override RenderTexture GenerateRenderTexture (int resolution){
					matTwirl.SetFloat ("_Twirl", this ["Strenght"]);
					matTwirl.SetInt ("_Direction", this ["Direction"] == 0 ? 0 : 1);
					return inputs [0].Generate (resolution).ApplyMaterial (matTwirl).renderTexture;
				}

				protected override void OnKill (){}
			}
		}
	}
}

