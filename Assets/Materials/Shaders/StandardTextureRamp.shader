Shader "Custom/StandardTextureRamp"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _AlbedoRamp ("Albedo Ramp", 2D) = "white" {}
        _AltColor ("Alternate Color", Color) = (0,0,0,1)
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _MetallicGlossMap ("Metallic Map", 2D) = "white" {}
        _BumpMap ("Bumpmap", 2D) = "bump" {}
        
        _RampOffset("Ramp Offset", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        
        #pragma surface surf Standard fullforwardshadows
        
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex : TEXCOORD0;
            float2 uv2_AlbedoRamp : TEXCOORD1;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        fixed4 _AltColor;
        float _RampOffset;
        
        sampler2D _BumpMap;
        sampler2D _MetallicGlossMap;
        sampler2D _AlbedoRamp;

        UNITY_INSTANCING_BUFFER_START(Props)
            
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
        
            float offset = IN.uv2_AlbedoRamp.y;
            offset += _RampOffset;
            offset = 1-clamp(offset-.5, .2, 1);
            
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            //fixed4 c2 = tex2D(_AlbedoRamp, IN.uv_MainTex);
            fixed4 c2 = _AltColor;
            c = (c * (1-offset )) + (c2 * offset);
            o.Albedo = c.rgb;
            
            fixed4 metallicMap = tex2D (_MetallicGlossMap, IN.uv_MainTex);
            o.Metallic = _Metallic * metallicMap;
            o.Smoothness = _Glossiness * metallicMap;
            
            o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_MainTex));
            
            
            
            //o.Albedo *= tex2D(_AlbedoRamp, uv2);
            
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
