Shader "opPostFX/TreeShadow"
{
    Properties
    {
		_MainTex("Texture", 2D) = "white" {}
		_ShadowTex("Shadow texture", 2D) = "white" {}
        _Intensity("Intensity", Range(0,1)) = 0.85
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
			float4 _MainTex_TexelSize;

			float4 _CameraPos;
			sampler2D _ShadowTex;
			float _Intensity;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }
#define mod(x, y) (x - y * floor(x / y))
			float drawShadow(float2 uv)
			{
				float totShadow = 0.0f;

				float rep = 1.;// float2(5., 5.);
				float off = 10.;
				float x = abs(uv.x) + rep * 0.5;
				float index = floor(x / rep);
				uv.y += sin(index)*0.25-.95;
				uv.x = fmod(x, rep) - rep * 0.5;
				//uv.y = clamp(uv.y, -1.0, 1.0);
				float shadow = 1. - tex2D(_ShadowTex, mul(uv, r2d(index)) - float2(-0.5, -0.5)).x;
				totShadow += shadow;
				/*
				int cnt = 5;
				float fcnt = float(cnt);
				for (int i = 0; i < cnt; ++i)
				{
					float f = ((float(i) / fcnt)-0.5f)*2.0f; // -1 to 1
					float sepDist = 1.0f;
					float2 nuv = uv;
					nuv = uv + float2(sepDist*f, 0.0f);// +float2(0.5, 0.5);
					nuv = mul(r2d(f*3.), nuv*.5);
					float shadow = 1.-tex2D(_ShadowTex, nuv).x;
					totShadow += shadow/fcnt;
				}
				*/
				return saturate(totShadow);
			}

            fixed4 frag (v2f i) : SV_Target
            {
                float3 prevCol = tex2D(_MainTex, i.uv).xyz;   

                fixed2 hlf2 = fixed2(0.5,0.5);
				//fixed2 ouv = .5*(i.uv*4. + *.25+float2(0.,-.5))*_MainTex_TexelSize.zw / _MainTex_TexelSize.zz;

				float shadow = drawShadow(1.5*(i.uv*_MainTex_TexelSize.zw / _MainTex_TexelSize.zz) + float2(_CameraPos.z, 0.) *.08);

                float3 col = lerp(prevCol, prevCol * _Intensity, pow(shadow, .8));

                return fixed4(col, 1.0);
            }
            ENDCG
        }
    }
}
