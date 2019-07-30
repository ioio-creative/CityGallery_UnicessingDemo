Shader "Custom/Unicessing/Unlit/DrawPolygonUnlit"
{
	Properties
	{
		[HideInInspector] _MainTex ("Texture", 2D) = "white" {}

		_N("N", Int) = 4
		_Size ("Size", Range(0, 1)) = 0.25
		_W("Width", Range(0, 1.0)) = 0.05
		_Rotation ("Rotation", Range(0, 360)) = 0

		[Space(12)]
		[Toggle(SHOW_BACKGROUND)] B("Show Background", Float) = 1
		_FillColor("Fill Color", Color) = (1,1,1,1)
		_EdgeColor("Edge Color", Color) = (0,0,0,1)
		_BgColor("BG Color", Color) = (0,0,1,1)

		
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			#define PI 3.14159265358979
			int _N; // 頂点数
			fixed _Size; // 多角形の大きさ
			fixed _W; // 線の太さ
			float _Rotation; // 回転

			fixed4 _FillColor; // 塗りつぶしの色
			fixed4 _EdgeColor; // 線の色
			fixed4 _BgColor; // 背景の色
			
            #pragma shader_feature SHOW_BACKGROUND

			fixed4 frag (v2f i) : SV_Target
			{
				float2 uv = i.uv - 0.5;
				float r = length(uv); // 原点からの距離
				float theta = atan2(uv.x, uv.y) + _Rotation + 2.0 * PI; // 原点からの角度 (負にならないように2π足しています)
				theta = theta % (2.0 * PI / _N);  // 角度の補正

				float r2 = cos(PI / _N) / cos (PI / _N - theta); // 中心から多角形までの距離
				float step1 = step(r, r2 * _Size);
				float step2 = step(r, r2 * (_Size - _W));

				#if SHOW_BACKGROUND // 背景表示
					float step3 = step(r2 * _Size, r);
					return step1 * _EdgeColor + step2 * (_FillColor - _EdgeColor) + step3 * _BgColor;
				#else // 背景非表示
					clip(step1-0.001);
					return step1 * _EdgeColor + step2 * (_FillColor - _EdgeColor);
				#endif
			}
			ENDCG
		}
	}
}
