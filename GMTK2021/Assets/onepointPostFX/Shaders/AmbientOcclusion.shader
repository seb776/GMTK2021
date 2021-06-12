// Taken and tweaked from https://www.shadertoy.com/view/Ms33WB


Shader "opPostFX/AmbientOcclusion"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Sample("Samples", Range(8,100)) = 16
        _Sample_Radian("Samples Radian", Range(0, 1)) = 0.02
        _Intensity("Intensity", Range(1,30)) = 1.
        _Scale("Scale", Range(1, 400)) = 2.5
        _Bias("Bias", Range(0.,1.)) = 0.5
        _MaxDist("Max Distance", Range(0,1)) = 0.07
    }

    SubShader
    {
        Cull Off

        Pass
        {
            CGPROGRAM
                #pragma vertex VertexProgram
                #pragma fragment FragmentProgram

                #define MOD3 half3(.1031,.11369,.13787)
                    #include "UnityCG.cginc"

                sampler2D _MainTex;
                texture2D _CameraDepthTexture;
                float4 _MainTex_TexelSize;
                float3 __WorldSpaceCameraPos;
                int _Sample;
                float _Intensity, _Scale, _Bias, _Sample_Radian, _MaxDist;

                struct VertexData {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct MyInterpolators {
                    float4 pos : SV_POSITION;
                    float2 uv : TEXCOORD0;
                    float3 viewDirection : TEXCOORD1;
                };

                MyInterpolators VertexProgram(VertexData v) {
                    MyInterpolators i;
                    i.viewDirection = WorldSpaceViewDir(v.vertex);
                    i.pos = UnityObjectToClipPos(v.vertex);
                    i.uv = v.uv;
                    return i;
                }
                
                SamplerState sampler_CameraDepthTexture
                {

                };

                float hash12(float2 p)
                {
                    half3 p3 = frac(half3(p.x, p.y, p.x) * MOD3);
                    p3 += dot(p3, half3(p3.y, p3.z, p3.x) + 19.19);
                    return frac((p3.x + p3.y) * p3.z);
                }

                float2 hash22(float2 p)
                {
                    float3 p3 = frac(float3(p.x, p.y, p.x) * MOD3);
                    p3 += dot(p3, float3(p3.y, p3.z, p3.x) + 19.19);
                    return frac(float2((p3.x + p3.y) * p3.z, (p3.x+p3.z)*p3.y));
                }
                float _fl;
                float3 getPosition(float2 uv)
                {
                    float fl = 1.;// _CameraDepthTexture.SampleLevel(sampler_CameraDepthTexture, float2(0, 0), 0).x;
                    float d = _CameraDepthTexture.SampleLevel(sampler_CameraDepthTexture, uv, 0).x;

                    float2 p = uv * 2. - 1.;
                    float3x3 ca = float3x3(1.,0.,0.,0.,1.,0.,0.,0.,-1. / 1.5);
                    float3 rd = normalize(mul(ca, float3(p, fl)));

                    float3 pos = rd * d;
                    return pos;
                }

                float3 getNormal(float2 uv) {
                    return _CameraDepthTexture.SampleLevel(sampler_CameraDepthTexture, uv, 0).xxx;
                }

                float2 getRandom(float2 uv) {
                    return normalize(hash22(uv * 126.1231) * 2. - 1.);
                }

                float doAmbientOcclusion(in float2 tcoord, in float2 uv, in float3 p, in float3 cnorm)
                {
                    float3 diff = getPosition(tcoord + uv) - p;
                    float l = length(diff);
                    float3 v = diff / l;
                    float d = l * _Scale;
                    float ao = max(0.0, dot(cnorm, v) - _Bias) * (1.0 / (1.0 + d));
                    ao *= smoothstep(_MaxDist, _MaxDist * 0.5, l);
                    return ao;
                }

                float spiralAO(float2 uv, float3 p, float3 n, float rad)
                {
                    float goldenAngle = 2.4;
                    float ao = 0.;
                    float inv = 1. / float(_Sample);
                    float radius = 0.;

                    float rotatePhase = hash12(uv * 100.) * 6.28;
                    float rStep = inv * rad;
                    float2 spiralUV;

                    for (int i = 0; i < _Sample; i++) {
                        spiralUV.x = sin(rotatePhase);
                        spiralUV.y = cos(rotatePhase);
                        radius += rStep;
                        ao += doAmbientOcclusion(uv, spiralUV * radius, p, n);
                        rotatePhase += goldenAngle;
                    }
                    ao *= inv;
                    return ao;
                }

                float getFL(float3 fwd)
                {
                    float depth = _CameraDepthTexture.Sample(sampler_CameraDepthTexture, float2(0., 0.)).r;

                    float3 worldPos = _WorldSpaceCameraPos + normalize(fwd) * depth;

                    float3 n = normalize(cross(ddy(worldPos), ddx(worldPos)));
                    return n.x;
                }

                float4 FragmentProgram(MyInterpolators i) : SV_Target
                {
                    float2 uv = i.uv;
                    float3 p = getPosition(uv);

                    float depth = _CameraDepthTexture.Sample(sampler_CameraDepthTexture, uv).r;
                    float3 cameraRightW = mul((float3x3)unity_CameraToWorld, float3(1, 0, 0));
                    float3 cameraUpW = mul((float3x3)unity_CameraToWorld, float3(0, 1, 0));
                    float3 cameraFwdW = mul((float3x3)unity_CameraToWorld, float3(0, 0, 1));

                    // Screen size
                    float scrW = _ScreenParams.x;
                    float scrH = _ScreenParams.y;
                    float h = 2.0f / unity_CameraProjection._m11;
                    float w = h * scrW / scrH;
                    float3 ray = cameraFwdW + (i.uv.x - 0.5f) * w * cameraRightW
                        + (i.uv.y - 0.5f) * h * cameraUpW;

                    float3 normViewDir = normalize(ray);

                    float3 worldPos = _WorldSpaceCameraPos + normViewDir * depth;

                    float3 n = normalize(cross(ddy(worldPos), ddx(worldPos)));
                    
                    float ao = 0.;
                    float rad = _Sample_Radian / p.z;
                    _fl = getFL(cameraFwdW);
                    ao = spiralAO(uv, p, n, rad);
                    ao = 1. - ao * _Intensity;

                    if (depth < 0.01)
                        ao = 1.;

                    //return float4(normViewDir * 0.5 + 0.5, 1);
                    return float4(ao, ao, ao, 1.0)*  float4(tex2D(_MainTex, uv).xyz, 1.);
                }
            ENDCG
        }
    }
}
