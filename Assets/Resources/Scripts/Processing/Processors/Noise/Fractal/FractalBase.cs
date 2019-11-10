using UnityEngine;
using System.Collections;

namespace ProTeGe{
	namespace TextureProcessors{
		namespace Noise{
			public abstract class ProcessorFractalNoise : TextureProcessor{
				protected abstract Material matGather { get; }
				protected abstract float baseValue { get; }

				protected abstract ProTeGe_Texture GetBandNoise (int i, int resolution);

				protected virtual void AddSpecificProperties () { }

				private float[] bandValues;
				private ProTeGe_Texture[] bandNoises;
				private Material matValue;

				protected int maxBandValues;

				public ProcessorFractalNoise(){
					matValue = new Material(Shader.Find("ProTeGe/Processors/Shared/Value"));
					matValue.SetFloat ("_Value", baseValue);

					bandNoises = new ProTeGe_Texture[20];
					bandValues = new float[20];

					AddSpecificProperties ();
					AddProperty (new ProcessorProperty_fixed("Fractal", 0.5f));
					AddProperty (new ProcessorProperty_fixed("Big", 1));	
					AddProperty (new ProcessorProperty_fixed("Small", 0));	
					AddProperty (new ProcessorProperty_button("Generate"));

					AddPropertyHook("Generate", delegate { ReleaseBands(); } );
				}

				protected override void OnKill(){
					ReleaseBands ();
				}

				public override void OnSnooze (){
					ReleaseBands ();
				}

				public override void OnWake(){
					if (Globals.instance.lowMemoryMode == false) {
						UpdateBandValues ();
						CreateBandNoises (Globals.instance.textureSize_preview);
					}
				}

				protected void ReleaseBands(){
					for(int i = 0; i < bandNoises.Length; i++){
						if (bandNoises [i] != null) {
							bandNoises [i].Release ();
							bandNoises [i] = null;
						}
					}
				}

				protected sealed override RenderTexture GenerateRenderTexture(int resolution){
					UpdateBandValues ();

					ProTeGe_Texture t = new ProTeGe_Texture (resolution);
					t.ApplyMaterial (matValue);

					if (Globals.instance.lowMemoryMode == false) {
						CreateBandNoises (resolution);

						for (int i = 0; i < bandValues.Length; i++) {
							if (bandValues [i] != 0) {
								matGather.SetFloat ("_Opacity", bandValues [i]);
								matGather.SetTexture ("_Tex2", bandNoises [i].renderTexture);
								t.ApplyMaterial (matGather);
							}
						}
					} else {
						ReleaseBands ();

						for (int i = 1; i < bandValues.Length; i++) {
							if (bandValues [i] != 0) {
								ProTeGe_Texture temp = GetBandNoise (i, resolution);

								matGather.SetFloat ("_Opacity", bandValues [i]);
								matGather.SetTexture ("_Tex2", temp.renderTexture);
								t.ApplyMaterial (matGather);

								temp.Release ();
							}
						}
					}

					return t.renderTexture;
				}

				public sealed override void OnUpdateResolution(int oldResolution, int newResolution){
					int curMaxBandCount = 0;
					for(int i = 0; Mathf.Pow(2, i+2) <= oldResolution; i ++)
						curMaxBandCount ++;

					int newMaxBandsCount = 0;
					for(int i = 0; Mathf.Pow(2, i+2) <= newResolution; i ++)
						newMaxBandsCount ++;

					int lowCut = (int)(curMaxBandCount * this ["Small"]);
					if (lowCut > 0) {
						lowCut = lowCut + newMaxBandsCount - curMaxBandCount;
						this ["Small"] = (float)lowCut / newMaxBandsCount;
					}

					int highCut = (int)Mathf.Lerp(1, curMaxBandCount, this ["Big"]);
					highCut = highCut + newMaxBandsCount - curMaxBandCount;
					this ["Big"] = (float)(highCut-1) / (newMaxBandsCount-1);

					ReleaseBands ();
				}

				private void CreateBandNoises(int resolution){
					for (int i = 0; i < bandValues.Length; i++) 
						if (bandValues [i] != 0 && bandNoises [i] == null)
							bandNoises [i] = GetBandNoise (i, resolution);
				}

				private void UpdateBandValues(){
					int maxBandsCount = 0;
					for(int i = 0; Mathf.Pow(2, i+2) <= Globals.instance.textureSize_preview; i ++)
						maxBandsCount ++;

					int lowCut = (int)(maxBandsCount * this ["Small"]);
					int highCut = (int)Mathf.Lerp(1, maxBandsCount, this ["Big"]);

					if (lowCut > highCut - 1)
						lowCut = highCut - 1;

					for (int i = 0; i < lowCut; i++)
						bandValues [i] = 0;
					for (int i = lowCut; i < highCut; i++)
						bandValues [i] = Mathf.Pow (this ["Fractal"]*3 + 0.1f, i);
					for (int i = highCut; i < bandValues.Length; i++)
						bandValues [i] = 0;

					float sumValues = 0;
					for(int i = 0; i < bandValues.Length; i++)
						sumValues += bandValues[i];

					if(sumValues != 0)
						for(int i = 0; i < bandValues.Length; i++)
							bandValues[i] /=  sumValues;
				}
			}
		}
	}
}