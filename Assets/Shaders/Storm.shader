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
        Tags { "RenderType"="Opaque" }
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

            float3 tornado(float3 coord, float2 speed, float radius) {
                float2 polar = float2(atan2(coord.x, coord.z), coord.y) + speed * _Time.y;

                return float3(sin(polar.x) * radius, cos(polar.x) * radius, polar.y);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float2 uv1 = i.uv.yx * float2( 5, 10) + float2( 0.5, 0.2) * _Time.y;
                float2 uv2 = i.uv.yx * float2( 3, 6) + float2( -0.7, 0.3) * _Time.y;

                fixed4 col1 = tex2D(_MainTex, uv1);
                fixed4 col2 = tex2D(_MainTex, uv2);

                col1 = lerp(_Col0, _Col1, col1.x);
                col2 = lerp(_Col0, _Col1, col2.x);

                float sceneDepth = LinearEyeDepth (tex2D(_CameraDepthTexture, i.screenPos.xy / i.screenPos.w).x);
                float currentDepth = LinearEyeDepth (i.vertex.z);
                float viewDepth = (sceneDepth - currentDepth);

                float3 coord1 = tornado(i.worldPos, float2( 0.5, 0.2), 2.);
                float3 coord2 = tornado(i.worldPos, float2( -0.7, 0.3), 2.);

                float noise1 = 0.5 + 0.5*octaveNoise(float4(0.3*coord1, _Time.y * 0.1), 8, 0.5);
                float noise2 = 0.5 + 0.5*octaveNoise(float4(0.2*coord2, _Time.y * 0.07), 8, 0.5);
                float noise = 0.5 + 0.5 * sqrt(pow(noise1, 4) + pow(noise2, 4));

                float opacity1 = getOpacityWithOffset(viewDepth, noise1);
                float opacity2 = getOpacityWithOffset(viewDepth, 1 + noise2);

                fixed4 col = lerp(col1, col2, opacity2 / (opacity1 + opacity2));


                float opacity = opacity1 * smoothstep(0.7, 0.9, i.uv.x);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return float4(noise * col.xyz, opacity);;
            }
            ENDCG
        }
    }
}
