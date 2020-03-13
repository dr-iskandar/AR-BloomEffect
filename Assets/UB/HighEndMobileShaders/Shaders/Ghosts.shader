Shader "UB/Ghosts" {
	Properties{
		_Color("Main Color", Color) = (1,1,1,1)
		_MainTex("Base (A)", 2D) = "white" {}
		_BumpMap ("Normalmap", 2D) = "bump" {}
		_ReflAmount ("Reflection amount", float) = 0.5
        _ReflDistort ("Reflection distort", float) = 0.25
		[HideInInspector]_ReflectionTex("Reflection", 2D) = "white" { }
	}

	SubShader{

		Tags{
			"RenderType" = "Transparent"
			"IgnoreProjector" = "True"
			"Queue" = "Transparent"
		}

		CGPROGRAM

		#pragma surface surf Lambert alpha

		struct Input {
			float2 uv_MainTex;
			float2 uv_BumpMap;
			float4 screenPos;
		};

		uniform fixed4 _Color;
		uniform sampler2D _MainTex;
		uniform sampler2D _BumpMap;
		uniform float _ReflAmount;
		uniform float _ReflDistort;
		uniform sampler2D _ReflectionTex;

		void surf(Input IN, inout SurfaceOutput o) {
			fixed2 screenUV = (IN.screenPos.xy) / (IN.screenPos.w);
    
		    float4 tex = tex2D(_MainTex, IN.uv_MainTex);
		    fixed3 nor = UnpackNormal (tex2D(_BumpMap, IN.uv_BumpMap));

		    screenUV.xy += nor *_ReflDistort;

		    float4 refl = tex2D(_ReflectionTex, screenUV);

		 	o.Albedo = tex.rgb*(1-_ReflAmount);
		    o.Emission = refl * _Color *_ReflAmount;
		    o.Normal = nor.rgb;
		    o.Alpha = tex.a;
		}
		ENDCG
	}

	FallBack "Diffuse"
}

