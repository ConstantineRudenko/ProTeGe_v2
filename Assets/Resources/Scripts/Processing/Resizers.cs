using UnityEngine;

public abstract class B_TextureResizer{
    protected abstract RenderTexture UpSample(RenderTexture texture, int newWidth, int newHeight, bool dontRelease);

    protected RenderTexture GetTemp (int width, int height){
		RenderTexture rt = RenderTexture.GetTemporary(width, height, 0, RenderTextureFormat.ARGBFloat);
		rt.wrapMode = TextureWrapMode.Repeat;
		return rt;
	}

	public RenderTexture Resize(RenderTexture texture, int newWidth, int newHeight, bool dontRelease = false){
		if(newWidth <= texture.width && newHeight <= texture.height){
			RenderTexture temp = GetTemp (newWidth, newHeight);

			texture.filterMode = FilterMode.Trilinear;
			Graphics.Blit(texture, temp);

			if(!dontRelease)
				RenderTexture.ReleaseTemporary(texture);

			return temp;
		}
		return UpSample(texture,newWidth, newHeight, dontRelease);
	}
}

public class TextureResizerNearestNeighbour : B_TextureResizer{
	protected override RenderTexture UpSample(RenderTexture texture, int newWidth, int newHeight, bool dontRelease){
		RenderTexture temp = GetTemp (newWidth, newHeight);

		texture.filterMode = FilterMode.Point;
		Graphics.Blit(texture, temp);

		if(!dontRelease)
			RenderTexture.ReleaseTemporary(texture);
		
		return temp;
	}
}

public class TextureResizerBiliniar : B_TextureResizer{
	protected override RenderTexture UpSample(RenderTexture texture, int newWidth, int newHeight, bool dontRelease){
		RenderTexture temp = GetTemp (newWidth, newHeight);

		texture.filterMode = FilterMode.Trilinear;
		Graphics.Blit(texture, temp);

		if(dontRelease == false)
			RenderTexture.ReleaseTemporary(texture);
		
		return temp;
	}
}

public class TextureResizerBicubic : B_TextureResizer{

	private static Material UpscaleHorizontalMaterial;
	private static Material UpscaleVerticalMaterial;

	static TextureResizerBicubic(){
		UpscaleHorizontalMaterial = new Material(Shader.Find("ProTeGe/Internal/Upsample/Upsample horizontal"));
		UpscaleVerticalMaterial = new Material(Shader.Find("ProTeGe/Internal/Upsample/Upsample vertical"));
	}

	protected override RenderTexture UpSample(RenderTexture texture, int newWidth, int newHeight, bool dontRelease){
		UpscaleHorizontalMaterial.SetFloat("_InputWidth", texture.width);
		UpscaleVerticalMaterial.SetFloat("_InputHeight", texture.height);

		RenderTexture temp = GetTemp (newWidth, texture.height);
		texture.filterMode = FilterMode.Trilinear;
		Graphics.Blit(texture, temp, UpscaleHorizontalMaterial);

		if(dontRelease == false)
			RenderTexture.ReleaseTemporary(texture);
		texture = temp;

		temp = GetTemp (newWidth, newHeight);
		texture.filterMode = FilterMode.Trilinear;
		Graphics.Blit(texture, temp, UpscaleVerticalMaterial);

		RenderTexture.ReleaseTemporary(texture);

		return temp; 
	}
}