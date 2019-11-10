using UnityEngine;
using System.Collections;

namespace ProTeGe{
	namespace TextureProcessors{
		namespace Filters {
			public sealed class ProcessorBokeh : TextureProcessor {
				public override string name{ get{ return "Bokeh"; } }

			public override int inputsCount{ get{ return 2; } }

				Material m;

			public ProcessorBokeh() {
				m = new Material(Shader.Find("ProTeGe/Processors/Filters/Motion blur" ));
				AddProperty (new ProcessorProperty_fixed("Quality", 0.3f));
				AddProperty (new ProcessorProperty_float("Range", 2, 4));
				AddProperty (new ProcessorProperty_float("Sides", 1.5f, 5));
				AddProperty (new ProcessorProperty_fixed("Range variance", 1));
				AddProperty (new ProcessorProperty_bool ("Selective Sampling"));

			}
				
			protected override RenderTexture GenerateRenderTexture(int resolution){
				ProTeGe_Texture tex2;
				tex2 = inputs [1].Generate (resolution);
				m.SetTexture ("_RangeCoefTex", tex2.renderTexture);

				m.SetInt ("_selectiveSampling", this ["Selective Sampling"] == 1 ? 1 : 0);

				float qualityCoef = Mathf.Lerp (0.3f, 1, this ["Quality"]);

				float radius = 16 * this["Range"] + 1;

				m.SetFloat ("_Iterations", radius*qualityCoef);

				m.SetFloat ("_RangeVariance",  this ["Range variance"]);

				qualityCoef = Mathf.Ceil(radius * qualityCoef) / radius;

					float step = 1.0f / Globals.instance.textureSize_preview / qualityCoef;

					float tmp = (int)(this ["Sides"]);
					int sides = 4 + (int)tmp * 2;

					float angle = ((sides-2)*Mathf.PI/sides);

					ProTeGe_Texture t = inputs [0].Generate (resolution);
					int i;
					for (i = 0; i < sides/2; i++) {
						m.SetFloat ("_Ox", (Mathf.Cos (angle*i)*step));
						m.SetFloat ("_Oy", (Mathf.Sin (angle*i)*step));
						t.ApplyMaterial (m);
					}

					tex2.Release ();

					return t.renderTexture;
				}
			}
		}
	}
}