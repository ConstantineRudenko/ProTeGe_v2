using UnityEngine;
using System.Collections;

namespace ProTeGe{
	namespace TextureProcessors{
		namespace Other {
			public sealed class ProcessorDither : TextureProcessor{

				/*
						uses: 
				  			whiteNoise
				 */

				private TextureProcessors.Noise.ProcessorSimplePerlin p;
				private ProTeGe_Texture noise;
				private Material m;
				private Material mGather;

				public override string name {
					get { return "Dither"; }
				}

				public override int inputsCount{ get{ return 1; } }

				public ProcessorDither()
				{
					AddProperty (new ProcessorProperty_float ("Strength", 1, 1));
					AddProperty (new ProcessorProperty_bool  ("Grayscale"));

					p = new TextureProcessors.Noise.ProcessorSimplePerlin ();
					p.cacheON = false;
					p.updatePreview = false;

					m = new Material(Shader.Find("ProTeGe/Processors/Other/Dither"));
					mGather = new Material(Shader.Find("ProTeGe/Processors/Other/Dither/Gather noises"));
				}

				private void UpdateNoise(int resolution){
					if (noise != null) {
						if (noise.size != Globals.instance.textureSize_preview) {
							noise.Release ();
							noise = null;
						}
					}
					if (noise == null) {
						p ["Resolution"] = Globals.instance.textureSize_preview / 2;

						ProTeGe_Texture r, g, b;

						p ["Seed"] = 1;
						r = p.Generate (resolution);

						p ["Seed"] = 1.2f;
						g = p.Generate (resolution);

						p ["Seed"] = 1.7f;
						b = p.Generate (resolution);

						mGather.SetTexture ("_TexR", r.renderTexture);
						mGather.SetTexture ("_TexG", g.renderTexture);
						mGather.SetTexture ("_TexB", b.renderTexture);

						noise = new ProTeGe_Texture ();
						noise.ApplyMaterial (mGather);

						r.Release ();
						g.Release ();
						b.Release ();
					}
				}

				protected override RenderTexture GenerateRenderTexture(int resolution){
					ProTeGe_Texture t = inputs[0].Generate(resolution);

					UpdateNoise (resolution);

					m.SetTexture ("_NoiseTex", noise.renderTexture);
					m.SetInt   ("_Grayscale", this ["Grayscale"] > 0 ? 1 : 0);
					m.SetFloat ("_Strength", this ["Strength"]);

					t.ApplyMaterial(m);

					return t.renderTexture;
				}

				protected override void OnKill(){
					if (noise != null)
						noise.Release ();

					p.Kill ();
				}
			}
		}
	}
}