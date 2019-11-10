Shader "ProTeGe/Processors/Filters/Gaussian blur"
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
			float _Ox, _Oy, _Iterations, _Range, _RangeVariance;
			int _selectiveSampling;

			float4 frag (v2f i) : SV_Target
			{
				float2 step = float2(_Ox, _Oy);

				float tmp = 1.0 / (_Range * _Range);

				float gauss = 1;
				float helper = exp(tmp * 0.5);
				float helper2 = exp(tmp);

				float3 sum = tex2D(_MainTex, i.uv);
				float sumCoef = 1;
				float positiveRangeCoef, negativeRangeCoef;

				float myRangeCoef = tex2D(_RangeCoefTex, i.uv);
				myRangeCoef = lerp(1, myRangeCoef, _RangeVariance);

				step *= myRangeCoef;

				for(int k = 1; k < _Iterations; k++){
				if (_selectiveSampling){
					gauss = gauss / helper;
					helper = helper * helper2;

					float dist = lerp(myRangeCoef, 0, float(k) / (_Iterations - 1));

					positiveRangeCoef = tex2D(_RangeCoefTex, i.uv + step * k).r;
					negativeRangeCoef = tex2D(_RangeCoefTex, i.uv - step * k).r;
					
					positiveRangeCoef = lerp(1, positiveRangeCoef, _RangeVariance);
					negativeRangeCoef = lerp(1, negativeRangeCoef, _RangeVariance);
					
					int positiveActive = positiveRangeCoef >= dist;
					int negativeActive = negativeRangeCoef >= dist;
					
					sum += positiveActive ?
						tex2D(_MainTex, i.uv + step * k * positiveRangeCoef) * gauss :
						0;
					
					sum += negativeActive ?
						tex2D(_MainTex, i.uv - step * k * negativeRangeCoef) * gauss :
						0;

					sumCoef += (positiveActive + negativeActive)*gauss;
				}
				else{
					gauss = gauss / helper;
					helper = helper * helper2;

					sumCoef = sumCoef + gauss * 2.0;

					sum += tex2D(_MainTex, i.uv + step * k) * gauss;
					sum += tex2D(_MainTex, i.uv - step * k) * gauss;
				}
				}
				return float4(sum / sumCoef, 1.0);
			}
			ENDCG
		}
	}
}

