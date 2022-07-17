// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/Storm"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Col0 ("Color 0", Color) = (1,1,1,1)
        _Col1 ("Color 1", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        Cull back 
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
        
            #include "./noiseSimplex.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD1;
                float4 screenPos : TEXCOORD2;
                float3 normal : NORMAL;
            };

            sampler2D _MainTex;
            sampler2D _CameraDepthTexture;
            float4 _MainTex_ST;
            float4 _Col0;
            float4 _Col1;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos (v.vertex);
                o.screenPos = ComputeScreenPos(o.vertex);
  
                o.worldPos = mul (unity_ObjectToWorld, v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normal = v.normal;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            float getOpacityWithOffset(float viewDepth, float offset) {
                return max(0., min(1.0 - exp(-(viewDepth - offset) * 1), 1.));
            }

            float octaveNoise(float4 coord, int octaves, float roughness) {
                float sum = 0;

                float4 currCoord = coord;
                float currFac = roughness;

                for (int i = 0; i < octaves; i++) {
                    sum += snoise(currCoord) * currFac;
                    currCoord *= 2;
                    currFac *= roughness;
                }

                return sum;
            }

            float2 tornado(float3 coord, float2 speed, float radius) {
                float2 polar = float2(atan2(coord.x, coord.z), coord.y) + speed * _Time.y;

                return polar;
            }

            float3 intersectCylinder(float3 src, float3 dst, float r) {
                float3 o = src;
                float3 d = dst - src;

                float3 o2 = o*o;
                float3 d2 = d*d;
                float r2 = r*r;

                float t = (sqrt(d2.x * (r2 - o2.z) + d2.z * (r2 - o2.x) + 2*d.z*d.x*o.x*o.z) - d.x*o.x - d.z*o.z) / (d2.x + d2.z);

                return o + t*d;
            }

            float3 polarToCarth(float2 coord, float radius) {

                return float3(radius*float2(sin(coord.x), cos(coord.x)), coord.y);;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 src = _WorldSpaceCameraPos;
                float3 dst = i.worldPos;
                
                float opacity;

                float totNoise;
                fixed4 totCol;

                const int count = 3;
                
                float sceneDepth = LinearEyeDepth (tex2D(_CameraDepthTexture, i.screenPos.xy / i.screenPos.w).x);

                for (int j = 0; j < count; j++) {
                    float2 dir = j % 2 == 0 ? float2( 0.5, -0.2) : float2( -0.7, -0.3);
                    dir = dir * (j % 3 + 1);

                    float r = 2 + 0.5*j;

                    float2 coord = tornado(intersectCylinder(src, dst, r), dir, r) + float2(20.0*j, -5.0*j);

                    fixed4 col = tex2D(_MainTex, coord * float2(7/2/3.141, -0.3));
                    col = lerp(_Col0, _Col1, col.x);

                    float3 carth = polarToCarth(coord, r);

                    float noise = 0.5 + 0.5*octaveNoise(float4(0.3*carth, _Time.y * 0.1), 4, 0.5);

                    float currentDepth = mul(UNITY_MATRIX_MV, carth).z;

                    totNoise += noise;
                    totCol += col;

                    //opacity = getOpacityWithOffset(sceneDepth - currentDepth, 0.);
                }

                /*
                float2 coord1 = tornado(i.worldPos, float2( 0.5, -0.2), 2.);
                float2 coord2 = tornado(intersectCylinder(src, dst, 2.2), float2( -0.7, -0.3), 2.);

                fixed4 col1 = tex2D(_MainTex, coord1 * float2(5/2/3.141, -2));
                fixed4 col2 = tex2D(_MainTex, coord2 * float2( 10/2/3.141, -1.5));

                col1 = lerp(_Col0, _Col1, col1.x);
                col2 = lerp(_Col0, _Col1, col2.x);

                float currentDepth = LinearEyeDepth (i.vertex.z);
                float viewDepth = (sceneDepth - currentDepth);

                float noise1 = 0.5 + 0.5*octaveNoise(float4(0.3*polarToCarth(coord1, 2), _Time.y * 0.1), 8, 0.5);
                float noise2 = 0.5 + 0.5*octaveNoise(float4(0.2*polarToCarth(coord2, 3), _Time.y * 0.07), 8, 0.5);
                float noise = 0.5 + 0.5 * sqrt(pow(noise1, 4) + pow(noise2, 4));

                float opacity1 = getOpacityWithOffset(viewDepth, noise1);
                float opacity2 = getOpacityWithOffset(viewDepth, 1 + noise2);

                fixed4 col = lerp(col1, col2, opacity2 / (opacity1 + opacity2));*/

                
                float currentDepth = LinearEyeDepth (i.vertex.z);
                float viewDepth = (sceneDepth - currentDepth);
                float opacity1 = getOpacityWithOffset(viewDepth, 0.1);


                opacity = opacity1 * smoothstep(0.7, 0.9, i.uv.x);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return float4(totNoise / count * totCol.xyz / count, opacity);;
            }
            ENDCG
        }
    }
}
