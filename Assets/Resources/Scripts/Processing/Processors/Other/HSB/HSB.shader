// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// used by:
//		Other/Contrast

Shader "ProTeGe/Processors/Other/HSB"
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
			float _H,_S,_B;
			int _Colorize;

			float3 rgb2hsb (float3 col)
			{
				float4 K = float4(0.0, -1.0/3.0, 2.0/3.0, -1.0);
				float4 p = col.g < col.b ? float4(col.bg, K.wz) : float4(col.gb, K.xy);
				float4 q = col.r < p.x ? float4(p.xyw, col.r) : float4(col.r, p.yzx);

				float d = q.x - min(q.w, q.y);
				float e = 1.0e-10; 
				return float3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
			}

			float3 hsb2rgb (float3 col)
			{
				float4 K = float4(1.0, 2.0/3.0, 1.0/3.0, 3.0);
				float3 p = abs(frac(col.xxx + K.xyz) * 6.0 - K.www);
				return col.z * lerp(K.xxx, saturate(p - K.xxx), col.y);
			}

			float4 frag (v2f i) : SV_Target
			{
				float4 col = tex2D(_MainTex, i.uv);

				if (_Colorize) { col *= float4(1,0,0,0);  }

				col = saturate(col);

				float3 hsb = rgb2hsb(float3(col.r, col.g, col.b));

				hsb.x = hsb.x - 0.5 + _H;
				hsb.yz = hsb.yz * float2(_S, _B);

				col.xyz = hsb2rgb(hsb.xyz);

				return col;
			}

			ENDCG
		}
	}
}