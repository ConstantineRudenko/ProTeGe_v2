using UnityEngine;
using System.Collections;

namespace ProTeGe{
	namespace TextureProcessors{
		namespace Filters{
			public class ProcessorCurlNoise : TextureProcessor{

				private Material m;

				public override string name { get { return "curl noise"; } }
				public override int inputsCount{ get{ return 1; } }

				public ProcessorCurlNoise ()	{	
					m = new Material(Shader.Find("ProTeGe/Processors/Filters/CurlNoise" ));
					inputs [0].emptyTextureType = InputHandler.EmptyTextureType.White;

				}
				 
				protected override RenderTexture GenerateRenderTexture(int resolution){
					float step = 1.0f / Globals.instance.textureSize_preview;
					m.SetFloat("_step", step);

					ProTeGe_Texture t = inputs [0].Generate (resolution);

					t.ApplyMaterial(m);
					return t.renderTexture;
				}

			}
		}
	}
}