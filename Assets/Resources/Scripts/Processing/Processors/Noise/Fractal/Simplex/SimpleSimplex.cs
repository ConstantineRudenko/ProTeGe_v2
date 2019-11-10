using UnityEngine;
using System.Collections;

namespace ProTeGe{
	namespace TextureProcessors{
		namespace Noise{
			public class ProcessorSimpleSimplex : ProcessorSimplePerlin{
				public override string name {
					get { return "Simple simplex noise"; }
				}

				public ProcessorSimpleSimplex (){
					m = new Material(Shader.Find("ProTeGe/Processors/Noise/Simplex noise/Simplex"));
				}
			}
		}
	}
}