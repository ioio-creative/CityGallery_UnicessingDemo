// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Unicessing/Unlit/FillUnlit"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex ("Texture", 2D) = "white" {}

		[Enum(UnityEngine.Rendering.CullMode)] _Cull("Cull", Float) = 2                // Back
		[Enum(UnityEngine.Rendering.CompareFunction)] _ZTest("ZTest", Float) = 4       // LEqual
		[Enum(Off, 0, On, 1)] _ZWrite("ZWrite", Float) = 1			                   // On
		[Enum(UnityEngine.Rendering.BlendMode)] _SrcFactor("Src Factor", Float) = 5    // SrcAlpha
		[Enum(UnityEngine.Rendering.BlendMode)] _DstFactor("Dst Factor", Float) = 10   // OneMinusSrcAlpha
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }

		Cull[_Cull]
		ZTest[_ZTest]
		ZWrite[_ZWrite]
		Blend[_SrcFactor][_DstFactor]

		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
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
			fixed4 _Color;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv) * _Color;
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
