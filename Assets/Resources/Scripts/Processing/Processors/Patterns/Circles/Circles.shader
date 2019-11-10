// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// used by:
//		Patterns/Lines


Shader "ProTeGe/Processors/Pattern/Circles"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
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
			
			sampler2D _MainTex;
			float _Frequency;
			float _Noise;
			float _Flatness;
			float _Thickness;

			float4 frag (v2f i) : SV_Target
			{
				float4 col = tex2D(_MainTex, i.uv);

				float freq = _Frequency;
				freq = int(freq*20)+1;

				_Flatness *= 0.999;

				float dist = length(float2(0.5,0.5)-i.uv.xy);
				//dist = i.uv.x; lines
				dist += _Noise * col.x * 0.5;

				float start = int(dist * freq);
				start = start / freq;
				float end = start + 1.0 / freq;
				float mid = (end + start) * 0.5;

				dist = abs(dist - mid) / (end - start) * 2;

				float r = dist;
				r = saturate(r / _Thickness);

				r = saturate(r - _Flatness) / (1.0 - _Flatness);

				return float4(r,r,r,1);
			}
			ENDCG
		}
	}
}
