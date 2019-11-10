// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// used by:
//		Patterns/Lines


Shader "ProTeGe/Processors/Filters/Curl"
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
			
			sampler2D _MainTex, _curlNoiseTex;
			float _step, _iterations;
			int _Mode;

			float4 frag (v2f i) : SV_Target
			{
				float2 _vector = tex2D(_curlNoiseTex, i.uv).rg;
				float2 curCoor = i.uv;
				float3 sum = tex2D(_MainTex, i.uv);

				if(_Mode == 0){
					for (int k = 0; k < _iterations; k++){
						curCoor += _vector*_step;
						sum += tex2D(_MainTex, curCoor); 
						_vector = tex2D(_curlNoiseTex, curCoor).rg;
					}

					sum /= _iterations + 1;

				}
				if(_Mode == 1){
					float2 _vectorN = _vector;
					float2 curCoorN = curCoor;

					for (int k = 0; k < _iterations; k++){
						curCoor += _vector*_step;
						curCoorN -= _vectorN*_step;

						sum += tex2D(_MainTex, curCoor); 
						sum += tex2D(_MainTex, curCoorN); 

						_vector = tex2D(_curlNoiseTex, curCoor).rg;
						_vectorN = tex2D(_curlNoiseTex, curCoorN).rg;
					}

					sum /= _iterations * 2 + 1;
				}


				return float4(sum, 1.0);
			}
			ENDCG
		}
	}
}
