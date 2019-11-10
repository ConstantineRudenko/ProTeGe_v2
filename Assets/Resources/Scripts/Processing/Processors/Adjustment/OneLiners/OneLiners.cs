using UnityEngine;
using System.Collections;

namespace ProTeGe{
	namespace TextureProcessors{
		namespace Adjustment {
			public sealed class ProcessorGain : LinearProcessorBase{
				public override string name { get { return "Gain"; } }

				protected override string shaderName{
					get { return "ProTeGe/Processors/Adjustment/Gain"; }
				}
			}

			public sealed class ProcessorContrast : LinearProcessorBase{
				public override string name { get { return "Contrast"; } }

				protected override string shaderName{
					get { return "ProTeGe/Processors/Adjustment/Contrast"; }
				}
			}

			public sealed class ProcessorGamma : LinearProcessorBase{
				public override string name { get { return "Gamma"; } }

				protected override string shaderName{
					get { return "ProTeGe/Processors/Adjustment/Gamma"; }
				}
			}

			public class ProcessorInvert : LinearProcessorBase {
				public override string name { get { return "Invert"; } }
				protected override string shaderName {
					get { return "ProTeGe/Processors/Adjustment/Invert"; }
				}
			}

			public class ProcessorSaturate : ZeroPropertiesProcessorBase {
				public override string name { get { return "Saturate"; } }
				protected override string shaderName {
					get { return "ProTeGe/Processors/Adjustment/Saturate"; }
				}
			}
		}

		namespace Other{
			public class ProcessorRemoveAlpha : ZeroPropertiesProcessorBase {
				public override string name { get { return "Remove alpha"; } }
				protected override string shaderName {
					get { return "ProTeGe/Processors/Other/RemoveAlpha"; }
				}
			}
		}
	}
}

