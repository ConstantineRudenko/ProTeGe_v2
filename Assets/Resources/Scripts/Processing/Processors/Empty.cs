using UnityEngine;
using System.Collections;

namespace ProTeGe{
	namespace TextureProcessors{
		public sealed class EmptyProcessor : TextureProcessor{

			private Material m;

			public override string name {
				get { return "empty node"; }
			}

			public EmptyProcessor(){
				m = new Material(Shader.Find("ProTeGe/Processors/Shared/Value"));
				AddProperty (new ProcessorProperty_fixed("value", 1));
			}

			public override int inputsCount{ get{ return 0; } }

			protected override RenderTexture GenerateRenderTexture(int resolution){
				RenderTexture t = TempRenderTexture(resolution);
				m.SetFloat ("_Value", this ["value"]);
				Graphics.Blit(t, t, m);
				return t;
			}
		}
	}
}