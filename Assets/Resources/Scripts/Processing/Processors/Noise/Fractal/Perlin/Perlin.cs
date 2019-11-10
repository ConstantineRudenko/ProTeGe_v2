using UnityEngine;
using System.Collections;

namespace ProTeGe{
	namespace TextureProcessors{
		namespace Noise{
			public class ProcessorPerlinNoise : ProcessorFractalNoise{

				protected TextureProcessor perlin;
				private Material matAdd;

				public override string name { get { return "Perlin noise"; } }
				public override int inputsCount{ get{ return 0; } }

				protected override Material matGather{ get { return matAdd; } }
				protected override float baseValue{ get { return 0; } }

				protected override void AddSpecificProperties(){
					AddProperty(new ProcessorProperty_bool ("Cracks"));

					AddPropertyHook("Cracks", delegate { ReleaseBands(); } );
					AddPropertyHook("Cracks", delegate (float x) { perlin["Cracks"] = x; } );
				}

				public ProcessorPerlinNoise(){
					matAdd = new Material(Shader.Find("ProTeGe/Processors/Mix/Add"));

					perlin = new ProcessorSimplePerlin();
					perlin.cacheON = false;

					AddPropertyHook("Generate", delegate { perlin ["Seed"] = Time.time; } );
				}

				protected override void OnKill(){
					base.OnKill ();
					perlin.Kill ();
				}

				protected override ProTeGe_Texture GetBandNoise(int i, int resolution){
					perlin["Resolution"] = resolution / Mathf.Pow(2, i+1);
					return perlin.Generate (resolution);
				}
			}
		}
	}
}