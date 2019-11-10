// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

//	used in:
//		Noise/Wavelet
//		ProceduralTexture

Shader "ProTeGe/Internal/Upsample/Upsample vertical"
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

			inline float pow3(float x) { return x*x*x; }
			inline float pow2(float x) { return x*x; }

			sampler2D _MainTex;
			uint _InputHeight = 1;

			float4 frag (v2f i) : SV_Target
			{
				float height = _InputHeight;

				float pos = i.uv.y * height;
				float x = frac(pos);

				float pos0 = floor(pos) - 1;
				float pos1 = pos0 + 1;
				float pos2 = pos0 + 2;
				float pos3 = pos0 + 3;

				float p0 = frac(pos0 / height);
				float p1 = frac(pos1 / height);
				float p2 = frac(pos2 / height);
				float p3 = frac(pos3 / height);

				float4 c0 = tex2D(_MainTex, float2(i.uv.x, p0));
				float4 c1 = tex2D(_MainTex, float2(i.uv.x, p1));
				float4 c2 = tex2D(_MainTex, float2(i.uv.x, p2));
				float4 c3 = tex2D(_MainTex, float2(i.uv.x, p3));

				float3 d0 = c1.xyz - c0.xyz;
				float3 d1 = c2.xyz - c1.xyz;
				float3 d2 = c3.xyz - c2.xyz;

				float3 k0 = lerp(d0,d1,0.5);
				float3 k1 = lerp(d1,d2,0.5);

				float3 a = k0;
				float3 c = k1 + k0 - 2*d1;
				float3 b = 3*d1 - 2*k0 - k1;

				float3 r = a*x + b*x*x + c*x*x*x;

				float3 rn = r + c1;

				return float4(rn,1);
			}    

			ENDCG
		}
	}
}