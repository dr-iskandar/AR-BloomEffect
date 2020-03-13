// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "UB/PostEffects/Refractions" {
    Properties {
		[HideInInspector]_MainTex ("Base (RGB)", 2D) = "white" {}
		[HideInInspector]_BumpMap("Normalmap", 2D) = "bump" {}
		[HideInInspector]_TileX ("TileX", float) = 1
		[HideInInspector]_TileY ("TileY", float) = 1
		[HideInInspector]_ReflDistort("Reflection distort", float) = 0.05

    }
   
	Subshader{

		Pass{
			Tags{ "Queue" = "Opaque" }

			CGPROGRAM

			#include "UnityCG.cginc"

			#pragma vertex vert
			#pragma fragment frag

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _BumpMap;
			float _ReflDistort;
			float _TileX;
			float _TileY;
			
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct vertexOutput {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			vertexOutput vert(appdata v)
			{
				vertexOutput o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

			half4 frag(vertexOutput i) : SV_Target
			{
				fixed3 nor = UnpackNormal(tex2D(_BumpMap, float2(i.uv.x*_TileX,i.uv.y*_TileY)));
				i.uv += nor *_ReflDistort;
				return tex2D(_MainTex, i.uv);
			}
			ENDCG
		}
	}
}