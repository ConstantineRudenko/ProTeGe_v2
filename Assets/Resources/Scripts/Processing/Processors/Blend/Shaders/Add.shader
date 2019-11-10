Shader "ProTeGe/Processors/Mix/Add"
{
	Properties
	{
		_MainTex ("MainTex (RGB)", 2D) = "white" {}
		_TexCoef ("TexCoef (RGB)", 2D) = "white" {}
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
			
			float _Opacity;
			sampler2D _MainTex;
			sampler2D _Tex2;
			sampler2D _TexCoef;

			float4 frag (v2f i) : SV_Target
			{
				float4 col = tex2D(_MainTex, i.uv);
				float4 col2 = tex2D(_Tex2, i.uv);
				float col3 = tex2D(_TexCoef, i.uv);

				_Opacity = _Opacity * col3.x;

				col.xyz = col.xyz + col2.xyz * _Opacity;
				return col;
			}
			ENDCG
		}
	}
}