// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

//	used in:
//		Noise/SimplePerlin
//		Noise/Perlin

Shader "ProTeGe/Processors/Noise/Perlin noise/Perlin"
{
	Properties
	{

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

			struct v3f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v3f vert (appdata v)
			{
				v3f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			float rand2D(float2 uv){
				uv = frac(uv);
				float x = frac(cos(uv.x*64)*256);
				float y = frac(cos(uv.y*137)*241);
				float z = x+y;
				return frac(cos((z)*107)*269);
			}

			float2 randVector2(float2 uv){
				float h = rand2D(uv);
				uv.x = sin(frac(sin(h*17965))*13572);
				uv.y = cos(frac(cos(h*113))*92245);
				return uv;
			}

			float4 sCurve(float4 x){
				return -2*x*x*x+3*x*x;
				//float4 x2 = x * x;
				//float4 x4 = x2 * x2;
				//return x4 * x * 6 - x4 * 15 + x2 * x * 10;
			}

			uint _Resolution;
			bool _Modulus = false;
			float _RandomSeed;

			fixed4 frag (v3f i) : SV_Target
			{
				float res = _Resolution;
				float2 seed = float2(_RandomSeed, _RandomSeed);

				int x1 = i.uv.x * _Resolution;
				int x2 = x1 + 1;
				int y1 = i.uv.y * _Resolution;
				int y2 = y1 + 1;

				float2 p = i.uv * _Resolution;

				float2 p1 = float2(x1, y1);
				float2 p2 = float2(x2, y1);
				float2 p3 = float2(x1, y2);
				float2 p4 = float2(x2, y2);

				float2 v1 = randVector2(p1 / res + seed);
				float2 v2 = randVector2(p2 / res + seed);
				float2 v3 = randVector2(p3 / res + seed);
				float2 v4 = randVector2(p4 / res + seed);

				float2 pv1 = p1 - p;
				float2 pv2 = p2 - p;
				float2 pv3 = p3 - p;
				float2 pv4 = p4 - p;

				float4 dist;
				dist.x = length(pv1);
				dist.y = length(pv2);
				dist.z = length(pv3);
				dist.w = length(pv4);

				float4 c;
				c.x = dot(pv1, v1);
				c.y = dot(pv2, v2);
				c.z = dot(pv3, v3);
				c.w = dot(pv4, v4);

				dist = 1.0 - saturate(dist);
				dist = sCurve(dist);
				dist /= dot(dist, float4(1,1,1,1));

				c = dot(c, dist);

				if(_Modulus == 0){
					c = (c+1.0)*0.5;
					c = c*2 - 1;
					c = c * 2;
					c = (c + 1)*0.5;
				}
				else
					c = abs(c)*2;

				return c;
			}
			ENDCG
		}
	}
}
