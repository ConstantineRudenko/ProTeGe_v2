Shader "ProTeGe/Processors/Filters/BloomV"
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
			float _Range;
			float _step;

			float4 frag (v2f i) : SV_Target
			{
				float3 res = _Range;

				for(int k = -_Range; k <= _Range; k++){
					float vert_dist =  abs(k);
					float2 move = float2(0,_step*k);
					float3 horDist = tex2D(_MainTex, i.uv + move).xyz;
					float3 total_dist = sqrt(horDist*horDist + vert_dist*vert_dist);
					res = min(res, total_dist);
				}
				res = max(0,1.0 - res / _Range);
				return float4(res,1);

			}
			ENDCG
		}
	}
}

