Shader "opPostFX/VignetteShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Color("Color", Color) = (0,0,0,0)
        _Intensity("Intensity", Range(0,1)) = 0.85
        _Smoothness("Smoothness", Range(0,1)) = 0.5
        _Radius("Radius", Range(0,1)) = 0.5
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

            #include "UnityCG.cginc"
            #include "OPCommon.cginc"

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
            float4 _Color;
            float _Intensity;
            float _Smoothness;
            float _Radius;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }
            fixed4 frag (v2f i) : SV_Target
            {
                float3 prevCol = tex2D(_MainTex, i.uv).xyz;

                fixed2 center = fixed2(0.5,0.5);
                
                float d = length(center - i.uv) - _Radius;

                float3 vignetted = lerp(_Color.xyz, prevCol,  (1.-sat(d*_Smoothness*40.0)));

                float3 col = lerp(prevCol, vignetted, _Intensity);

                return fixed4(col, 1.0);
            }
            ENDCG
        }
    }
}
