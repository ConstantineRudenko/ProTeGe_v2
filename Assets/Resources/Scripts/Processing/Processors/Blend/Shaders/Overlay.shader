Shader "ProTeGe/Processors/Mix/Overlay"
{
	Properties
	{
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_TexCoef ("TexCoef (RGB)", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			float overlay(float x, float y, float k){
				return y > 0.5 ? 1.0 - (1.0 - x)*(1.0 - (2*y-1.0)*k) :
						x * lerp(1.0, y * 2, k);
			}
			
			float _Opacity;
			sampler2D _MainTex;
			sampler2D _Tex2;
			sampler2D _TexCoef;

			float4 frag (v2f i) : SV_Target
			{
				float4 col = tex2D(_MainTex, i.uv);
				float4 col2 = tex2D(_Tex2, i.uv);
				float col3 = tex2D(_TexCoef, i.uv);

				_Opacity = _Opacity * col3.x;

				col.xyz = float3(overlay(col.x, col2.x, _Opacity), overlay(col.y, col2.y, _Opacity), overlay(col.z, col2.z, _Opacity));
				return col;
			}
			ENDCG
		}
	}
}