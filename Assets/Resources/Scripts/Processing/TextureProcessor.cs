using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ProTeGe{
	namespace TextureProcessors{
		public abstract class TextureProcessor {

            public bool cacheON = true;
            public bool updatePreview = false;
            public List<ProcessorProperty> properties;
            
            public virtual void OnSnooze() {    }
            public virtual void OnWake() { }
            public abstract string name { get; }
            public abstract int inputsCount { get; }
            protected abstract RenderTexture GenerateRenderTexture(int resolution);

			private Material matSaturate = new Material(Shader.Find("ProTeGe/Processors/Adjustment/Saturate"));

            public ProcessorProperty GetPropertyByName (string name){
				ProcessorProperty p = properties.Find (x => x.name == name);
				if (p == null)
					throw new System.Exception ("missing processor property \"" + name + "\" in \"" + name + "\"");
				else
					return p;
			}

			private long currentCacheID {
				get {
					return _currentCacheID;
				}
				set {
					if (value < 0)
						_currentCacheID = 0;
					else
						_currentCacheID = value;
				}
			}
			private long _currentCacheID = 0;

			public long GetCurrentCacheID(int resolusion){
				return currentCacheID;
			}

			public virtual void OnUpdateResolution(int oldResolution, int newResolution) { }

			public void ReleaseCache (){
				if (cachedTexture == null)
					return;
				
				RenderTexture.ReleaseTemporary (cachedTexture);
				cachedTexture = null;
				currentCacheID ++;
			}

			public float this [string propertyName] {
				get{ return GetProperty (propertyName); }
				set{ SetProperty (propertyName, value); }
			}

			public bool CheckPropertyEnabled (string name){
				ProcessorProperty p = properties.Find (x => x.name == name);
				if (p == null)
					return false;
				return p.enabled;
			}

			public Dictionary<string,ProcessorProperty.ProcessorPropertyType> GetPropertyList (){
				Dictionary<string, ProcessorProperty.ProcessorPropertyType> d = new Dictionary<string,ProcessorProperty.ProcessorPropertyType> ();
				foreach (ProcessorProperty p in properties)
					if(p.hidden != true)
						d.Add (p.name, p.type);
				return d;
			}

			public bool isDead {
				get { 
					return _isDead;
				}
			}

			public void Kill () {
				ReleaseCache ();
				foreach (InputHandler ih in inputs)
					if (ih != null)
						ih.Kill ();
				OnKill ();
				_isDead = true;
			}

			public ProTeGe_Texture Generate (int resolution){
				if (isDead)
					throw new System.InvalidOperationException ("generating texture from a dead processor");

				ValidateCache (resolution);

				if (cacheON == false)
					return new ProTeGe_Texture (GenerateRenderTexture (resolution), false).ApplyMaterial (matSaturate);
				else{
					if (cachedTexture == null)
						cachedTexture = GenerateRenderTexture (resolution);
					return new ProTeGe_Texture (cachedTexture, true).ApplyMaterial (matSaturate);
				}
					
			}

			public System.Tuple<ProTeGe_Texture, long> Generate_with_cacheID(int resolution){
				ProTeGe_Texture t = Generate (resolution);;
				var result = new System.Tuple<ProTeGe_Texture, long> (t, currentCacheID);
				return result;
			}

			public InputHandler[] inputs{ get { return _inputs; } }

			protected void AddPropertyHook(string propertyName, ProcessorProperty.PropertyChangedAction action){
				foreach (ProcessorProperty p in properties) {
					if (p.name == propertyName) {
						p.AddHook (action);
						return;
					}
				}
				throw new System.Exception ("adding hook for non-existent property");
			}

			protected void RemovePropertyHook(string propertyName, ProcessorProperty.PropertyChangedAction action){
				foreach (ProcessorProperty p in properties) {
					if (p.name == propertyName) {
						p.RemoveHook (action);
						return;
					}
				}
				throw new System.Exception ("adding hook for non-existent property");
			}

			protected RenderTexture TempRenderTexture (int size){
				return RenderTexture.GetTemporary (size, size, 0, RenderTextureFormat.ARGBFloat);
			}

			protected virtual void OnKill () { }

			protected void EnableProperty (string name){
				ProcessorProperty p = properties.Find (x => x.name == name);
				if (p != null)
					p.enabled = true;
			}

			protected void DisableProperty (string name){
				ProcessorProperty p = properties.Find (x => x.name == name);
				if (p != null)
					p.enabled = false;
			}

			protected void AddProperty (ProcessorProperty property){
				properties.Add (property);
				property.AddHook (delegate { this.ReleaseCache (); });
			}

			protected void AddHiddenProperty (ProcessorProperty property){
				properties.Add (property);
				property.AddHook (delegate { this.ReleaseCache (); });
				property.AddHook (delegate
					{
						if (updatePreview) {
							if (Globals.instance.realtimeUpdatePreview == true)
								Globals.instance.components.outputNode.UpdatePreview ();
							else
								if (property.isSlider == false)
									Globals.instance.components.outputNode.UpdatePreview ();
						}
					}
				);
				property.hidden = true;
			}

			public RenderTexture cachedTexture = null;

			private readonly InputHandler[] _inputs;
			private bool _isDead = false;

			private float GetProperty (string name){
				ProcessorProperty p = properties.Find (x => x.name == name);
				if (p == null)
					throw new System.Exception ("missing processor property \"" + name + "\" in \"" + this.name + "\"");
				else
					return p.value;
			}

			private void SetProperty (string name, float value){
				ProcessorProperty p = properties.Find (x => x.name == name);
				if (p != null) {
					float old = p.value;
					p.value = value;

					if (old != value) {
						ReleaseCache ();
						if (updatePreview) {
							if (Globals.instance.realtimeUpdatePreview == true)
								Globals.instance.components.outputNode.UpdatePreview ();
							else {
								if (p.type != ProcessorProperty.ProcessorPropertyType.Float)
								if (p.type != ProcessorProperty.ProcessorPropertyType.Fixed)
									Globals.instance.components.outputNode.UpdatePreview ();
							}
						}
					}
				} else throw new System.Exception ("processor \"" + this.name + "\n does not have property " + name);
			}

			public bool IsCacheOutdated(int resolution, long cacheID){
				if (cachedTexture == null) {
					return true;
				}
				
				ValidateCache (resolution);

				return currentCacheID != cacheID;
			}

			private void ValidateCache (int resolution)
			{
				if (cacheON == false)
					ReleaseCache ();

				for (int i = 0; i < inputsCount; i++)
					if (inputs [i].IsOutdated(resolution))
						ReleaseCache ();

				if (cachedTexture != null)
				if (cachedTexture.width != resolution) {
					ReleaseCache ();
				}
			}

			public TextureProcessor(){
				_inputs = new InputHandler[3];

				for(int i = 0; i < inputsCount; i++)
					_inputs[i] = new InputHandler();
				
				properties = new List<ProcessorProperty>();
			}
		}
	}
}