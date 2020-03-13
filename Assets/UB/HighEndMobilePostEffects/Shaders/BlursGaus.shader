// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "UB/PostEffects/BlursGaus"
{
	Properties
	{
		[HideInInspector]_CustomFloatParam1("Iteration", float) = 1
		[HideInInspector]_CustomFloatParam2("Neighbour", float) = 1
		[HideInInspector]_MainTex("Base (RGB)", 2D) = "" {}
	}

	SubShader
	{
		Pass
		{
			ZTest Always 
			Cull Off
			Fog{ Mode off }

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

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _CustomFloatParam1;
			float _CustomFloatParam2;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

			fixed4 frag(v2f i) : COLOR
			{
				float stepX = (1.0 / (_ScreenParams.xy / _ScreenParams.w).x)*(_CustomFloatParam2 + (_CustomFloatParam1 % 2));
				float stepY = (1.0 / (_ScreenParams.xy / _ScreenParams.w).y)*(_CustomFloatParam2 + (_CustomFloatParam1 % 2));

				half4 color = float4 (0,0,0,0);

				fixed2 copyUV;

				copyUV.x = i.uv.x - stepX;
				copyUV.y = i.uv.y - stepY;
				color += tex2D (_MainTex, copyUV)*0.077847;
				copyUV.x = i.uv.x;
				copyUV.y = i.uv.y - stepY;
				color += tex2D (_MainTex, copyUV)*0.123317;
				copyUV.x = i.uv.x + stepX;
				copyUV.y = i.uv.y - stepY;
				color += tex2D (_MainTex, copyUV)*0.077847;

				copyUV.x = i.uv.x - stepX;
				copyUV.y = i.uv.y;
				color += tex2D (_MainTex, copyUV)*0.123317;
				copyUV.x = i.uv.x;
				copyUV.y = i.uv.y;
				color += tex2D (_MainTex, copyUV)*0.195346;
				copyUV.x = i.uv.x + stepX;
				copyUV.y = i.uv.y;
				color += tex2D (_MainTex, copyUV)*0.123317;

				copyUV.x = i.uv.x - stepX;
				copyUV.y = i.uv.y + stepY;
				color += tex2D (_MainTex, copyUV)*0.077847;
				copyUV.x = i.uv.x;
				copyUV.y = i.uv.y + stepY;
				color += tex2D (_MainTex, copyUV)*0.123317;
				copyUV.x = i.uv.x + stepX;
				copyUV.y = i.uv.y + stepY;
				color += tex2D (_MainTex, copyUV)*0.077847;


		//		//5*5 gaus
		//		fixed2 copyUV;
		//
		//		copyUV.x = i.uv.x + stepX*-2;
		//		copyUV.y = i.uv.y + stepY*-2;
		//		color += tex2D (_MainTex, copyUV)*0.0036630036; // 1/273
		//		copyUV.x = i.uv.x - stepX; //*-1
		//		copyUV.y = i.uv.y + stepY*-2;
		//		color += tex2D (_MainTex, copyUV)*0.0146520146; // 4/273
		//		copyUV.x = i.uv.x; // + stepX*0;
		//		copyUV.y = i.uv.y + stepY*-2;
		//		color += tex2D (_MainTex, copyUV)*0.0256410256; // 7/273
		//		copyUV.x = i.uv.x + stepX; //*1
		//		copyUV.y = i.uv.y + stepY*-2;
		//		color += tex2D (_MainTex, copyUV)*0.0146520146; // 4/273
		//		copyUV.x = i.uv.x + stepX*2;
		//		copyUV.y = i.uv.y + stepY*-2;
		//		color += tex2D (_MainTex, copyUV)*0.0036630036; // 1/273
		//
		//		copyUV.x = i.uv.x + stepX*-2;
		//		copyUV.y = i.uv.y - stepY; //*-1;
		//		color += tex2D (_MainTex, copyUV)*0.0146520146; // 4/273
		//		copyUV.x = i.uv.x - stepX; //*-1;
		//		copyUV.y = i.uv.y - stepY; //*-1;
		//		color += tex2D (_MainTex, copyUV)*0.0586080586; // 16/273
		//		copyUV.x = i.uv.x; // + stepX*0;
		//		copyUV.y = i.uv.y - stepY; //*-1;
		//		color += tex2D (_MainTex, copyUV)*0.0952380952; // 26/273
		//		copyUV.x = i.uv.x + stepX; //*1;
		//		copyUV.y = i.uv.y - stepY; //*-1;
		//		color += tex2D (_MainTex, copyUV)*0.0586080586; // 16/273
		//		copyUV.x = i.uv.x + stepX*2;
		//		copyUV.y = i.uv.y - stepY; //*-1;
		//		color += tex2D (_MainTex, copyUV)*0.0146520146; // 4/273
		//
		//		copyUV.x = i.uv.x + stepX*-2;
		//		copyUV.y = i.uv.y; // + stepY*0;
		//		color += tex2D (_MainTex, copyUV)*0.0256410256; // 7/273
		//		copyUV.x = i.uv.x - stepX; //*-1;
		//		copyUV.y = i.uv.y; // + stepY*0;
		//		color += tex2D (_MainTex, copyUV)*0.0952380952; // 26/273
		//		copyUV.x = i.uv.x; // + stepX*0;
		//		copyUV.y = i.uv.y; // + stepY*0;
		//		color += tex2D (_MainTex, copyUV)*0.1501831501; // 41/273
		//		copyUV.x = i.uv.x + stepX; //*1;
		//		copyUV.y = i.uv.y; // + stepY*0;
		//		color += tex2D (_MainTex, copyUV)*0.0952380952; // 26/273
		//		copyUV.x = i.uv.x + stepX*2;
		//		copyUV.y = i.uv.y; // + stepY*0;
		//		color += tex2D (_MainTex, copyUV)*0.0256410256; // 7/273
		//
		//		copyUV.x = i.uv.x + stepX*-2;
		//		copyUV.y = i.uv.y + stepY;
		//		color += tex2D (_MainTex, copyUV)*0.0146520146; // 4/273
		//		copyUV.x = i.uv.x - stepX; //*-1;
		//		copyUV.y = i.uv.y + stepY;
		//		color += tex2D (_MainTex, copyUV)*0.0586080586; // 16/273
		//		copyUV.x = i.uv.x; // + stepX*0;
		//		copyUV.y = i.uv.y + stepY;
		//		color += tex2D (_MainTex, copyUV)*0.0952380952; // 26/273
		//		copyUV.x = i.uv.x + stepX; //*1;
		//		copyUV.y = i.uv.y + stepY;
		//		color += tex2D (_MainTex, copyUV)*0.0586080586; // 16/273
		//		copyUV.x = i.uv.x + stepX*2;
		//		copyUV.y = i.uv.y + stepY;
		//		color += tex2D (_MainTex, copyUV)*0.0146520146; // 4/273
		//
		//		copyUV.x = i.uv.x + stepX*-2;
		//		copyUV.y = i.uv.y + stepY*2;
		//		color += tex2D (_MainTex, copyUV)*0.0036630036; // 1/273
		//		copyUV.x = i.uv.x - stepX; //*-1;
		//		copyUV.y = i.uv.y + stepY*2;
		//		color += tex2D (_MainTex, copyUV)*0.0146520146; // 4/273
		//		copyUV.x = i.uv.x; // + stepX*0;
		//		copyUV.y = i.uv.y + stepY*2;
		//		color += tex2D (_MainTex, copyUV)*0.0256410256; // 7/273
		//		copyUV.x = i.uv.x + stepX; //*1;
		//		copyUV.y = i.uv.y + stepY*2;
		//		color += tex2D (_MainTex, copyUV)*0.0146520146; // 4/273
		//		copyUV.x = i.uv.x + stepX*2;
		//		copyUV.y = i.uv.y + stepY*2;
		//		color += tex2D (_MainTex, copyUV)*0.0036630036; // 1/273

				return color;
			}

			ENDCG
		}
	}
}

