Shader "opPostFX/WhiteBalance"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Temperature("Temperature", Range(-100, 100)) = 0
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
            float _Temperature;

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

                fixed3 leftCol = fixed3(255, 185, 95)/255.;
                if (sign(_Temperature) < 0.)
                    leftCol = fixed3(0, 181, 255)/255.;

                float3 col = lerp(prevCol, leftCol* prevCol, abs(_Temperature)/100.);

                float g = 1. / 1.2;
                col = pow(col, fixed3(g, g, g));

                return fixed4(col, 1.0);
            }
            ENDCG
        }
    }
}
