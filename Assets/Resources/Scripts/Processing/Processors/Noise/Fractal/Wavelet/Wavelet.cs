using UnityEngine;
using System.Collections;

namespace ProTeGe{
	namespace TextureProcessors{
		namespace Noise{
			public sealed class ProcessorWaveletNoise : ProcessorFractalNoise{

				private ProcessorWhiteNoise white;
				private Material  matGatherWavelet;
				private Material matDiff;

				public override string name { get { return "Wavelet noise"; } }
				public override int inputsCount{ get{ return 0; } }

				protected override Material matGather{ get { return matGatherWavelet; } }
				protected override float baseValue{ get { return 0.5f; } }

				public ProcessorWaveletNoise () {
					matGatherWavelet = new Material(Shader.Find("ProTeGe/Processors/Noise/Wavelet noise/Gather"));
					matDiff = new Material(Shader.Find("ProTeGe/Processors/Noise/Wavelet noise/Diff"));

					white = new ProcessorWhiteNoise();
					white.cacheON = false;

					AddPropertyHook("Generate", delegate { white ["Seed"] = Time.time; } );
				}

				protected override void OnKill(){
					base.OnKill ();
					white.Kill ();
				}

				protected override ProTeGe_Texture GetBandNoise(int i, int resolution){
					int noiseSize = (int)(resolution / Mathf.Pow(2, i));

					ProTeGe_Texture noise = white.Generate (noiseSize);
					noise.size = noiseSize;

					if (noise.size >= 4) {
						ProTeGe_Texture scaledNoise = new ProTeGe_Texture (noise.size / 2);
						scaledNoise.CopyFrom (noise, true);
						scaledNoise.upscaleMode = ProTeGe_Texture.UpscaleModes.Bicubic;
						scaledNoise.size *= 2;

						matDiff.SetTexture ("_Tex2", scaledNoise.renderTexture);
						noise.ApplyMaterial (matDiff);

						scaledNoise.Release ();
					}

					noise.upscaleMode = ProTeGe_Texture.UpscaleModes.Bicubic;
					noise.size = Globals.instance.textureSize_preview;

					return noise;
				}
			}
		}
	}
}