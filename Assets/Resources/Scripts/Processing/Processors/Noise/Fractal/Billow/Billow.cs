using UnityEngine;
using System.Collections;

namespace ProTeGe{
	namespace TextureProcessors{
		namespace Noise{
			public class ProcessorBillowNoise : ProcessorFractalNoise{
				
				protected TextureProcessor cellular;
				protected TextureProcessor seeder;
				private Material matAdd;

				public override string name { get { return "Billow noise"; } }
				public override int inputsCount{ get{ return 0; } }

				protected override Material matGather{ get { return matAdd; } }
				protected override float baseValue{ get { return 0.0f; } }

				public ProcessorBillowNoise(){
					matAdd = new Material(Shader.Find("ProTeGe/Processors/Mix/Add"));

					cellular = new ProcessorCellularNoise ();
					seeder = new ProcessorCellularNoiseSeeder ();
					seeder ["Order"] = 0;

					cellular.cacheON = false;
					seeder.cacheON = false;
					cellular.inputs [0].connectedProcessor = seeder;

					AddPropertyHook("Generate", delegate {
						seeder ["Seed"] = Time.time;
						cellular["Seed"] = Time.time;
					} );
				}

				protected override void AddSpecificProperties(){
					AddProperty (new ProcessorProperty_dropdown ("Mode", new string[]{
						"bubbles", "stones", "stones 2", "3d bubbles", "tiles" }));
					AddProperty (new ProcessorProperty_dropdown ("Distance mode", new string[]{ "euclidean", "max", "sum" }));
					AddProperty (new ProcessorProperty_bool ("Invert"));


					AddPropertyHook ("Distance mode", delegate (float x) { 
						ReleaseBands();
						seeder ["Distance mode"] = x;
					});

					AddPropertyHook ("Mode", delegate  (float x) {
						ReleaseBands();
						cellular ["Mode"] = x;
					});

					AddPropertyHook ("Invert", delegate  (float x) {
						ReleaseBands();
						cellular ["Invert"] = x;
					});
				}

				protected override void OnKill(){
					base.OnKill ();
					cellular.Kill ();
					seeder.Kill ();
				}

				protected override ProTeGe_Texture GetBandNoise(int i, int resolution){
					seeder["Density"] = Mathf.Log(resolution / Mathf.Pow(2, i+2)) / Mathf.Log(2);
					return cellular.Generate (resolution);
				}
			}
		}
	}
}