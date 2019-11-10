using UnityEngine;
using System.Collections;

namespace ProTeGe{
	namespace TextureProcessors{
		public abstract class ZeroPropertiesProcessorBase : TextureProcessor{
			public override int inputsCount{ get{ return 1; } }

			public ZeroPropertiesProcessorBase ()
			{
				m = new Material(Shader.Find(shaderName));
			}

			protected abstract string shaderName { get; }

			protected override RenderTexture GenerateRenderTexture(int resolution){
				ProTeGe_Texture t = inputs [0].Generate (resolution);
				t.ApplyMaterial(m);
				return t.renderTexture;
			}

			private Material m;
		}

		public abstract class LinearProcessorBase : TextureProcessor{
			public override int inputsCount{ get{ return 1; } }

			public LinearProcessorBase ()
			{
				m = new Material(Shader.Find(shaderName));
				AddProperty (new ProcessorProperty_float (name, 0.5f));
			}

			protected abstract string shaderName { get; }

			protected override RenderTexture GenerateRenderTexture(int resolution){
				m.SetFloat("_" + name, this[name]);

				ProTeGe_Texture t = inputs [0].Generate (resolution);
				t.ApplyMaterial(m);

				return t.renderTexture;
			}

			private Material m;
		}

		public abstract class MixProcessorBase : TextureProcessor{
			public override int inputsCount{ get{ return 3; } }

			public MixProcessorBase () {
				m = new Material(Shader.Find(shaderName));
				AddProperty (new ProcessorProperty_fixed ("Opacity", 0.5f));
				inputs [1].emptyTextureType = emptyTextureType;
			}

			protected abstract string shaderName { get; }

			protected virtual InputHandler.EmptyTextureType emptyTextureType {
				get { return InputHandler.EmptyTextureType.White; }
			}

			protected override RenderTexture GenerateRenderTexture(int resolution){
				ProTeGe_Texture t = inputs [0].Generate (resolution);
				ProTeGe_Texture t2 = inputs [1].Generate (resolution);
				ProTeGe_Texture t3 = inputs [2].Generate (resolution);

				m.SetFloat("_Opacity", this["Opacity"]);
				m.SetTexture("_Tex2", t2.renderTexture);
				m.SetTexture ("_TexCoef", t3.renderTexture);

				t.ApplyMaterial(m);

				t2.Release ();
				t3.Release ();
				return t.renderTexture;
			}

			private Material m;
		}
	}
}