// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/FlatLit"
{

	Properties
	{
		_Color ("Main colour", Color) = (1,1,1,1)
	}
		
	CGINCLUDE
	#include "UnityCG.cginc"
	#include "AutoLight.cginc"
		
	struct v2f
	{
		float4 pos: SV_POSITION;
		LIGHTING_COORDS(0,1)
	};

	fixed4 _Color;

	v2f vert(appdata_full v)
	{
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		TRANSFER_VERTEX_TO_FRAGMENT(o);
		return o;
	}

	fixed4 frag(v2f i) : SV_Target
	{
		fixed4 col = _Color;
		fixed4 light = LIGHT_ATTENUATION(i);
		return light;
		return col;
	}

	ENDCG
		
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			Tags { "LightMode"="ForwardBase" }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma multi_compile_fwdbase
			ENDCG
		}
	}
	Fallback "VertexLit"
}
