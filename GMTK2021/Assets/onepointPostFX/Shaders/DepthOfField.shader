// Code inspiré de https://www.shadertoy.com/view/lstBDl

Shader "opPostFX/DepthOfField"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Max_Blur_Size("Max Blur", Range(1, 60)) = 20.0
        _Rad_Scale("Rad Scale", Range(0, 1)) = 0.5
        _uFar("Far Plane", Range(0, 3000)) = 10
        _Focus_Point("Focus Point", Range(0, 180)) = 78.0
        _Focus_Scale("Focus Scale", Range(0, 180)) = 50.0
    }

    CGINCLUDE
        #include "UnityCG.cginc"


        sampler2D _MainTex;
        sampler2D _CameraDepthTexture;
        float4 _MainTex_TexelSize;

        float _Max_Blur_Size, _Rad_Scale, _uFar, _Focus_Point, _Focus_Scale;

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

        SamplerState sampler_CameraDepthTexture
        {

        };
        ENDCG

        SubShader
        {
            Cull Off

            Pass
            {
                CGPROGRAM
                    #pragma vertex VertexProgram
                    #pragma fragment FragmentProgram
                    #define DISPLAY_GAMMA 1.5
                    #define GOLDEN_ANGLE 2.39996323
                    
                    float getBlurSize(float depth, float focusPoint, float focusScale)
                    {
                        float coc = clamp((1.0 / focusPoint - 1.0 / depth) * focusScale, -1.0, 1.0);
                        return abs(coc) * _Max_Blur_Size;
                    }

                    float3 depthOfField(float2 texCoord, float focusPoint, float focusScale)
                    {
                        float depth = tex2D(_CameraDepthTexture, texCoord).r;
                        float centerDepth = depth * _uFar;
                        float centerSize = getBlurSize(centerDepth, focusPoint, focusScale);
                        float3 color = tex2D(_MainTex, texCoord).rgb;
                        float tot = 1.0;

                        float2 texelSize = half2(1.0, 1.0) / half2(_MainTex_TexelSize.z, _MainTex_TexelSize.w);

                        float radius = _Rad_Scale;
                        for (float ang = 0.0; radius < _Max_Blur_Size; ang += GOLDEN_ANGLE)
                        {
                            half2 tc = texCoord + half2(cos(ang), sin(ang)) * texelSize * radius;

                            half3 sampleColor = tex2D(_MainTex, tc).rgb;
                            float sampleDepth = tex2D(_CameraDepthTexture, tc).r * _uFar;
                            float sampleSize = getBlurSize(sampleDepth, focusPoint, focusScale);

                            if (sampleDepth > centerDepth)
                            {
                                sampleSize = clamp(sampleSize, 0.0, centerSize * 2.0);
                            }

                            float m = smoothstep(radius - 0.5, radius + 0.5, sampleSize);
                            color += lerp(color / tot, sampleColor, m);
                            tot += 1.0;
                            radius += _Rad_Scale / radius;
                        }

                        return color /= tot;
                    }

                    float4 FragmentProgram(Interpolators i) : SV_Target
                    {
                        float2 uv = i.uv;

                        float4 color = tex2D(_MainTex, uv).rgba;

                        color.rgb = depthOfField(uv, _Focus_Point, _Focus_Scale);

                        return float4(color.rgb, 1);

                        //tone mapping & inverse gamma correction
                        //color.rgb = float3(1.7, 1.8, 1.9) * color.rgb / (1.0 + color.rgb);
                        //return float4(pow(color.rgb, half3(1.0 / DISPLAY_GAMMA, 1.0 / DISPLAY_GAMMA, 1.0 / DISPLAY_GAMMA)), 1.0);
                        
                        
                        //float depth = tex2D(_CameraDepthTexture, uv).r;
                        //return half4(depth * 0.5 + 0.5, depth * 0.5 + 0.5, depth * 0.5 + 0.5, 1);
                    }
                 ENDCG
            }
        }
}
