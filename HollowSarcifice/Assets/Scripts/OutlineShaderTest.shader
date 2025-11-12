Shader "Sprites/OutlineShaderTest"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineSize ("Outline Size", Range(0.0, 0.2)) = 0.05
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }
        Blend One OneMinusSrcAlpha
        Cull Off Lighting Off ZWrite Off

        Pass
        {
            Name "Outline"
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
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            float4 _OutlineColor;
            float _OutlineSize;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 offsets[8] = {
                    float2(-1, 0), float2(1, 0),
                    float2(0, -1), float2(0, 1),
                    float2(-1, -1), float2(-1, 1),
                    float2(1, -1), float2(1, 1)
                };

                float alpha = 0.0;
                for (int j = 0; j < 8; j++)
                {
                    float2 offset = offsets[j] * _MainTex_TexelSize.xy * _OutlineSize * 100.0;
                    alpha = max(alpha, tex2D(_MainTex, i.uv + offset).a);
                }

                fixed4 texColor = tex2D(_MainTex, i.uv);
                fixed4 outline = _OutlineColor * (alpha - texColor.a);
                return outline + texColor;
            }
            ENDCG
        }
    }
}
