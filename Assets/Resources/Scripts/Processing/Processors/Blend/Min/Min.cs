using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProTeGe{
	namespace TextureProcessors{
		namespace Mix {
			public class ProcessorMin : TextureProcessor {

				private Material matMin;

				public override string name { get { return "min"; } }
				public override int inputsCount { get { return 2; } }

				public ProcessorMin(){
					matMin = new Material(Shader.Find("ProTeGe/Processors/Mix/Min"));
					inputs [0].emptyTextureType = InputHandler.EmptyTextureType.White;
					inputs [1].emptyTextureType = InputHandler.EmptyTextureType.White;
					AddProperty (new ProcessorProperty_bool ("Luminance"));
				}

				protected override RenderTexture GenerateRenderTexture (int resolution){
					ProTeGe_Texture tex2;
					tex2 = inputs [1].Generate (resolution);
					matMin.SetInt ("_Luminance", this ["Luminance"] == 0 ? 0 : 1);
					matMin.SetTexture ("_Tex2", tex2.renderTexture);
					ProTeGe_Texture t = inputs [0].Generate (resolution).ApplyMaterial (matMin);
					tex2.Release ();
					return t.renderTexture;
				}

				protected override void OnKill (){}
			}
		}
	}
}