Shader "ProTeGe/Processors/Mix/Max"
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

			int _Luminance;

			fixed4 frag (v2f i) : SV_Target
			{
				float4 col = tex2D(_MainTex, i.uv);
				float4 col2 = tex2D(_Tex2, i.uv);

				if (!_Luminance){ col.rgb = max(col.rgb, col2.rgb); }
				else { 
				float3 col_tmp = col.xyz * float3(0.2126, 0.7152, 0.0722); 
				float3 col2_tmp = col2.xyz * float3(0.2126, 0.7152, 0.0722);
				col = col_tmp.r+ col_tmp.g + col_tmp.b >= col2_tmp.r + col2_tmp.g + col2_tmp.b ? col : col2; 
				}

				return col;
			}
			ENDCG
		}
	}
}
