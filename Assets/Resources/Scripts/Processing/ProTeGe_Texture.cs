using UnityEngine;
using System.Collections;

namespace ProTeGe{
	public sealed class ProTeGe_Texture {
		public ProTeGe_Texture (RenderTexture t, bool dontRelease = false)
		{
			CheckTexture (t);
			this._dontRelease = dontRelease;
			_texture = t;
		}

		public ProTeGe_Texture (ProTeGe_Texture other, bool dontRelease = false)
		{
			CopyFrom (other, false);
			_dontRelease = dontRelease;
		}

		public ProTeGe_Texture (int size, bool dontRelease = false)
		{
			_dontRelease = dontRelease;
			_texture = GetTemp (size);
		}

	    public ProTeGe_Texture(bool dontRelease = false)
	    {
	        int size = Globals.instance.textureSize_preview;
	        _dontRelease = dontRelease;
	        _texture = GetTemp(size);
	    }

		public RenderTexture renderTexture {
			get {
				if (_texture == null)
					throw new System.Exception ("using released ProceduralTexture");
				return  _texture;
			}
		}

		public bool dontRelease{ get { return _dontRelease; } }

		public ProTeGe_Texture ApplyMaterial (Material m)
		{
			RenderTexture temp = GetTemp (size);
			Graphics.Blit (renderTexture, temp, m);
			Release ();
			_texture = temp;
			_dontRelease = false;
			return this;
		}

		public ProTeGe_Texture ApplyShader (Shader s)
		{
			Material m = new Material (s);
			ApplyMaterial (m);
			return this;
		}

		public void Release ()
		{
			if (renderTexture == null)
				throw new System.Exception ("Releasing ProceduralTexture with texture = null");
			if (!dontRelease)
				RenderTexture.ReleaseTemporary (renderTexture);
			_texture = null;
		}

		public void ForceRelease ()
		{
			if (renderTexture == null)
				throw new System.Exception ("Releasing ProceduralTexture with texture = null");
			RenderTexture.ReleaseTemporary (renderTexture);
			_texture = null;
		}

		public int size {
			get { return renderTexture.width; }
			set {
				_texture = resizer.Resize (renderTexture, value, value, dontRelease);
				_dontRelease = false;
			}
		}

		public void CopyFrom (ProTeGe_Texture other, bool keepMySize)
		{
			if (size == other.size) {
				Graphics.Blit (other.renderTexture, renderTexture);
				return;
			}
				
			int mySize = size;
			Release ();

			if (keepMySize) {
				ProTeGe_Texture temp = new ProTeGe_Texture (other.renderTexture, true);
				temp.size = mySize;
				_texture = temp.renderTexture;

				_dontRelease = false;

				return;
			}

			_texture = GetTemp (other.size);
			Graphics.Blit (other.renderTexture, renderTexture);

		}

		public ProTeGe_Texture ReplaceByClone ()
		{
			RenderTexture temp = renderTexture;
			_texture = GetTemp (size);
			Graphics.Blit (temp, renderTexture);

			if (dontRelease == false)
				RenderTexture.ReleaseTemporary (temp);

			_dontRelease = false;
			
			return this;
		}

		private B_TextureResizer resizerNearestNeighbour = new TextureResizerNearestNeighbour ();
		private B_TextureResizer resizerBilinear = new TextureResizerBiliniar ();
		private B_TextureResizer resizerBicubic = new TextureResizerBicubic ();
		private RenderTexture _texture;
	    private bool _dontRelease = false;
	    public enum UpscaleModes { NearestNeighbour, Bilinear, Bicubic };

	    public UpscaleModes upscaleMode = UpscaleModes.Bicubic;


	    private B_TextureResizer resizer {
			get {
				switch (upscaleMode) {
				case UpscaleModes.NearestNeighbour:
					return resizerNearestNeighbour;
				case UpscaleModes.Bilinear:
					return resizerBilinear;
				case UpscaleModes.Bicubic:
					return resizerBicubic;
				default:
					return resizerBilinear;
				}
			}
		}

		private RenderTexture GetTemp (int size)
		{
			RenderTexture rt = RenderTexture.GetTemporary (size, size, 0, RenderTextureFormat.ARGBFloat);
			rt.wrapMode = TextureWrapMode.Repeat;
			return rt;
		}

		private void CheckTexture (RenderTexture t)
		{
			if (t == null)
				throw new System.ArgumentNullException ("creating ProceduralTexture with null texture");
			if (t.width != t.height)
				throw new System.ArgumentException ("creating ProceduralTexture with non-square texture");
		}
	}
}