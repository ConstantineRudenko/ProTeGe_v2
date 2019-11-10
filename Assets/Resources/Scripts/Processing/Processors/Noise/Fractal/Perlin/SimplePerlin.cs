using UnityEngine;
using System.Collections;

namespace ProTeGe{
	namespace TextureProcessors{
		namespace Noise{
			public class ProcessorSimplePerlin : TextureProcessor{

				/*
						uses: 
							perlin
				 */

				protected Material m;

				public override string name {
					get { return "Simple perlin noise"; }
				}

				public override int inputsCount{ get{ return 0; } }

				public ProcessorSimplePerlin ()	{
					m = new Material(Shader.Find("ProTeGe/Processors/Noise/Perlin noise/Perlin"));
					AddProperty(new ProcessorProperty_button ("Generate"));
					AddProperty(new ProcessorProperty_int  ("Resolution"));
					AddProperty(new ProcessorProperty_bool ("Cracks"));
					AddProperty(new ProcessorProperty_float("Seed"));
					this["Generate"] = 0;
				}

				protected override RenderTexture GenerateRenderTexture(int resolution){
					m.SetInt("_Resolution", (int)this["Resolution"]);
					m.SetInt("_Modulus", this["Cracks"] == 0 ? 0 : 1);
					m.SetFloat ("_RandomSeed", this ["Seed"]);

					this["Generate"] = this["Generate"] * 0;
					ProTeGe_Texture t = new ProTeGe_Texture();
					t.ApplyMaterial(m);
					return t.renderTexture;
				}
			}
		}
	}
}