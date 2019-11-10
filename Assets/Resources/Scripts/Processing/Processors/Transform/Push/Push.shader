Shader "ProTeGe/Processors/Transform/Push"
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
			sampler2D _Tex2;
			sampler2D _Tex3;

			float _Horizontal, _Vertical;

			fixed4 frag (v2f i) : SV_Target
			{
				float2 main = i.uv;
				float4 hor = tex2D(_Tex2, i.uv);
				float4 ver = tex2D(_Tex3, i.uv);

				main.x += hor.x * _Horizontal;
				main.y += ver.y * _Vertical;

				return tex2D(_MainTex, main);
			}
			ENDCG
		}
	}
}
