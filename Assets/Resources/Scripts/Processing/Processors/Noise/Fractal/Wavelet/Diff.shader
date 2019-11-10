Shader "ProTeGe/Processors/Noise/Wavelet noise/Diff"
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

			//int _Alternative;
			sampler2D _MainTex;
			sampler2D _Tex2;

			float4 frag (v2f i) : SV_Target
			{
				float4 col = tex2D(_MainTex, i.uv);
				float4 col2 = tex2D(_Tex2, i.uv);

				//if(_Alternative)
					col.xyz = (col.xyz - col2.xyz)*3 + 0.5;
				//else
					//col.xyz = saturate((col.xyz - col2.xyz)*6);


				return col;
			}
			ENDCG
		}
	}
}