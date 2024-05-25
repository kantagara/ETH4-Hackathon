Shader "Custom/ProgressiveBuildShader"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _SpecColor ("Specular Color", Color) = (1,1,1,1)
        _Color ("Color", Color) = (1,1,1,1)
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _BumpMap ("Normal Map", 2D) = "bump" {}
        _Progress ("Building Progress (0-1)", Range(0,1)) = 0.5
        _ObjectHeight ("Object Height", Float) = 10.0 // Default object height, adjust as necessary
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 200

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite On
        AlphaTest Greater 0.5

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows addshadow

        sampler2D _MainTex;
        sampler2D _BumpMap;
        fixed3 _Color;
        float _Progress;
        float _ObjectHeight;
        float _Glossiness;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Normal mapping
            float3 normal = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex));
            o.Normal = normal;

            // Compute height fraction based on the world position normalized by the object's height
            float heightFraction = IN.worldPos.y / _ObjectHeight;

            // Albedo and visibility based on building progress
            fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = tex.rgb * step(heightFraction, _Progress) * _Color;
            o.Alpha = tex.a * step(heightFraction, _Progress);

            // Specular and smoothness
            o.Smoothness = _Glossiness;

            // Cut-off transparency
            clip(o.Alpha - 0.01); // Adjust the alpha cutoff value for better edges
        }
        ENDCG
    }
    FallBack "Diffuse"
}
