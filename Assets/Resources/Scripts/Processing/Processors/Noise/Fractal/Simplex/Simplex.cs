using UnityEngine;
using System.Collections;

namespace ProTeGe{
	namespace TextureProcessors{
		namespace Noise{
			public sealed class ProcessorSimplexNoise : ProcessorPerlinNoise {
				public ProcessorSimplexNoise () {
					perlin.Kill ();
					perlin = new ProcessorSimpleSimplex ();
				}

				public override string name {
					get { return "Simplex noise"; }
				}

			}
		}
	}
}
