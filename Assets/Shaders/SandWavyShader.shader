Shader "Custom/SandWavyShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", 2D) = "white" {}
        _Normal ("Normal", 2D) = "white" {}
        _Speed ("Speed", Range(0,2)) = 0.1
        _Distortion ("Distortion", Range(0,0.1)) = 0.01
        _DispTex ("Displacement Texture", 2D) = "gray" {}
        _Displacement ("Displacement Scale", Range(0, 0.1)) = 0.03
        _Tess ("Tessellation", Range(1,128)) = 4
     }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows vertex:disp tessellate:tessDistance 

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        #include "./noiseSimplex.cginc"
        #include "Tessellation.cginc"

        sampler2D _MainTex;
        sampler2D _Glossiness;
        sampler2D _Normal;
        sampler2D _DispTex;
        fixed4 _Color;

        float _Speed;
        float _Distortion;
        float _Displacement;

        struct Input
        {
            float2 uv_MainTex;
        };


        float2 sandWaves(float2 uv, float t) {
            float time = t * _Speed;
            float4 coord = float4(10 * uv, time, time + 1.0);
            return _Distortion * float2(snoise(coord.xyz), snoise(coord.xyw));
        }

        struct appdata {
            float4 vertex : POSITION;
            float4 tangent : TANGENT;
            float3 normal : NORMAL;
            float2 texcoord : TEXCOORD0;
        };
        
        
        float _Tess;

        float4 tessDistance (appdata v0, appdata v1, appdata v2) {
            float minDist = 10.0;
            float maxDist = 25.0;
            return UnityDistanceBasedTess(v0.vertex, v1.vertex, v2.vertex, minDist, maxDist, _Tess);
        }

        
        void disp (inout appdata v)
        {
            float2 wave = sandWaves(10.* v.texcoord.xy, _Time.y);

            float2 coord = 10.* v.texcoord.xy;

            float d = (tex2Dlod(_DispTex, float4(coord.xy,0,0)).r - 0.5) * _Displacement;
            v.vertex.xyz += float3(wave.x, 0., wave.y) + v.normal * d;
        }


        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float2 wave = sandWaves(IN.uv_MainTex, _Time.y);

            float2 coord = IN.uv_MainTex; // + wave;

            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, coord) * _Color;
            fixed4 gloss = tex2D (_Glossiness, coord);

            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = 0.;
            o.Smoothness = gloss;
            o.Alpha = c.a;
            o.Normal =  UnpackNormal(tex2D (_Normal, coord));
        }
        ENDCG
    }
    FallBack "Diffuse"
}
