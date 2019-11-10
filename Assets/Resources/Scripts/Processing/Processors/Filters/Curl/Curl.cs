using UnityEngine;
using System.Collections;

namespace ProTeGe{
	namespace TextureProcessors{
		namespace Filters{
			public sealed class ProcessorCurlFilter : TextureProcessor{

				private Material m;
				private Material curl;
				private ProTeGe.TextureProcessors.Noise.ProcessorFractalNoise perlin;

				public override string name { get { return "curl filter"; } }
				public override int inputsCount{ get{ return 1; } }

				public ProcessorCurlFilter ()	{	
					m = new Material (Shader.Find("ProTeGe/Processors/Filters/Curl" ));
					curl = new Material (Shader.Find("ProTeGe/Processors/Filters/CurlNoise"));

					perlin = new Noise.ProcessorSimplexNoise ();

					inputs [0].emptyTextureType = InputHandler.EmptyTextureType.White;
					AddProperty (new ProcessorProperty_float ("Strength", 1f, 1));
					AddProperty (new ProcessorProperty_fixed ("Quality", 0));

					AddProperty (new ProcessorProperty_float("Fractal", 1, 1));
					AddProperty (new ProcessorProperty_fixed("Big", 1));	
					AddProperty (new ProcessorProperty_fixed("Small", 0));	

					AddProperty (new ProcessorProperty_button("Generate"));

					AddPropertyHook("Generate", delegate {
						perlin["Generate"] = 1;
					} );
				}

				protected override RenderTexture GenerateRenderTexture(int resolution){
					perlin ["Fractal"] = this ["Fractal"];
					perlin ["Big"] = this ["Big"];
					perlin ["Small"] = this ["Small"];

					ProTeGe_Texture noiseTex = perlin.Generate (resolution);

					float qualityCoef = Mathf.Lerp(0.001f, 0.07f, this ["Quality"]);
					float strength = this ["Strength"];
					strength = strength * 1024 * 16;
					float step = 1.0f / Globals.instance.textureSize_preview;
					m.SetFloat ("_step", step/qualityCoef);
					m.SetFloat ("_iterations", strength*qualityCoef + 1);
					m.SetInt ("_Mode", 0); 

					curl.SetFloat("_step", step);
					noiseTex.ApplyMaterial (curl);

					m.SetTexture ("_curlNoiseTex", noiseTex.renderTexture);

					ProTeGe_Texture t = inputs [0].Generate (resolution);

					t.ApplyMaterial(m);

					noiseTex.Release ();

					return t.renderTexture;
				}

			}
		}
	}
}