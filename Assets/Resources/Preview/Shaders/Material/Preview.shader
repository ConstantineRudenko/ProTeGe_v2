Shader "ProTeGe/Preview" {
	Properties {
		_Parallax("                          Parallax", Range(0,1)) = 0.5
		_MainTex("                          Texture", 2D) = "white" {}
		_BumpTex("                         Normal map", 2D) = "bump"  {}
		_SmoothnessTex("                         Smoothness", 2D) = "white" {}
		_MetallicTex("                         Metallicity", 2D) = "white" {}
		_OcclusionTex("                          Occlusionn", 2D) = "white" {}
		_ParallaxTex("                         Parallax map", 2D) = "black"  {}
		_EmissionTex("                          Emission", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 4.0

		sampler2D _MainTex;
		sampler2D _BumpTex;
		sampler2D _SmoothnessTex;
		sampler2D _MetallicTex;
		sampler2D _OcclusionTex;
		sampler2D _EmissionTex;
		sampler2D _ParallaxTex;

		float _Parallax;

		struct Input {
			float2 uv_MainTex;
			float2 uv_MetallicTex;
			float2 uv_BumpTex;
			float2 uv_EmissionTex;
			float2 uv_OcclusionTex;
			float2 uv_SmoothnessTex;
			float2 uv_ParallaxTex;
			float3 viewDir;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {

				#if UNITY_UV_STARTS_AT_TOP

			IN.uv_MainTex.y = 1.0 - IN.uv_MainTex.y;
			IN.uv_MetallicTex.y = 1.0 - IN.uv_MetallicTex.y;
			IN.uv_BumpTex.y = 1.0 - IN.uv_BumpTex.y;
			IN.uv_EmissionTex.y = 1.0 - IN.uv_EmissionTex.y;
			IN.uv_OcclusionTex.y = 1.0 - IN.uv_OcclusionTex.y;
			IN.uv_SmoothnessTex.y = 1.0 - IN.uv_SmoothnessTex.y;
			IN.uv_ParallaxTex.y = 1.0 - IN.uv_ParallaxTex.y;

				#endif

			//begin parallax

			const int ParallaxSamples = 36;
			const int ParallaxSecondarySamples = 0;
			const float parallax_distance = 0.2;

			float2 p = normalize(IN.viewDir).xy;
			p.y *= -1;

			p *= _Parallax * parallax_distance;

			float2 dp = p / ParallaxSamples;
			float h = 1;
			float dh = h / ParallaxSamples;
			float prev_this_h = h;

			for (int i = 0; i < ParallaxSamples; i++)
			{
				float this_h = tex2D(_ParallaxTex, IN.uv_ParallaxTex + p).r;

				if (this_h >= h) {
					h += dh;
					p += dp;

					float delta1 = this_h - h;
					float delta2 = h + dh - prev_this_h;
					float ratio = delta1 / (delta1 + delta2);
					p += ratio * dp;

					break;
				}
				else
				{
					prev_this_h = this_h;
					h -= dh;
					p -= dp;
				}
			}

			IN.uv_MainTex += p;
			IN.uv_BumpTex += p;
			IN.uv_MetallicTex += p;
			IN.uv_EmissionTex += p;
			IN.uv_ParallaxTex += p;
			IN.uv_OcclusionTex += p;

			//end parallax

			o.Albedo = pow(tex2D(_MainTex, IN.uv_MainTex),2.2);

			o.Smoothness = tex2D(_SmoothnessTex, IN.uv_SmoothnessTex).r;
			o.Normal = pow(tex2D(_BumpTex, IN.uv_BumpTex),0.45) * 2.0 - float3(1,1,1);

			o.Occlusion = tex2D(_OcclusionTex, IN.uv_OcclusionTex).r;
			o.Metallic = tex2D(_MetallicTex, IN.uv_MetallicTex).r;
			o.Emission.rgb = tex2D(_EmissionTex, IN.uv_EmissionTex).rgb;
		} 
		ENDCG
	}
	FallBack "Diffuse"
}
