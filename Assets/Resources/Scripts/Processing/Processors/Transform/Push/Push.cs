using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProTeGe{
	namespace TextureProcessors{
		namespace Adjustment {
			public class ProcessorPush : TextureProcessor {

				private Material matPush;

				public override string name { get { return "push"; } }
				public override int inputsCount { get { return 3; } }

				public ProcessorPush(){
					matPush = new Material(Shader.Find("ProTeGe/Processors/Transform/Push"));
					inputs [0].emptyTextureType = InputHandler.EmptyTextureType.White;
					inputs [1].emptyTextureType = InputHandler.EmptyTextureType.White;
					inputs [2].emptyTextureType = InputHandler.EmptyTextureType.White;
					AddProperty (new ProcessorProperty_float("Horizontal", 0.4f, 1));
					AddProperty (new ProcessorProperty_float("Vertical", 0.4f, 1));
				}

				protected override RenderTexture GenerateRenderTexture (int resolution){
					ProTeGe_Texture hor, ver;
					hor = inputs [1].Generate (resolution);
					ver = inputs [2].Generate (resolution);
					matPush.SetFloat ("_Horizontal", this ["Horizontal"]);
					matPush.SetFloat ("_Vertical", this ["Vertical"]);
					matPush.SetTexture ("_Tex2", hor.renderTexture);
					matPush.SetTexture ("_Tex3", ver.renderTexture);
					ProTeGe_Texture t = inputs [0].Generate (resolution).ApplyMaterial (matPush);
					hor.Release ();
					ver.Release ();
					return t.renderTexture;
				}

				protected override void OnKill (){}
			}
		}
	}
}