Shader "Custom/RadialSolidColorFadeInShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1) // Cor padrão branca
        _Fade("Fade", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float4 _Color;
            float _Fade;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                // Calculate radial gradient
                float2 uv = i.uv * 2 - 1; // Center the UV coordinates
                float radius = length(uv);
                float radialFade = smoothstep(_Fade - 0.1, _Fade, radius); // Inverse the gradient

                // Apply the radial fade effect to the solid color
                _Color.a *= radialFade;

                return _Color;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
