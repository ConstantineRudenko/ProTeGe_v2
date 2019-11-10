// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// used by:
//		Patterns/Lines


Shader "ProTeGe/Processors/Filters/CurlNoise"
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
			float _step;

			float4 frag (v2f i) : SV_Target
			{
				float4 col = tex2D(_MainTex, i.uv);
				float hor, ver;

				hor = tex2D(_MainTex, float2(i.uv.x, i.uv.y + _step)).r - tex2D(_MainTex, float2(i.uv.x ,i.uv.y - _step)).r;
				ver = tex2D(_MainTex, float2(i.uv.x + _step, i.uv.y)).r - tex2D(_MainTex, float2(i.uv.x - _step, i.uv.y)).r;

				return float4(hor, ver, 0, 0);
			}
			ENDCG
		}
	}
}
