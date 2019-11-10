using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ProTeGe{
	public class ProTeGe_Texture_cached{
		public ProTeGe_Texture texture;
		public long cache_id;
	}

	namespace TextureProcessors{

		public class InputHandler {

			private class EmptyTextureProcessor : TextureProcessor {

				private Material matRGB;

				public override string name { get { return "RGB"; } }
				public override int inputsCount { get { return 0; } }

				public EmptyTextureProcessor() {
					matRGB = new Material(Shader.Find("ProTeGe/Processors/Other/RGB"));
					AddProperty (new ProcessorProperty_float("R", 1, 1));
					AddProperty (new ProcessorProperty_float("G", 1, 1));
					AddProperty (new ProcessorProperty_float("B", 1, 1));
				}

				protected override RenderTexture GenerateRenderTexture (int resolution){
					matRGB.SetFloat ("R", this ["R"]);
					matRGB.SetFloat ("G", this ["G"]);
					matRGB.SetFloat ("B", this ["B"]);
					RenderTexture rt = RenderTexture.GetTemporary (resolution, resolution);
					Graphics.Blit (null, rt, matRGB);
					return rt;
				}

				protected override void OnKill (){}
			}

            public enum EmptyTextureType { White, Black, Grey, NormalMap }
            public bool isConnected = false;

            public EmptyTextureType emptyTextureType {
				get {
					return _emptyTextureType;
				}
				set {
					_emptyTextureType = value;
					UpdateEmptyProcessorColor ();
				}
			}

			public void Kill(){
				emptyProcessor.Kill ();
			}

			public TextureProcessor connectedProcessor {
				get {
					if (_connectedProcessor == null)
						return emptyProcessor;
					
					return _connectedProcessor;
				}
				set { 
					if(_connectedProcessor != value)
						cacheID = -1;
					_connectedProcessor = value;
					isConnected = value != null;
				}
			}

			public ProTeGe_Texture Generate (int resolution)
			{
				if (connectedProcessor == null)
					throw new System.Exception ("somehow TextureProcessorInputHandler has connectedProcessor null");

				if (connectedProcessor.isDead)
					connectedProcessor = null;

				System.Tuple<ProTeGe_Texture, long> generated = connectedProcessor.Generate_with_cacheID (resolution);
				cacheID = generated.Item2;

				return generated.Item1;
			}

			public bool IsOutdated(int resolution){
				return connectedProcessor.IsCacheOutdated (resolution, cacheID);
			}

			private TextureProcessor _connectedProcessor;
			private TextureProcessor emptyProcessor;
			private EmptyTextureType _emptyTextureType;
			private long cacheID = -1;

			private void UpdateEmptyProcessorColor(){
				switch (emptyTextureType) {
				case EmptyTextureType.White:
					emptyProcessor ["R"] = 1;
					emptyProcessor ["G"] = 1;
					emptyProcessor ["B"] = 1;
					break;
				case EmptyTextureType.Black:
					emptyProcessor ["R"] = 0;
					emptyProcessor ["G"] = 0;
					emptyProcessor ["B"] = 0;
					break;
				case EmptyTextureType.Grey:
					emptyProcessor ["R"] = 0.5f;
					emptyProcessor ["G"] = 0.5f;
					emptyProcessor ["B"] = 0.5f;
					break;
				case EmptyTextureType.NormalMap:
					emptyProcessor ["R"] = 0.5f;
					emptyProcessor ["G"] = 0.5f;
					emptyProcessor ["B"] = 1;
					break;
				default:
					throw new System.Exception ("unsupported empty texture type");
				}
			}

			public InputHandler(TextureProcessor processor = null, EmptyTextureType emptyTextureType = EmptyTextureType.White){
				connectedProcessor = processor;
				emptyProcessor = new EmptyTextureProcessor ();
				this.emptyTextureType = emptyTextureType;
				UpdateEmptyProcessorColor ();
			}
		}

		public abstract class ProcessorProperty{

            public delegate void PropertyChangedAction(float value);
            public enum ProcessorPropertyType { Integer, Float, Fixed, Bool, Button, Dropdown }

            public bool enabled = true;
            private float _value;
            public bool hidden;

            public virtual float value{
				get{
                    return _value;
				}
				set{
					if(_value != value)
						foreach (PropertyChangedAction hook in hooks)
							hook (value);
                    _value = value;
				}
			}

			public string name { get { return _name; } }

			public ProcessorPropertyType type { get { return _type; } }

			public void AddHook(PropertyChangedAction a){
				if (hooks.Contains (a))
					throw new System.Exception ("adding property hook twice");
				hooks.Add (a);
			}

			public void RemoveHook(PropertyChangedAction a){
				if (hooks.Contains (a) == false)
					throw new System.Exception ("removing property hook which is not even there");
				hooks.Remove(a);
			}

			public virtual bool isSlider{ get{ return false; } }

			private ProcessorPropertyType _type;
			private string _name;
			private List<PropertyChangedAction> hooks;

			public ProcessorProperty(string name, ProcessorPropertyType type, float defaultValue = 0){
				hooks = new List<PropertyChangedAction> ();
				this._name = name;
				this._type = type;
				this.value = defaultValue;
			}
		}

		public class ProcessorProperty_int : ProcessorProperty{
			public override bool isSlider{ get{ return false; } }
			public ProcessorProperty_int(string name, float defaultValue = 1, int savedLimit = 6)
				: base (name, ProcessorPropertyType.Integer, defaultValue){

			}
			public int savedLimit;
			public override float value {
				get{ return base.value; }
				set{
					base.value = Mathf.Round (value);
				}
			}
		}

		public class ProcessorProperty_float : ProcessorProperty{
			public override bool isSlider{ get{ return true; } }
			public ProcessorProperty_float(string name, float defaultValue = 0, float savedLimit = 6)
					: base (name, ProcessorPropertyType.Float, defaultValue){
				this.savedLimit = savedLimit;
			}	
			public float savedLimit;
		}

		public class ProcessorProperty_fixed : ProcessorProperty{
			public override bool isSlider{ get{ return true; } }
			public ProcessorProperty_fixed(string name, float defaultValue)
				: base (name, ProcessorPropertyType.Fixed, defaultValue){
			}
			public override float value{
				get { return base.value; }
				set {
					if (value < 0 || value > 1)
						throw new System.ArgumentOutOfRangeException (
							"TextureProcessorProperty_fixed can only accept values in range [0,1]. received: "
							+ value.ToString ());
					base.value = value;
				}
			}
		}

		public class ProcessorProperty_bool : ProcessorProperty{
			public override bool isSlider{ get{ return false; } }
			public ProcessorProperty_bool(string name, bool defaultValue = false)
				: base (name, ProcessorPropertyType.Bool, defaultValue ? 1:0){

			}
			public override float value {
				get{ return base.value; }
				set {
					base.value = Mathf.RoundToInt (value) == 0 ? 0 : 1;
				}
			}
		}

		public class ProcessorProperty_button : ProcessorProperty{
			public override bool isSlider{ get{ return false; } }
			public ProcessorProperty_button(string name)
				: base (name, ProcessorPropertyType.Button, 0){

			}
			public override float value{
				get{
					return 0;
				}
				set{
					if (Mathf.RoundToInt (value) != 0)
						base.value = base.value == 0 ? 1 : 0;
				}
			}
		}

		public class ProcessorProperty_dropdown : ProcessorProperty{
			public override bool isSlider{ get{ return false; } }
			public string[] options { get; private set; }

			public ProcessorProperty_dropdown(string name, string[] valueNames)
				: base (name, ProcessorPropertyType.Dropdown, 0){
				this.options = valueNames;
			}
		}

	}
}