using UnityEngine;
using System.Collections;

namespace ProTeGe{
	namespace TextureProcessors{
		namespace Filters {
			public sealed class ProcessorMotionBlur : TextureProcessor {
				public override string name{ get{ return "Motion blur"; } }

				public override int inputsCount{ get{ return 2; } }

				Material m;

				public ProcessorMotionBlur() {
					m = new Material(Shader.Find("ProTeGe/Processors/Filters/Motion blur" ));
					AddProperty (new ProcessorProperty_float("Range", 2, 8));
					AddProperty (new ProcessorProperty_fixed("Quality", 0.3f));
					AddProperty (new ProcessorProperty_float("Angle", 0, Mathf.PI));
					AddProperty (new ProcessorProperty_fixed("Range variance", 1));
					AddProperty (new ProcessorProperty_bool ("Selective Sampling"));
				}

				protected override RenderTexture GenerateRenderTexture(int resolution){
					ProTeGe_Texture tex2;
					tex2 = inputs [1].Generate (resolution);
					m.SetTexture ("_RangeCoefTex", tex2.renderTexture);

					m.SetInt ("_selectiveSampling", this ["Selective Sampling"] == 1 ? 1 : 0);

					m.SetFloat ("_RangeVariance", this ["Range variance"]);
					float qualityCoef = Mathf.Lerp (0.3f, 1, this ["Quality"]);
					float step = 1.0f / Globals.instance.textureSize_preview;

					float radius = 16 * this["Range"] + 1;
					radius = radius * resolution / 1024;

					m.SetFloat ("_Iterations", radius*qualityCoef);
					qualityCoef = Mathf.Ceil(radius * qualityCoef) / radius;
					m.SetFloat ("_Ox", (Mathf.Cos (this["Angle"])*step)/qualityCoef);
					m.SetFloat ("_Oy", (Mathf.Sin (this["Angle"])*step)/qualityCoef);

					ProTeGe_Texture t = inputs [0].Generate (resolution);
					t.ApplyMaterial (m);
					tex2.Release ();
					return t.renderTexture;
				}
			}
		}
	}
}