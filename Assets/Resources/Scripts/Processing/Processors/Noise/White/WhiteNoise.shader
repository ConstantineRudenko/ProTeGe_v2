// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

//	used in:
//		Noise/Wavelet
//		Noise/SimpleWhite

Shader "ProTeGe/Processors/Noise/White noise"
{
	Properties
	{

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

			float rand2D(float2 uv){
				uv = frac(uv);
				float x = frac(cos(uv.x*64)*256);
				float y = frac(cos(uv.y*137)*241);
				float z = x+y;
				return frac(cos((z)*107)*269);
			}

			float _RandomSeed;

			float4 frag (v2f i) : SV_Target
			{
				float2 seed = float2(_RandomSeed, _RandomSeed);
				float q  = rand2D(i.uv + seed);
				q = abs(q*2-1);
				return float4(q,q,q,0);
			}    

			ENDCG
		}
	}
}