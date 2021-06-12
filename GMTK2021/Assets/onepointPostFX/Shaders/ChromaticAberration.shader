Shader "opPostFX/ChromaticAberration"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _ChromaticStrength("Chromatic strength", Range(0,0.05)) = 0.005
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
            float _ChromaticStrength;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed2 dir = normalize(i.uv - fixed2(0.5, 0.5));

                col.r = tex2D(_MainTex, i.uv+dir* _ChromaticStrength).r;
                col.g = tex2D(_MainTex, i.uv).g;
                col.b = tex2D(_MainTex, i.uv - dir * _ChromaticStrength).b;

                return col;
            }
            ENDCG
        }
    }
}
