using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProTeGe{
	namespace TextureProcessors{
		namespace Mix {
			public class ProcessorMax : TextureProcessor {

				private Material matMax;

				public override string name { get { return "max"; } }
				public override int inputsCount { get { return 2; } }

				public ProcessorMax(){
					matMax = new Material(Shader.Find("ProTeGe/Processors/Mix/Max"));
					inputs [0].emptyTextureType = InputHandler.EmptyTextureType.Black;
					inputs [1].emptyTextureType = InputHandler.EmptyTextureType.Black;
					AddProperty (new ProcessorProperty_bool ("Luminance"));
				}

				protected override RenderTexture GenerateRenderTexture (int resolution){
					ProTeGe_Texture tex2;
					tex2 = inputs [1].Generate (resolution);
					matMax.SetInt ("_Luminance", this ["Luminance"] == 0 ? 0 : 1);
					matMax.SetTexture ("_Tex2", tex2.renderTexture);
					ProTeGe_Texture t = inputs [0].Generate (resolution).ApplyMaterial (matMax);
					tex2.Release ();
					return t.renderTexture;
				}

				protected override void OnKill (){}
			}
		}
	}
}