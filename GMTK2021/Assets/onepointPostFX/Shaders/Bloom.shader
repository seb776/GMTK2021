Shader "opPostFX/Bloom"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Threshold("Threshold", Range(0,1)) = 0.5
        _Radius("Radius", Range(1,500)) = 1
        _Intensity("Intensity", Range(0,1)) = 0.5
    }

    CGINCLUDE
        #include "UnityCG.cginc"

        sampler2D _MainTex;
        float4 _MainTex_TexelSize;
        float _Blur, _Threshold, _Radius, _Intensity;
        float _isToggled;

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

            Pass {
                CGPROGRAM
                    #pragma vertex VertexProgram
                    #pragma fragment FragmentProgram
                    #define pow2(x) (x * x)

                half3 linearToneMapping(half3 color)
                {
                    float exposure = 1.;
                    color = clamp(exposure * color, 0., 1.);
                    color = pow(color, half3(1. / 2.2, 1. / 2.2, 1. / 2.2));
                    return color;
                }
                
                float gaussian(half2 i) {
                    float pi = atan(1.0) * 4.0;
                    float sigma = float(_Radius) * 0.25;

                    return 1.0 / (2.0 * pi * pow2(sigma)) * exp(-((pow2(i.x) + pow2(i.y)) / (2.0 * pow2(sigma))));
                }

                half3 blur(sampler2D sp, half2 uv, half2 scale) {
                    half3 col = half3(0.0, 0.0, 0.0);
                    float accum = 0.0;
                    float weight;
                    half2 offset;
                    float sigma = float(_Radius) * 0.25;

                    for (int x = -sigma / 2; x < sigma / 2; x += 3) {
                        for (int y = -sigma / 2; y < sigma / 2; y += 3) {
                            offset = half2(x, y);
                            weight = gaussian(offset);
                            half3 smple = tex2D(sp, uv + scale * offset).rgb;
                            float luminance = 0.2126 * smple.x + 0.7152 * smple.y + 0.0722 * smple.z;
                            if (luminance > _Threshold)
                                col += smple * weight;
                            accum += weight;
                        }
                    }

                    return col / accum;
                }


                float normpdf(in float x, in float sigma)
                {
                    return 0.39894 * exp(-0.5 * x * x / (sigma * sigma)) / sigma;
                }


                float3 blur2(float4 pos, float2 uv)
                {
                    float3 c = tex2D(_MainTex, uv).rgb;
                    //declare stuff
                    const int _mSize = 11;
                    const int kSize = (_mSize - 1) / 2;

                    half3 final_colour = half3(0.0, 0., 0.);

                    float kernel[_mSize];
                    //create the 1-D kernel
                    float sigma = 7.0;
                    float Z = 0.0;
                    for (int j = 0; j <= kSize; ++j)
                    {
                        kernel[kSize + j] = kernel[kSize - j] = normpdf(float(j), sigma);
                    }

                    //get the normalization factor (as the gaussian has been clamped)
                    for (int j = 0; j < _mSize; ++j)
                    {
                        Z += kernel[j];
                    }

                    //read out the texels
                    for (int i = -kSize; i <= kSize; ++i)
                    {
                        for (int j = -kSize; j <= kSize; ++j)
                        {
                            final_colour += kernel[kSize + j] * kernel[kSize + i] * tex2D(_MainTex, (pos.xy + half2(float(i),float(j))) / half2(_ScreenParams.x, _ScreenParams.y)).rgb;

                        }
                    }


                    return final_colour / (Z * Z);
                }

                float3 blur3(float2 uv)
                {
                    float Pi = 6.28318530718; // Pi*2

                    // GAUSSIAN BLUR SETTINGS {{{
                    float Directions = 16.0; // BLUR DIRECTIONS (Default 16.0 - More is better but slower)
                    float Quality = 3.0; // BLUR QUALITY (Default 4.0 - More is better but slower)
                    float Size = 8.0; // BLUR SIZE (Radius)
                    // GAUSSIAN BLUR SETTINGS }}}

                    float2 Radius = Size / half2(_ScreenParams.x, _ScreenParams.y);

                    // Pixel colour
                    float4 Color = tex2D(_MainTex, uv);

                    // Blur calculations
                    for (float d = 0.0; d < Pi; d += Pi / Directions)
                    {
                        for (float i = 1.0 / Quality; i <= 1.0; i += 1.0 / Quality)
                        {
                            Color += tex2D(_MainTex, uv + float2(cos(d),sin(d)) * Radius * i);
                        }
                    }

                    // Output to screen
                    Color /= Quality * Directions - 15.0;
                    return Color;
                }

                half3 Bloom(float2 uv, float4 pos, float threshold, float radius, float intensity, half2 ps)
                {
                    half3 col = half3(0.,0.,0.);
                    col = tex2D(_MainTex, uv).xyz;

                    //half3 bloomSample = blur(_MainTex, uv, ps);
                    //half3 bloomSample = blur2(pos, uv);
                    half3 bloomSample = blur3(uv);

                    col = col + (bloomSample * intensity);
                    return col;
                }


                half4 FragmentProgram(Interpolators i) : SV_Target{
                    float2 uv = i.uv;
                    half2 ps = half2(1.0, 1.0) / half2(_MainTex_TexelSize.z,  _MainTex_TexelSize.w);
                    half3 col = Bloom(uv, i.pos, _Threshold, _Radius, _Intensity, ps);
                    return half4(col, 1.0);
                }
            ENDCG
        }
    }
}
