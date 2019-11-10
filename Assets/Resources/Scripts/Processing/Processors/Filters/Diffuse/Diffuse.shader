Shader "ProTeGe/Processors/Filters/Diffuse"
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

			sampler2D _MainTex;
			float _Range, _step;
			static const float PI = 3.14159265;

			float4 frag (v2f i) : SV_Target
			{
				float _range = lerp(0,_Range,rand2D(i.uv)*_Range);
				float _angle = lerp(0,PI,rand2D(i.uv + float2(0.5,0.5)));

				return tex2D(_MainTex, i.uv + _range*_step*float2(cos(_angle),sin(_angle)));
			}
			ENDCG
		}
	}
}

