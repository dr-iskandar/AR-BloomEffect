
Shader "UB/Waters" {
    Properties {
		_WaterSpeed ("Water Speed", Float) = 0.2
		_ReflectionDistortion ("Reflection Distortion", Float) = 1
		//_ReflectionCorrection ("Reflection Correction", Float) = 0
		_FakeDepth ("FakeDepth", Float) = 0.05
		_Emission ("Emission", Float) = 0.3
		_Alpha ("Alpha", Float) = 0.5
		_MainTex ("WaveTexture", 2D) = "white" {}
		_DepthTex ("DepthTexture", 2D) = "white" {}
		[HideInInspector]_ReflectionTex ("Reflection", 2D) = "white" { }
    }

    SubShader {

    	Tags { 
			"Queue"="Transparent"  
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
		}
		LOD 100

		CGPROGRAM

		#pragma surface surf Lambert alpha 

		#include "UnityCG.cginc"

		struct Input {
			float2 uv_MainTex;
			float2 uv_DepthTex;
			float4 screenPos;
		};


		sampler2D _MainTex;
		sampler2D _DepthTex;

		uniform float _WaterSpeed;
		uniform float _ReflectionDistortion;
		uniform float _FakeDepth;
		uniform float _Emission;
		uniform float _Alpha;
        uniform sampler2D _ReflectionTex;

		void surf (Input IN, inout SurfaceOutput o) 
		{
			float stepX = (1.0 / (_ScreenParams.xy / _ScreenParams.w).x);

			fixed2 screenUV = (IN.screenPos.xy) / (IN.screenPos.w);

            float waveslide = _WaterSpeed*_Time.x;

			half3 col_orig = tex2D(_MainTex, float2(IN.uv_MainTex.x,IN.uv_MainTex.y+waveslide)).rgb*_FakeDepth;

			half3 col1 = tex2D(_DepthTex, float2(IN.uv_MainTex.x+col_orig.b,IN.uv_MainTex.y+col_orig.g)).rgb;


			screenUV = float2(screenUV.x+col_orig.b*(1/IN.screenPos.z)*_ReflectionDistortion/5,
				screenUV.y+col_orig.g*(1/IN.screenPos.z)*_ReflectionDistortion);
			fixed4 refl = tex2D(_ReflectionTex, screenUV);

			half3 norm1 = UnpackNormal (tex2D (_DepthTex, IN.uv_DepthTex));

			half3 col = col1+col1+float3(0,0,-0.07);
			o.Albedo = col;
			o.Alpha = _Alpha; 
		
			o.Emission = _Emission*refl;
			o.Normal = norm1;
		}

		ENDCG

	}
	Fallback "Diffuse"
}