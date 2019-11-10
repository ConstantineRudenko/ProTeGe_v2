using UnityEngine;
using System.Collections;

namespace ProTeGe{
	namespace TextureProcessors{

		namespace Mix {
			public sealed class ProcessorAdd : MixProcessorBase{
				public override string name { get { return "Add"; } }

				protected override string shaderName{
					get { return "ProTeGe/Processors/Mix/Add"; }
				}

				protected override InputHandler.EmptyTextureType emptyTextureType {
					get { return InputHandler.EmptyTextureType.Black; }
				}
			}

			public sealed class ProcessorBlend : MixProcessorBase{
				public override string name { get { return "Blend"; } }

				protected override string shaderName{
					get { return "ProTeGe/Processors/Mix/Blend"; }
				}
			}

			public sealed class ProcessorScreen : MixProcessorBase{
				public override string name { get { return "Screen"; } }

				protected override string shaderName{
					get { return "ProTeGe/Processors/Mix/Screen"; }
				}

				protected override InputHandler.EmptyTextureType emptyTextureType {
					get { return InputHandler.EmptyTextureType.Black; }
				}

			}

			public sealed class ProcessorMultiply : MixProcessorBase{
				public override string name { get { return "Multiply"; } }

				protected override string shaderName{
					get { return "ProTeGe/Processors/Mix/Multiply"; }
				}

				protected override InputHandler.EmptyTextureType emptyTextureType {
					get { return InputHandler.EmptyTextureType.Black; }
				}

			}

			public sealed class ProcessorOverlay : MixProcessorBase{
				public override string name { get { return "Overlay"; } }

				protected override string shaderName{
					get { return "ProTeGe/Processors/Mix/Overlay"; }
				}

				protected override InputHandler.EmptyTextureType emptyTextureType {
					get { return InputHandler.EmptyTextureType.Grey; }
				}

			}

			public sealed class ProcessorSubtract : MixProcessorBase{
				public override string name { get { return "Subtract"; } }

				protected override string shaderName{
					get { return "ProTeGe/Processors/Mix/Subtract"; }
				}

				protected override InputHandler.EmptyTextureType emptyTextureType {
					get { return InputHandler.EmptyTextureType.White; }
				}
			}

		}
	}
}