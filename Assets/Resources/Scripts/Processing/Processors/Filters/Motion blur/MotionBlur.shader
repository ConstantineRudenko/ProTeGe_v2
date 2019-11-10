Shader "ProTeGe/Processors/Filters/Motion blur"
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
			
			sampler2D _MainTex, _RangeCoefTex;
			float _Iterations, _Ox, _Oy, _RangeVariance;
			int _selectiveSampling;

			float4 frag (v2f i) : SV_Target
			{
				float3 sum = tex2D(_MainTex, i.uv);
				float2 step = float2(_Ox, _Oy);
				float positiveRangeCoef, negativeRangeCoef;
				float sumCoef = 1;

				float myRangeCoef = tex2D(_RangeCoefTex, i.uv);
				myRangeCoef = lerp(1.0, myRangeCoef, _RangeVariance);

				step *= myRangeCoef;

				int k;
				for(k = 1; k < _Iterations; k++){
					if (_selectiveSampling){
						float dist = lerp(myRangeCoef, 0, float(k) / (_Iterations - 1));

						positiveRangeCoef = tex2D(_RangeCoefTex, i.uv + step * k).r;
						negativeRangeCoef = tex2D(_RangeCoefTex, i.uv - step * k).r;

						positiveRangeCoef = lerp(1, positiveRangeCoef, _RangeVariance);
						negativeRangeCoef = lerp(1, negativeRangeCoef, _RangeVariance);

						int positiveActive = positiveRangeCoef >= dist;
						int negativeActive = negativeRangeCoef >= dist;

						sum += positiveActive ?
							tex2D(_MainTex, i.uv + step * k * positiveRangeCoef) :
							0;

						sum += negativeActive ?
							tex2D(_MainTex, i.uv - step * k * negativeRangeCoef) :
							0;

						sumCoef += positiveActive + negativeActive;
					}
					else {
						sum += tex2D(_MainTex, i.uv + step * k);
						sum += tex2D(_MainTex, i.uv - step * k);
						sumCoef = sumCoef + 2;
					}
				}
				return float4(sum / sumCoef, 1.0);
			}
			ENDCG
		}
	}
}

