using UnityEngine;
using System.Collections;

namespace ProTeGe{
	namespace TextureProcessors{
		namespace Adjustment{
			public sealed class ProcessorLevels : TextureProcessor{

				private Material m;

				public override string name {
					get { return "Levels"; }
				}

				public override int inputsCount{ get{ return 1; } }

				public ProcessorLevels()
				{
					m = new Material(Shader.Find("ProTeGe/Processors/Adjustment/Levels"));
					AddProperty(new ProcessorProperty_fixed("Low", 0));
					AddProperty(new ProcessorProperty_fixed("Mid", 0.5f));
					AddProperty(new ProcessorProperty_fixed("High", 1.0f));
				}

				protected override RenderTexture GenerateRenderTexture(int resolution){
					m.SetFloat ("_Low", this ["Low"]);
					m.SetFloat ("_Mid", this ["Mid"]);
					m.SetFloat ("_High", this ["High"]);
					ProTeGe_Texture t = inputs [0].Generate (resolution);
					t.ApplyMaterial(m);
					return t.renderTexture;
				}
			}
		}
	}
}