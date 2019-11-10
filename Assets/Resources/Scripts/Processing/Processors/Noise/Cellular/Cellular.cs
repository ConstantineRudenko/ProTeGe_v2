using UnityEngine;
using System.Collections;

namespace ProTeGe{
	namespace TextureProcessors{
		namespace Noise{
			public class ProcessorCellularNoise : TextureProcessor{

				private Material matCellular;

				public override string name { get { return "cellular generator"; } }
				public override int inputsCount{ get{ return 1; } }
					
				public ProcessorCellularNoise ()	{
					matCellular = new Material(Shader.Find("ProTeGe/Processors/Noise/Cellular/Generator"));

					AddProperty (new ProcessorProperty_dropdown ("Mode", new string[]{
						"bubbles", "stones", "stones 2", "3d bubbles", "tiles" }));
					AddProperty (new ProcessorProperty_bool ("Invert"));
				}

				protected override RenderTexture GenerateRenderTexture(int resolution){
					matCellular.SetInt   ("_Invert", this ["Invert"] == 0 ? 0 : 1);
					matCellular.SetInt ("_Mode", Mathf.RoundToInt (this ["Mode"]));

					ProTeGe_Texture seedTexture = inputs [0].Generate (resolution);
					seedTexture.ApplyMaterial (matCellular);
					return seedTexture.renderTexture;
				}

			}
		}
	}
}