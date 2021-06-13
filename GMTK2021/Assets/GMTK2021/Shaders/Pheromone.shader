Shader "Unlit/Pheromone"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
		Blend SrcColor OneMinusSrcColor

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

			float _OffsetTime;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

			float _cir(float2 uv, float r)
			{
				return length(uv) - r;
			}

			float3 rdr(float2 uv)
			{
				float2 ouv = uv;
				float3 col = float3(1.,1.,1.)*0.;// tex2D(_MainTex, uv).xyz;
				uv = uv * 5. + float2(-0.1, -1.5);


				float shp = 300.;

				float2 uvf = uv;
				uvf.y = clamp((uvf.y + .3)*.5, -1., 0.);
				float dist = 0.5f;
				int cnt = 3;
				float fcnt = float(cnt);
				float3 baseWorldPos = unity_ObjectToWorld._m30_m31_m32;
				uv -= float2(-0.2, -.5);
				for (int i = 0; i < 7; ++i)
				{
					float fi = float(i);
					float2 p = uv + float2(sin(fi*10. + _Time.y+ _OffsetTime)*.7, fmod(_OffsetTime +fi + _Time.y+.2*_Time.y*(fi + 1.), 2.));
					float shape = _cir(p, 2.*lerp(.05, .1, saturate(fi / 7.)));
					col += .4*saturate(float3(0.878 + (cos(fi)*.5 + .5)*.5, 0.584 + (sin(fi)*.5 + .5)*.5, 0.933))*(1. - saturate(shape*10.));// *(1. - saturate(abs((uv.y + .75)*2.)));
				}
				col *= saturate(1.-abs(ouv.y*5.));
				return col;
			}


            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
				fixed4 col = fixed4(0.,0.,0.,1.);
				float2 uv = i.uv - float2(0.5, 0.5);

				col.xyz = rdr(uv*.5)*2.;
				col.w = length(col.xyz);

				//col.xyz = float3(1., 1., 1.);
				//col.w = float3(1., 1., 1.)*(1. - saturate((length(uv) - .5)*400.));

               // UNITY_APPLY_FOG(i.fogCoord, col);


                return col;
            }
            ENDCG
        }
    }
}
