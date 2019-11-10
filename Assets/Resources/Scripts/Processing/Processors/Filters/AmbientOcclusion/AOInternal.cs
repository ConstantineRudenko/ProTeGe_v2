using UnityEngine;
using System.Collections;

namespace ProTeGe{
	namespace TextureProcessors{
		namespace Filters{
			public sealed class ProcessorAOInternal : TextureProcessor {
				
				public override string name{ get{ return "AOInternal"; } }

				public override int inputsCount{ get{ return 1; } }

				Material m;
				Material normal;

				public ProcessorAOInternal() {
					m = new Material(Shader.Find("ProTeGe/Processors/Filters/AmbientOcclusion" ));
					normal = new Material (Shader.Find("ProTeGe/Processors/Filters/Normal map"));

					inputs [0].emptyTextureType = InputHandler.EmptyTextureType.White;

					AddProperty (new ProcessorProperty_float("Range", 16, 32));
					AddProperty (new ProcessorProperty_float("Quality", 4, 32));
					AddProperty (new ProcessorProperty_float("Intensity", 0.5f, 1));
				}

				protected override RenderTexture GenerateRenderTexture (int resolution){
					ProTeGe_Texture NormalTexture = inputs [0].Generate (resolution);

					normal.SetInt ("_Size", Globals.instance.textureSize_preview);
					normal.SetFloat ("_Noise", 0);
					normal.SetFloat ("_Intensity", Mathf.Lerp(0.01f, 1f, this ["Intensity"])*0.01f);

					NormalTexture.ApplyMaterial (normal);

					float sampleSize = this ["Range"] * this ["Quality"] / 16.0f + 8.0f;
					float step = 1.0f/resolution;
					float angleStep = Mathf.PI*2/sampleSize;

					m.SetTexture ("_NormalTexture", NormalTexture.renderTexture);
					m.SetFloat ("_Range", this ["Range"]);
					m.SetFloat ("_Intensity", Mathf.Lerp(0.01f, 1f, this ["Intensity"])*0.01f);
					m.SetInt   ("_Quality", (int)sampleSize);
					m.SetFloat ("_angleStep", angleStep);
					m.SetFloat ("_step", step);

					ProTeGe_Texture t = inputs [0].Generate (resolution);
					t.ApplyMaterial(m);


					NormalTexture.Release ();

					return t.renderTexture;
				}
			}
		}
	}
}
