Shader "Custom/VignetteShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _VignettePower ("VignettePower", Range(0.0, 5.0)) = 0.5
        // TODO: Add vignette color.
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float _VignettePower;

            fixed4 frag (v2f i) : SV_Target
            {
                float4 renderTex = tex2D(_MainTex, i.uv);
                float2 dist = (i.uv - 0.5f) * 1.5f;
                dist = dist * dist * dist;
                dist.x = 1 - dot(dist, dist)  * _VignettePower;
                renderTex *= dist.x;
                return renderTex;
            }
            ENDCG
        }
    }
}
