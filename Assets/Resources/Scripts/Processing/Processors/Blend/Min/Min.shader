Shader "ProTeGe/Processors/Mix/Min"
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
				float3 luma = float3(0.2126, 0.7152, 0.0722);

				if (_Luminance){
					col = dot(col.xyz, luma) <= dot(col2.xyz, luma) ? col : col2; 
				}
				else { 
					col.rgb = min(col.rgb, col2.rgb);
				}

				return col;
			}
			ENDCG
		}
	}
}
