Shader "ProTeGe/Processors/Noise/Cellular/Seeder"
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
			float _Randomness;
			float _Seed;
			float _Density;
			int _DistanceMode;

			float rand2D(float2 uv){
				uv.xy += float2(_Seed, _Seed);
				uv = frac(uv);
				float x = frac(cos(uv.x*64)*256);
				float y = frac(cos(uv.y*137)*241);
				float z = x+y;
				return frac(cos((z)*107)*269);
			}	

			float2 random_cell_point(int2 uv){
				
				int2 seamless_uv = uv;

				// do NOT touch it
				// frac() can sometimes create small error
				// and when fed to rand this error
				// leads to completely different results
				seamless_uv.x += seamless_uv.x < 0 ? _Density : 0;
				seamless_uv.x -= seamless_uv.x >= _Density ? _Density : 0;
				seamless_uv.y += seamless_uv.y < 0 ? _Density : 0;
				seamless_uv.y -= seamless_uv.y >= _Density ? _Density : 0;

				float2 rand_offset = rand2D(float2(seamless_uv) / _Density);

				float2 result  = lerp(
							uv + float2(0.5,0.5),
							uv + rand_offset, _Randomness
						);

				return result / _Density;
			}

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
				int2 cellular_uv = int2(i.uv * _Density);

				float near_point_dist = _Density;
				float near_point_dist2 = _Density;
				float near_point_dist3 = _Density;

				float2 near_point;

				for(uint k = 0; k < 25; k++){
					int2 move;
					move.x = int(k % 5) - 2;
					move.y = int(k / 5) - 2;

					float2 p = cellular_uv + move;
					float dist = custom_distance(i.uv, random_cell_point(p), _DistanceMode);

					if(dist <  near_point_dist){
						near_point_dist3 = near_point_dist2;
						near_point_dist2 = near_point_dist;
						near_point_dist = dist;

						near_point = p;
					}
					else{
						if(dist < near_point_dist2){
							near_point_dist3 = near_point_dist2;
							near_point_dist2 = dist;
						}
						else {
							if(dist < near_point_dist3)
								near_point_dist3 = dist;
						}
					}
				}

				// do NOT touch it
				// frac() can sometimes create small error
				// and when fed to rand this error
				// leads to completely different results
				near_point.x += near_point.x > 0 ? _Density : 0;
				near_point.x -= near_point.x >= _Density ? _Density : 0;
				near_point.y += near_point.y > 0 ? _Density : 0;
				near_point.y -= near_point.y >= _Density ? _Density : 0;

				float tileColor = rand2D(near_point / _Density);

				float4 result = float4(near_point_dist, near_point_dist2, near_point_dist3, tileColor);
				result.xyz *= _Density;
				result.xyz *= result.xyz;
				return result;
			}
			ENDCG
		}
	}
}
