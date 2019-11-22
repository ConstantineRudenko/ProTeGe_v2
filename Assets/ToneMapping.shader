Shader "ProTeGe/Image Effects/ToneMapping"
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
			float contrast;
			float rfix;
			float range;

			inline float toe(float x, float contrast)
			{

				return pow(x, contrast - (contrast - 1.0) * x);
			}

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				// just invert the colors
				
				col.rgb = saturate(col.rgb / range);
				col.r = toe(col.r, contrast);
				col.g = toe(col.g, contrast);
				col.b = toe(col.b, contrast);
				float rfixPlus1 = rfix + 1.0;
				col.rgb = col.rgb * float3(rfixPlus1, rfixPlus1, rfixPlus1) / (col.rgb + float3(rfix, rfix, rfix));
				return col;
			}
			ENDCG
		}
	}
}
