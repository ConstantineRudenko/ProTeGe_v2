// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// used by:
//		Other/Contrast

Shader "ProTeGe/Processors/Adjustment/Levels"
{
	Properties
	{
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
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

			float _Low;
			float _Mid;
			float _High;

			float4 frag (v2f i) : SV_Target
			{
				float4 col = tex2D(_MainTex, i.uv);

				float3 low = float3(1,1,1) * _Low;
				float gamma = log(0.5) / log(_Mid);

				col.xyz = (col.xyz - low) / (float3(1,1,1) - low);
				col.xyz = saturate(col.xyz);
				col.xyz /= _High;
				col.xyz = pow(col.xyz, gamma);

				return col;
			}
			ENDCG
		}
	}
}