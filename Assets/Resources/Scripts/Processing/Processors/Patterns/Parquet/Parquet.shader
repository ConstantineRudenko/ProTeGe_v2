// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// used by:
//		Patterns/Lines


Shader "ProTeGe/Processors/Pattern/Parquet"
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
			float _Flatness;
			float _Thickness;
			float _Length;
			float _Size;
			static const float PI = 3.14159265f;

			float4 frag (v2f i) : SV_Target
			{	

				float test = _Flatness;
		        float2 curCoord = i.uv.xy; 
		        curCoord *= sqrt(2); 
		        
				float turn = PI/4;
				float cosTurn = cos(turn);
				float sinTurn = sin(turn);
				float freq = int(_Length*_Size);

				curCoord = mul(curCoord, float2x2 (cosTurn, -sinTurn, sinTurn, cosTurn));
				curCoord = frac(curCoord);

				_Flatness *= 0.999;

				float dist = 0;

				int ySqare = int(curCoord.x * freq + int(curCoord.y * freq) - 0.5);

				if(ySqare%(int)_Length < (int)_Length/2 + 1 && (curCoord.x > 0.5/freq || curCoord.y > 1/freq))
					dist = curCoord.y;

				float start = int(dist * freq);
				start = start / freq;
				float end = start + 1.0 / freq;
				float mid = (end + start) * 0.5;

				dist = abs(dist - mid) / (end - start) * 2;

				float r = dist;
				dist = 0;

			 	int xSqare = int(curCoord.y*freq + int(curCoord.x * freq) + (_Length-1)/2 - frac((_Length)/2));

				if(xSqare%(int)_Length < (int)_Length/2 + 1)
					dist = curCoord.x;

				start = int(dist * freq);
				start = start / freq;
				end = start + 1.0 / freq;
				mid = (end + start) * 0.5;

				dist = abs(dist - mid) / (end - start) * 2;

				float r2 = dist;

				r = min(r,r2);
				r = saturate((r - _Thickness) / (1.0 - _Thickness));
				r = saturate(r / (1.0 - _Flatness));

				return float4(r,r,r,1);


			}
			ENDCG
		}
	}
}
