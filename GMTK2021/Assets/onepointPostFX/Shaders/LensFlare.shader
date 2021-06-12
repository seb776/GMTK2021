Shader "opPostFX/LensFlare"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
		_Intensity("Intensity", Range(0,1)) = 0.15
		_Power("Power", Range(0.1,5)) = 1.
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
            float4 _Color;
			float _Intensity;
			float _Power;
			float _Smoothness;
            float _Radius;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

			fixed3 postFx2(fixed2 uv, fixed2 ouv, float sz, float id)
			{
				fixed3 col = fixed3(0.,0.,0.);// = texture(iChannel0, uv).xyz;

				float c = abs(length(ouv) - .3 - id * sz*8. - sz * 9.) - sz;
				fixed3 rgb;
				float a = atan(ouv.y/ouv.x)*1.;
				float cnt = 16.;
				for (float i = 0.; i < cnt; ++i)
				{
					fixed2 coords = fixed2(.5, .5) + (mul((uv - fixed2(.5, .5))*(i + 1.)*0.01,r2d((i - cnt / 2.)*.025)) *-1.*sat(length(ouv*2.)));
					rgb += fixed3(1.,1.,1.)*
						(saturate((sin(a*400.*sin(a)) + sin(a*133.) + sin(a*100.))*.2 + .5)*.1 + .5)*
						tex2D(_MainTex, coords).x;
				}
				float pw = _Power;
				col += pow((1. - sat(c*5.))*pow(rgb / cnt, fixed3(1.,1.,1.)), fixed3(pw,pw,pw));

				return col * _Intensity;
			}

            fixed4 frag (v2f i) : SV_Target
            {
				fixed3 col = tex2D(_MainTex, i.uv).xyz;

				fixed2 hlf2 = fixed2(0.5, 0.5);
				fixed2 ouv = (i.uv - hlf2)*_MainTex_TexelSize.zw / _MainTex_TexelSize.zz;
				col += postFx2(i.uv, ouv, 0.01, 0.)*fixed3(1., 0., 0.);
				col += postFx2(i.uv, ouv, 0.01, -1.)*fixed3(0., 1., 0.);
				col += postFx2(i.uv, ouv, 0.01, -2.)*fixed3(0., 0., 1.);

                return fixed4(col, 1.0);
            }
            ENDCG
        }
    }
}
