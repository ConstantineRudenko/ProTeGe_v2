Shader "ProTeGe/Processors/Filters/Normal map"
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
			
			sampler2D _MainTex;
			uint _Size;
			float _Noise;
			float _Intensity;

			fixed4 frag (v2f i) : SV_Target
			{
				//	123
				//	4 6
				//	789

				//	q 2-3
				//	w 1-4

				float offset = 1.0 / _Size;
				float2 xOffset = float2(offset, 0);
				float2 yOffset = float2(0, offset);

				float h1 = tex2D(_MainTex, i.uv - xOffset + yOffset).r;
				float h2 = tex2D(_MainTex, i.uv + yOffset).r;
				float h3 = tex2D(_MainTex, i.uv + xOffset + yOffset).r;
				float h4 = tex2D(_MainTex, i.uv - xOffset).r;
				float h6 = tex2D(_MainTex, i.uv + xOffset).r;
				float h7 = tex2D(_MainTex, i.uv - xOffset - yOffset).r;
				float h8 = tex2D(_MainTex, i.uv - yOffset).r;
				float h9 = tex2D(_MainTex, i.uv + xOffset - yOffset).r;

				float k = 0.3536;	// sqrt(2) * 0.5

				float2 d;

				d.x = k*(h3+h9) + h6 - k*(h1+h7) - h4; 
				d.y = k*(h1+h3) + h2 - k*(h7+h9) - h8;

				d.x +=  randVector2(i.uv).x * _Noise / 8192;
				d.y +=  randVector2(i.uv).y * _Noise / 8192;

				d *= _Intensity * 3;

				float3 nx = float3(offset, offset, d.x);
				float3 ny = float3(offset, -offset, d.y);

				float4 col;
				col.rgb = cross(ny,nx);
				col.a = 1;

				col.rg = float2(dot(col.rg, float2(1, 1)), dot(col.rg, float2(-1, 1)));
				col.rgb = normalize(col.rgb);
				col.rgb = (col.rgb + float3(1,1,1))*0.5;

				return col;
			}
			ENDCG
		}
	}
}
