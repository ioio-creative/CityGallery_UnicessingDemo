// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Unicessing/Unlit/Stroke"
{
    Properties{
        _MainTex("Texture", 2D) = "white"{}

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
            
           #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;

            struct appdata
            {
                float4 vertex : POSITION;
                fixed3 color : COLOR0;
                float2 uv:TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                fixed3 color : COLOR0;
                float2 uv:TEXCOORD0;
            };

            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 texCol = tex2D(_MainTex, i.uv);
                fixed4 o = fixed4(i.color, 1) * texCol;
                return o;
            }
            ENDCG
        }
    }
}