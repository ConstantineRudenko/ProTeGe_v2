Shader "ProTeGe/Processors/Other/Dither"
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
			sampler2D _NoiseTex;
			int _Grayscale;
			float _Strength;

			float4 frag (v2f i) : SV_Target
			{
				float3 col = tex2D(_MainTex, i.uv).xyz;
				float3 col2 = tex2D(_NoiseTex, i.uv).xyz;

				if(_Grayscale)
					col.xyz += float3(1,1,1) * (col2.r - 0.5) * _Strength / 256.0;
				else 
					col.xyz += (col2 - float3(0.5,0.5,0.5)) * _Strength / 256.0;

				return float4(col, 1);
			}
			ENDCG
		}
	}
}
