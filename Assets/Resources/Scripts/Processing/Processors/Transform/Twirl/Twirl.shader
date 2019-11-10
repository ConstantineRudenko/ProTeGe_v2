// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// used by:
//		Other/Contrast

Shader "ProTeGe/Processors/Transform/Twirl"
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
			float _Twirl;
			int _Direction;

			float4 frag (v2f i) : SV_Target
			{
				float2 vec = i.uv;
				vec = vec - float2(0.5, 0.5);
				float r = length(vec);
				float rot = _Direction ? -_Twirl*r : _Twirl*r;
				vec = normalize(vec);
				float alpha = acos(vec.x)*sign(vec.y);
				alpha += rot*5; 
				i.uv.xy = float2(cos(alpha), sin(alpha))*r + float2(0.5, 0.5);
				return tex2D(_MainTex, i.uv);
			}
			ENDCG
		}
	}
}