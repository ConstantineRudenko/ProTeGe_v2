Shader "ProTeGe/Processors/Noise/Cellular/Generator"
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
			int _Invert;
			int _Mode;

			float custom_distance(float2 a, float2 b, int mode){
				if(mode == 0)
					return distance(a,b);
				if(mode == 1)
					return max(abs(a.x-b.x), abs(a.y-b.y));
				if(mode == 2)
					return abs(a.x-b.x) + abs(a.y-b.y);
				return 0;
			}

			fixed4 frag (v2f i) : SV_Target
			{	
				float4 result;

				float4 c = tex2D(_MainTex, i.uv);
				float near_point_dist = c.r;
				float near_point_dist2 = c.g;
				float near_point_dist3 = c.b;
				float color_seed = c.a;

				if(_Mode == 0){
					result = near_point_dist;
				}
				if(_Mode == 1){
					//near_point_dist2 *= near_point_dist2;
					near_point_dist = near_point_dist2 - near_point_dist;

					result = near_point_dist;
				}
				if(_Mode == 2){
					near_point_dist = near_point_dist3 - near_point_dist2;
					result = near_point_dist;
				}
				if(_Mode == 3){
					near_point_dist2 /= 1.4142;
					near_point_dist = sqrt(near_point_dist + near_point_dist2 * near_point_dist2);
					near_point_dist = 1.0 - near_point_dist;

					result = near_point_dist;
				}
				if(_Mode == 4){
					result = color_seed;
				}

				if(_Invert)
					result = 1.0 - result;

				return result;


				// DEBUG STUFF
				//
				//float margin = 0.002;
				//float randh = rand2D(float2(squarex/_Density, squarey/_Density));
				//float randv = rand2D(float2(squarex/_Density + 0.5*squarex/_Density, squarey/_Density));
				//float randx = rand2(float2(squarex,squarey)).x;
				//float randy = rand2(float2(squarex,squarey)).y;
				//if (abs(i.uv.x - randx) < margin && abs(i.uv.y - randy) < margin) {return 0;} // Debug dots
				//if (abs(squarex/_Density - i.uv.x) < margin || abs(squarey/_Density - i.uv.y) < margin ) {return 0;} // Debug grid 
				//if ((1-i.uv.x < margin) || (1-i.uv.y < margin)) { return 1; } // Debug separator
				//
				// DEBUG STUFF 
			}
			ENDCG
		}
	}
}
