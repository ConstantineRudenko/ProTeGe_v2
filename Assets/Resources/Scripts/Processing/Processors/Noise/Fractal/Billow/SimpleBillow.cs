using UnityEngine;
using System.Collections;

namespace ProTeGe{
	namespace TextureProcessors{
		namespace Noise{
			public class ProcessorSimpleBillow : TextureProcessor{
				protected Material seeder;
				protected Material generator;

				public override string name { get { return "simple billow noise"; } }
				public override int inputsCount{ get{ return 0; } }

				public ProcessorSimpleBillow ()	{
					generator = new Material(Shader.Find("ProTeGe/Processors/Noise/Cellular/Generator"));
					seeder = new Material(Shader.Find("ProTeGe/Processors/Noise/Cellular/Seeder"));

					AddProperty (new ProcessorProperty_float ("Density", 3f, 4f));
					AddProperty (new ProcessorProperty_button ("Generate"));
					AddProperty (new ProcessorProperty_int ("Seed"));

					AddPropertyHook("Generate", delegate { this["Seed"] ++; Globals.instance.components.nodeGuiCtrl.UpdateGuiPanel(); } );
				}

				protected override RenderTexture GenerateRenderTexture(int resolution){
					int numCells = (int)(Mathf.Pow (2, this ["Density"])) + 1;
					seeder.SetFloat ("_Density", numCells);
					seeder.SetInt   ("_DistanceMode", 1);
					seeder.SetFloat ("_Randomness", 1.0f);
					seeder.SetFloat ("_Seed",  this["Seed"] / 100);

					ProTeGe_Texture seed = new ProTeGe_Texture ();
					seed.ApplyMaterial (seeder);

					generator.SetInt   ("_Invert", 1);
					generator.SetFloat ("_Seed",  this["Seed"] / 100);
					generator.SetInt   ("_Mode", 0);

					seed.ApplyMaterial(generator);

					return seed.renderTexture;
				}
			}
		}
	}
}