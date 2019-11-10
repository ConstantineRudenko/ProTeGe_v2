using UnityEngine;
using System.Collections;

namespace ProTeGe{
	namespace TextureProcessors{
		namespace Noise{
			public class ProcessorCellularNoiseSeeder : TextureProcessor{

				private Material m;

				public override string name { get { return "cellular seeder"; } }
				public override int inputsCount{ get{ return 0; } }

				public ProcessorCellularNoiseSeeder ()	{
					m = new Material(Shader.Find("ProTeGe/Processors/Noise/Cellular/Seeder"));
					AddProperty (new ProcessorProperty_float ("Density", 3f, 4f));
					AddProperty (new ProcessorProperty_fixed ("Order", 0f));
					AddProperty (new ProcessorProperty_dropdown ("Distance mode", new string[]{ "euclidean", "max", "sum" }));
					AddProperty (new ProcessorProperty_button ("Generate"));

					AddProperty (new ProcessorProperty_int ("Seed"));

					AddPropertyHook("Generate", delegate { this["Seed"] ++; Globals.instance.components.nodeGuiCtrl.UpdateGuiPanel(); } );
				}

				protected override RenderTexture GenerateRenderTexture(int resolution){
					int numCells = (int)(Mathf.Pow (2, this ["Density"])) + 1;

					m.SetFloat ("_Density", numCells);
					m.SetFloat ("_Randomness", 1.0f - this ["Order"]);
					m.SetFloat ("_Seed",  this["Seed"] / 100);
					m.SetInt ("_DistanceMode", Mathf.RoundToInt(this ["Distance mode"]));

					ProTeGe_Texture t = new ProTeGe_Texture();
					t.ApplyMaterial(m);
					return t.renderTexture;
				}

			}
		}
	}
}