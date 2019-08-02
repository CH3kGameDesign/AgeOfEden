

Shader "Unlit/Rorschach"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
        _BlackLevel("Black Level", Range(0, 1)) = 0.27
        _Contrast("Contrast", float) = 60
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _BlackLevel;
            float _Contrast;
			fixed4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                o.uv = v.uv * 2.0f - 1.0f;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 mirrored = abs(i.uv);
                float2 uv = float2(mirrored.x, i.uv.y) * _MainTex_ST.xy;

                // Mess with these values tos change the movement speed/direction/blobbiness.
                float noise = tex2D(_MainTex, 0.3f * uv + _Time.x * float2(0.2f, 0.1f)).r
                    * tex2D(_MainTex, uv + _Time.x * float2(-0.3f, -0.1f) + float2(0.2f, 0.3f)).r;

                float mask = max(mirrored.x, mirrored.y);
                mask *= mask * mask * _BlackLevel;
                return saturate((noise + mask - _BlackLevel) * _Contrast) * _Color;
            }
            ENDCG
        }
    }
}