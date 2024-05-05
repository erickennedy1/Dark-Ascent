Shader "Custom/ReverseGradientOpacity"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _OpacitySpeed ("Opacity Speed", float) = 1.0
        _Center ("Center", Vector) = (0.5,0.5,0,0)
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        LOD 100

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            uniform sampler2D _MainTex;
            float _OpacitySpeed;
            float4 _Center;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float2 texcoord : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 center = _Center.xy;
                float2 toCenter = center - i.texcoord;
                float distance = length(toCenter);

                float opacity = distance * _OpacitySpeed;
                opacity = clamp(opacity, 0.0, 1.0);

                fixed4 col = tex2D(_MainTex, i.texcoord);
                col.a *= opacity;
                return col;
            }
            ENDCG
        }
    }
}
