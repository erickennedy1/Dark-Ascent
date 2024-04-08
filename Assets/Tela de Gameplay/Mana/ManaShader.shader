Shader "Custom/UnlitSpriteWaterShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Amplitude ("Amplitude da Onda", Float) = 0.05
        _Frequencia ("Frequência da Onda", Float) = 2.0
        _HeightThreshold ("Altura Inicial da Onda", Float) = 0.5
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha
        AlphaTest Greater 0.1
        ColorMask RGB
        Cull Off
        Lighting Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float _Amplitude;
            float _Frequencia;
            float _HeightThreshold;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.uv = v.uv;

                float heightAboveThreshold = v.vertex.y - _HeightThreshold;
                if(heightAboveThreshold > 0)
                {
                    float wave = sin(_Time.y * _Frequencia + v.vertex.x) * _Amplitude * heightAboveThreshold;
                    v.vertex.y += wave;
                }

                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
}
