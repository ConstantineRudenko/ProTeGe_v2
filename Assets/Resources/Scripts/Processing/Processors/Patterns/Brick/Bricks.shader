Shader "ProTeGe/Processors/Pattern/Bricks"
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
			float _Frequency_H;
			float _Frequency_V;
			float _Flatness;
			float _Size;

			float dist(float f, float freq)
			{
				float start = int(f * freq);
				start = start / freq;
				float end = start + 1.0 / freq;
				float mid = (end + start) * 0.5;
				return abs(f - mid) / (end - start) * 2;
			}

			float4 frag (v2f i) : SV_Target
			{
				float4 col = tex2D(_MainTex, i.uv);

				_Frequency_H = int(_Frequency_H*20)+1;
				_Frequency_V = int(_Frequency_V*10)*2+2;

				_Size = lerp(1, _Size, min(1.0 / _Frequency_V, 1.0 / _Frequency_H));
				_Flatness = lerp(1, _Flatness, min(1.0 / _Frequency_V, 1.0 / _Frequency_H));

				_Size = 1.0 - saturate(_Size);
				_Flatness = 1.0 - saturate(_Flatness * 0.999);

				float x = i.uv.x + 0.25/_Frequency_V;
				float y = i.uv.y;//

				float dist_v = dist(y, _Frequency_V);

				float dist_h = uint(y * _Frequency_V + 0.5) % 2 == 1 ?
					dist(x + 0.5 / _Frequency_H, _Frequency_H) :
					dist(x, _Frequency_H);

				float r_v = dist_v / _Frequency_V;
				r_v = saturate(r_v - _Size) / (1.0 - _Size);
				r_v = saturate(r_v / _Flatness);

				float r_h = dist_h / _Frequency_H;
				r_h = saturate(r_h - _Size) / (1.0 - _Size);
				r_h = saturate(r_h / _Flatness);

				float r = min(r_h, r_v);

				return float4(r,r,r,1);
			}
			ENDCG
		}
	}
}
