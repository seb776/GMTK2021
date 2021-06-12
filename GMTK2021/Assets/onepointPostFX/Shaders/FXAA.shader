Shader "opPostFX/FXAA"
{
        Properties
        {
            _MainTex("Texture", 2D) = "white" {}
        }

        CGINCLUDE
        #include "UnityCG.cginc"

        texture2D _MainTex;
        float4 _MainTex_TexelSize;

        struct VertexData {
            float4 vertex : POSITION;
            float2 uv : TEXCOORD0;
        };

        struct Interpolators {
            float4 pos : SV_POSITION;
            float2 uv : TEXCOORD0;
        };

        Interpolators VertexProgram(VertexData v) {
            Interpolators i;
            i.pos = UnityObjectToClipPos(v.vertex);
            i.uv = v.uv;
            return i;
        }


        ENDCG

        SubShader
        {
            Cull Off
            ZTest Always
            ZWrite Off

            Pass
            {
                CGPROGRAM
                    #pragma vertex VertexProgram
                    #pragma fragment FragmentProgram
                    #define FXAA_SPAN_MAX 8.0
                    #define FXAA_REDUCE_MUL   (1.0/FXAA_SPAN_MAX)
                    #define FXAA_REDUCE_MIN   (1.0/128.0)
                    #define FXAA_SUBPIX_SHIFT (1.0/4.0)

                SamplerState sampler_MainTex
                {
                };

                half3 FXAAShaderFunction(half4 uv, texture2D tex, half2 rcpFrame)
                {   
                    half3 rgbNW = tex.SampleLevel(sampler_MainTex, half2(uv.z, uv.w), 0.0).xyz;
                    half3 rgbNE = tex.SampleLevel(sampler_MainTex, half2(uv.z, uv.w) + half2(1,0) * rcpFrame.xy, 0.0).xyz;
                    half3 rgbSW = tex.SampleLevel(sampler_MainTex, half2(uv.z, uv.w) + half2(0, 1) * rcpFrame.xy, 0.0).xyz;
                    half3 rgbSE = tex.SampleLevel(sampler_MainTex, half2(uv.z, uv.w) + half2(1, 1) * rcpFrame.xy, 0.0).xyz;
                    half3 rgbM = tex.SampleLevel(sampler_MainTex, half2(uv.x, uv.y), 0.0).xyz;

                    half3 luma = half3(0.299, 0.587, 0.114);
                    float lumaNW = dot(rgbNW, luma);
                    float lumaNE = dot(rgbNE, luma);
                    float lumaSW = dot(rgbSW, luma);
                    float lumaSE = dot(rgbSE, luma);
                    float lumaM = dot(rgbM, luma);

                    float lumaMin = min(lumaM, min(min(lumaNW, lumaNE), min(lumaSW, lumaSE)));
                    float lumaMax = max(lumaM, max(max(lumaNW, lumaNE), max(lumaSW, lumaSE)));

                    half2 dir;
                    dir.x = -((lumaNW + lumaNE) - (lumaSW + lumaSE));
                    dir.y = ((lumaNW + lumaSW) - (lumaNE + lumaSE));

                    float dirReduce = max(
                        (lumaNW + lumaNE + lumaSW + lumaSE) * (0.25 * FXAA_REDUCE_MUL),
                        FXAA_REDUCE_MIN);
                    float rcpDirMin = 1.0 / (min(abs(dir.x), abs(dir.y)) + dirReduce);

                    dir = min(half2(FXAA_SPAN_MAX, FXAA_SPAN_MAX), max(half2(-FXAA_SPAN_MAX, -FXAA_SPAN_MAX), dir * rcpDirMin)) * rcpFrame.xy;

                    half3 rgbA = (1.0 / 2.0) * (
                        tex.SampleLevel(sampler_MainTex, half2(uv.x, uv.y) + dir * (1.0 / 3.0 - 0.5), 0.0).xyz +
                        tex.SampleLevel(sampler_MainTex, half2(uv.x, uv.y) + dir * (2.0 / 3.0 - 0.5), 0.0).xyz);

                    half3 rgbB = rgbA * (1.0 / 2.0) + (1.0 / 4.0) * (
                        tex.SampleLevel(sampler_MainTex, half2(uv.x, uv.y) + dir * (0.0 / 3.0 - 0.5), 0.0).xyz +
                        tex.SampleLevel(sampler_MainTex, half2(uv.x, uv.y) + dir * (3.0 / 3.0 - 0.5), 0.0).xyz);

                    float lumaB = dot(rgbB, luma);

                    if ((lumaB < lumaMin) || (lumaB > lumaMax))
                        return rgbA;

                    return rgbB;
                }

                half4 FragmentProgram(Interpolators i) : SV_Target{
                    half2 uv = i.uv;
                    half2 rcpFrame = half2(1.0, 1.0) / half2(_MainTex_TexelSize.z, _MainTex_TexelSize.w);

                    half4 uv4 = half4(uv.x, uv.y, uv - (rcpFrame * (0.5 + FXAA_SUBPIX_SHIFT)));
                    half3 col = FXAAShaderFunction(uv4, _MainTex, rcpFrame);
                    return half4(col, 1.0);
                }
                ENDCG
            }
        }
}
