Shader "Unlit/GhostBody"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
		Blend SrcAlpha OneMinusSrcAlpha

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
			   float2 uv = i.uv - float2(0.5,0.5);
			   fixed4 col = fixed4(1.0, 1., 1., 1.);

			   float shape = length(uv) - 0.5;
			   shape = abs(uv.x - pow(saturate(uv.y)*.5,.75)) - saturate(uv.y + .25)*0.31-sin((uv.y)*25.+_Time.y*3.)*.03;

			   col.w = (1. - saturate(shape*400.))*saturate(uv.y*3.);

                return col;
            }
            ENDCG
        }
    }
}
