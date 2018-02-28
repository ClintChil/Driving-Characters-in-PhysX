Shader "Custom/FlatLitSurface" {
	Properties {
		_MainTex ("Main texture", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		 
		CGPROGRAM
		#pragma surface surf Flat fullforwardshadows finalcolor:FlatColour

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		fixed _ShadowClip;
		half3 _ShadowCol;

		half4 LightingFlat (SurfaceOutput s, half3 lightDir, half atten) 
		{
	        half NdotL = saturate(dot(s.Normal, lightDir));
			half4 c;
			c.rgb = _LightColor0.rgb * atten * NdotL;
			c.a = 1.0f;
	        return c;
    	}

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		void FlatColour(Input IN, SurfaceOutput o, inout fixed4 color)
		{
			half cutoff = saturate(step(_ShadowClip, color.g));
			half3 ramp = cutoff + _ShadowCol.rgb;

			half3 lit = o.Albedo *_LightColor0.rgb;
			half3 unlit = o.Albedo * ramp *saturate(_LightColor0.rgb) * ramp;

			half4 c;
			c.rgb = lerp(unlit, lit, cutoff);
			c.a = o.Alpha;

			fixed4 newCol;
#ifdef UNITY_PASS_FORWARDADD
			color.rgb = (cutoff * _LightColor0.rgb) * 0.1f;
#else
			color.rgb = c.rgb;
#endif
		}

		void surf (Input IN, inout SurfaceOutput o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = _Color;
			o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb * c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
