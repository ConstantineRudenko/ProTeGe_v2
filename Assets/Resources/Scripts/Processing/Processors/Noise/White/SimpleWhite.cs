using UnityEngine;
using System.Collections;

namespace ProTeGe{
	namespace TextureProcessors{
		namespace Noise{
			public sealed class ProcessorWhiteNoise : TextureProcessor{

				/*
						uses: 
				  			whiteNoise
				 */

				private Material m;

				public override string name {
					get { return "White noise"; }
				}

				public override int inputsCount{ get{ return 0; } }

				public ProcessorWhiteNoise()
				{
					m = new Material(Shader.Find("ProTeGe/Processors/Noise/White noise"));
					AddProperty(new ProcessorProperty_float("Seed"));
				}

				protected override RenderTexture GenerateRenderTexture(int resolution){
					m.SetFloat ("_RandomSeed", this ["Seed"]);
					ProTeGe_Texture t = new ProTeGe_Texture(resolution);
					t.ApplyMaterial(m);
					return t.renderTexture;
				}
			}
		}
	}
}