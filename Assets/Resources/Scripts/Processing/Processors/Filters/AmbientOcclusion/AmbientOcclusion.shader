Shader "ProTeGe/Processors/Filters/AmbientOcclusion"
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
			
			sampler2D _MainTex, _NormalTexture;
			float _Range, _step, _angleStep, _Intensity;
			int _Quality;

			float rand2D(float2 uv){
				uv = frac(uv);
				float x = frac(cos(uv.x*64)*256);
				float y = frac(cos(uv.y*137)*241);
				float z = x+y;
				return frac(cos((z)*107)*269);
			}

			fixed4 frag (v2f i) : SV_Target
			{
				float currAngle = rand2D(i.uv);

				int raymarching_samples = _Quality / 6.283;
				if(raymarching_samples < 2)
					raymarching_samples = 2;
				
				_step = _step*_Range / raymarching_samples;

				float3 normalVector = tex2D(_NormalTexture, i.uv).rgb;
				normalVector -= float3(0.5,0.5,0.5);
				normalVector.y *= -1;
				normalVector = normalize(normalVector);

				float res = 0;

				float curHeigh = tex2D(_MainTex, i.uv).r;

				for (int k = 0; k < _Quality; k++){
					float contribution = 1;

					float radial_jitter = _step * rand2D(i.uv + float2(0,0.5));

					for (int t = 1; t <= raymarching_samples; t++){
						float cur_step = _step * t;
						cur_step -= radial_jitter;

						float2 currVector = float2(cos(currAngle), sin(currAngle));
						float colorDiff = tex2D(_MainTex, i.uv.xy+currVector*cur_step).r - curHeigh;

						float3 vector2 = float3(currVector*cur_step, colorDiff*_Intensity*10);
						vector2 = normalize(vector2);
						
						float coverage = 1.0 - dot(normalVector, vector2);
						coverage = coverage * 1.3;	// bias

						float falloff = (1.0 - float(t-1)/raymarching_samples);
						falloff = -2*falloff*falloff*falloff + 3*falloff*falloff;
						coverage = 1.0 - (1.0-coverage)*falloff;

						if(coverage < contribution)
							contribution = coverage;

						currAngle += _angleStep;
					}

					res += contribution;

				}

				return float4 (res,res,res,_Quality)/_Quality;


			}
			ENDCG
		}
	}
}
				