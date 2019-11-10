using UnityEngine;
using System.Collections;

namespace ProTeGe{
	namespace TextureProcessors{
		namespace Filters {
			public sealed class ProcessorGaussianBlur : TextureProcessor {
				public override string name{ get{ return "Gaussian blur"; } }

				public override int inputsCount{ get{ return 2; } }

				Material m;

				public ProcessorGaussianBlur() {
					m = new Material(Shader.Find("ProTeGe/Processors/Filters/Gaussian blur" ));
					AddProperty (new ProcessorProperty_float("Range", 2, 4));
					AddProperty (new ProcessorProperty_fixed("Quality", 0.3f));
					AddProperty (new ProcessorProperty_fixed("Undersampling", 0.75f));
					AddProperty (new ProcessorProperty_fixed("Range variance", 1));
					AddProperty (new ProcessorProperty_bool ("Selective Sampling"));
				}

				protected override RenderTexture GenerateRenderTexture(int resolution){
					m.SetInt ("_selectiveSampling", this ["Selective Sampling"] == 1 ? 1 : 0);

					float qualityCoef = Mathf.Lerp (1.0f / 16, 1, this ["Quality"]);

					float range = this ["Range"] * qualityCoef * resolution / 1024;
					range = range * 12.5f; //for visual consistency with same range bokeh with 4 + 0 sides
					float radius = range * 3 * Mathf.Lerp(1, 0.625f, this["Undersampling"]);
					float step = 1.0f / Globals.instance.textureSize_preview / qualityCoef;

					ProTeGe_Texture rangeCoefTex = inputs [1].Generate (resolution);

					m.SetFloat ("_Iterations", radius);
					m.SetFloat ("_Range", range);
					m.SetFloat ("_Ox", step);
					m.SetFloat ("_Oy", 0);
					m.SetTexture ("_RangeCoefTex", rangeCoefTex.renderTexture);
					m.SetFloat("_RangeVariance", this["Range variance"]);

					ProTeGe_Texture t = inputs [0].Generate (resolution);
					t.ApplyMaterial (m);

					m.SetFloat ("_Ox", 0);
					m.SetFloat ("_Oy", step);

					t.ApplyMaterial (m);

					rangeCoefTex.Release ();
					return t.renderTexture;
				}
			}
		}
	}
}