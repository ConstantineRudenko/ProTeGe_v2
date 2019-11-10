using UnityEngine;
using System.Collections;

namespace ProTeGe{
	namespace TextureProcessors{
		namespace Filters {
			public sealed class ProcessorBloom : TextureProcessor {
				public override string name{ get{ return "Bloom"; } }

				public override int inputsCount{ get{ return 1; } }

				Material bloomH, bloomV, matAdd, matScreen;

				public ProcessorBloom() {
					bloomH = new Material (Shader.Find ("ProTeGe/Processors/Filters/BloomH"));
					bloomV = new Material (Shader.Find ("ProTeGe/Processors/Filters/BloomV"));
					matAdd = new Material (Shader.Find ("ProTeGe/Processors/Mix/Add"));
					matScreen = new Material (Shader.Find ("ProTeGe/Processors/Mix/Screen"));

					AddProperty (new ProcessorProperty_float ("Range", 8, 64));
					AddProperty (new ProcessorProperty_fixed ("Threshold", 0.1f));
					AddProperty (new ProcessorProperty_fixed ("Strength", 0.1f));
					AddProperty (new ProcessorProperty_dropdown ("Mode", new string[]{ "Add", "Screen" }));

				}

				protected override RenderTexture GenerateRenderTexture(int resolution){
					ProTeGe_Texture t = inputs [0].Generate (resolution);
					float step = 1.0f / Globals.instance.textureSize_preview;

					bloomH.SetFloat ("_step", step);
					bloomV.SetFloat ("_step", step);
					bloomH.SetFloat ("_Range", this["Range"] + 1);
					bloomV.SetFloat ("_Range", this["Range"] + 1);
					bloomH.SetFloat ("_Threshold", this ["Threshold"]);

					t.ApplyMaterial (bloomH);

					t.ApplyMaterial (bloomV);

					ProTeGe_Texture tmp = inputs [0].Generate (resolution);

					if (Mathf.RoundToInt (this ["Mode"]) == 0) {
						matAdd.SetFloat ("_Opacity", this ["Strength"]);

						matAdd.SetTexture ("_Tex2", t.renderTexture);

						tmp.ApplyMaterial (matAdd);
					} 
					else {
						matScreen.SetFloat ("_Opacity", this["Strength"]);

						matScreen.SetTexture ("_Tex2", t.renderTexture);

						tmp.ApplyMaterial (matScreen);
					}

					t.Release();
					return tmp.renderTexture;
				}
			}
		}
	}
}