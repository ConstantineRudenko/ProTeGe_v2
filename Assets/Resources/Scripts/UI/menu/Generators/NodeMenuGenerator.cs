using UnityEngine;
using System.Collections;

namespace ProTeGe{
	namespace MenuLib{
		namespace Generators{

			public class NodeMenuGenerator : MenuGenerator {
				public NodeMenuGenerator(){
					root = new MenuItem("root");

					root.AddChild(new MenuItem("other"));
					root.AddChild(new MenuItem("filters"));
					root.AddChild(new MenuItem("noise"));
					root.AddChild(new MenuItem("pattern"));
					root.AddChild(new MenuItem("mix"));
					root.AddChild(new MenuItem("adjustment"));
					root.AddChild(new MenuItem("transform"));

					root["adjustment"].AddChild(new MenuItem("gain",   delegate {  NodeSpawner.Spawn(new TextureProcessors.Adjustment.ProcessorGain()); }  ));
					//root["adjustment"].AddChild(new MenuItem("gamma",   delegate {  NodeSpawner.Spawn(new TextureProcessors.Adjustment.ProcessorGamma()); }  ));
					root["adjustment"].AddChild(new MenuItem("contrast",  delegate {  NodeSpawner.Spawn(new TextureProcessors.Adjustment.ProcessorContrast()); }  ));
					root["adjustment"].AddChild(new MenuItem("invert",  delegate {  NodeSpawner.Spawn(new TextureProcessors.Adjustment.ProcessorInvert()); }  ));
					root["adjustment"].AddChild(new MenuItem("levels",  delegate {  NodeSpawner.Spawn(new TextureProcessors.Adjustment.ProcessorLevels()); }  ));

					root["transform"].AddChild(new MenuItem("twirl",  delegate {  NodeSpawner.Spawn(new TextureProcessors.Adjustment.ProcessorTwirl()); }  ));
					root["transform"].AddChild(new MenuItem("push",  delegate {  NodeSpawner.Spawn(new TextureProcessors.Adjustment.ProcessorPush()); }  ));


					root["other"].AddChild(new MenuItem("RGB",   delegate {  NodeSpawner.Spawn(new TextureProcessors.Other.ProcessorRGB()); }  ));
					root["other"].AddChild(new MenuItem("HSB",   delegate {  NodeSpawner.Spawn(new TextureProcessors.Other.ProcessorHSB()); }  ));
					root ["other"].AddChild (new MenuItem ("dither", delegate {	NodeSpawner.Spawn (new TextureProcessors.Other.ProcessorDither ());	}));

					root["filters"].AddChild(new MenuItem("gaussian blur",   delegate {  NodeSpawner.Spawn(new TextureProcessors.Filters.ProcessorGaussianBlur()); }  ));
					root["filters"].AddChild(new MenuItem("motion blur",   delegate {  NodeSpawner.Spawn(new TextureProcessors.Filters.ProcessorMotionBlur()); }  ));
					root["filters"].AddChild(new MenuItem("bokeh",   delegate {  NodeSpawner.Spawn(new TextureProcessors.Filters.ProcessorBokeh()); }  ));
					root["filters"].AddChild(new MenuItem("curl",   delegate {  NodeSpawner.Spawn(new TextureProcessors.Filters.ProcessorCurlFilter()); }  ));
					root["filters"].AddChild(new MenuItem("bloom",   delegate {  NodeSpawner.Spawn(new TextureProcessors.Filters.ProcessorBloom()); }  ));
					root["filters"].AddChild(new MenuItem("diffuse",   delegate {  NodeSpawner.Spawn(new TextureProcessors.Filters.ProcessorDiffuse()); }  ));
					root["filters"].AddChild(new MenuItem("normal map",   delegate {  NodeSpawner.Spawn(new TextureProcessors.Filters.ProcessorNormalMap()); }  ));
					root["filters"].AddChild(new MenuItem("ambient occlusion",   delegate {  NodeSpawner.Spawn(new TextureProcessors.Filters.ProcessorAmbientOcclusion()); }  ));
						
					//root["filter"].AddChild(new MenuItem("emboss"));
					//root["filter"].AddChild(new MenuItem("sharpen"));
					//root["filter"].AddChild(new MenuItem("custom matrix"));
					//root["filter"].AddChild(new MenuItem("radial blur"));

					root["pattern"].AddChild(new MenuItem("lines", delegate {  NodeSpawner.Spawn(new TextureProcessors.Pattern.ProcessorLines()); }));
					root["pattern"].AddChild(new MenuItem("circles", delegate {  NodeSpawner.Spawn(new TextureProcessors.Pattern.ProcessorCircles()); }));
					root["pattern"].AddChild(new MenuItem("bricks", delegate {  NodeSpawner.Spawn(new TextureProcessors.Pattern.ProcessorBricks()); }));
					//root["pattern"].AddChild(new MenuItem("knitting", delegate {  NodeSpawner.Spawn(new TextureProcessors.Pattern.ProcessorKnitting()); }));
					root["pattern"].AddChild(new MenuItem("herringbone", delegate{  NodeSpawner.Spawn(new TextureProcessors.Pattern.ProcessorParquet()); }));

					root["noise"].AddChild(new MenuItem("wavelet", delegate {  NodeSpawner.Spawn(new TextureProcessors.Noise.ProcessorWaveletNoise()); }));
					root["noise"].AddChild(new MenuItem("perlin", delegate {  NodeSpawner.Spawn(new TextureProcessors.Noise.ProcessorPerlinNoise()); }));
					root["noise"].AddChild(new MenuItem("simplex", delegate {  NodeSpawner.Spawn(new TextureProcessors.Noise.ProcessorSimplexNoise()); }));
					root["noise"].AddChild(new MenuItem("white", delegate {  NodeSpawner.Spawn(new TextureProcessors.Noise.ProcessorWhiteNoise()); }));
					root["noise"].AddChild(new MenuItem("billow", delegate {  NodeSpawner.Spawn(new TextureProcessors.Noise.ProcessorBillowNoise()); }));

					root ["noise"].AddChild (new MenuItem ("cellular"));
					root ["noise"] ["cellular"].AddChild (new MenuItem ("generator", delegate {
						NodeSpawner.Spawn (new TextureProcessors.Noise.ProcessorCellularNoise ());
					}));
					root ["noise"] ["cellular"].AddChild (new MenuItem ("seeder", delegate {
						NodeSpawner.Spawn (new TextureProcessors.Noise.ProcessorCellularNoiseSeeder ());
					}));

					root["mix"].AddChild(new MenuItem("add", delegate { NodeSpawner.Spawn(new TextureProcessors.Mix.ProcessorAdd()); }));
					root["mix"].AddChild(new MenuItem("blend", delegate { NodeSpawner.Spawn(new TextureProcessors.Mix.ProcessorBlend()); }));
					root["mix"].AddChild(new MenuItem("screen", delegate { NodeSpawner.Spawn(new TextureProcessors.Mix.ProcessorScreen()); }));
					root["mix"].AddChild(new MenuItem("multiply", delegate { NodeSpawner.Spawn(new TextureProcessors.Mix.ProcessorMultiply()); }));
					root["mix"].AddChild(new MenuItem("overlay", delegate { NodeSpawner.Spawn(new TextureProcessors.Mix.ProcessorOverlay()); }));
					root["mix"].AddChild(new MenuItem("subtract", delegate { NodeSpawner.Spawn(new TextureProcessors.Mix.ProcessorSubtract()); }));
					root["mix"].AddChild(new MenuItem("min", delegate { NodeSpawner.Spawn(new TextureProcessors.Mix.ProcessorMin()); }));
					root["mix"].AddChild(new MenuItem("max", delegate { NodeSpawner.Spawn(new TextureProcessors.Mix.ProcessorMax()); }));

					//root["blend"].AddChild(new MenuItem("max"));
					//root["blend"].AddChild(new MenuItem("min"));

					root.Sort();
				}
			}
		}
	}
}
