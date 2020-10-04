// 表示名
// マテリアルに設定するときUIのしたに出る[
Shader "UI/TransitionShader"
{
    Properties
    {
        // テクスチャ　カラー　レンジの設定
        [PerRendererData] _MaskTex ("Mask Texture", 2D) = "black" {}
        [PerRendererData] _Color("Color", Color)        = (0,0,0,1)
        _Range("Range", Range(0, 1))  = 1
    }
    SubShader
    {
        Tags { 
            "RenderType"        = "AlphaTest"
            "IgnoreProjector"   = "True"
            "RenderType"        = "TransparentCutout"
            "PreviewType"       = "Plane"
            "CanUseSpriteAtlas" = "True"
        }

        Pass
        {
            Cull Off
            ZWrite Off
            ZTest[unity_GUIZTestMode]
            Lighting Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            struct appdata
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct vtf
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 uv       : TEXCOORD0;
                // half2 texcoord  : TEXCOORD0;
                float4 worldPos : TEXCOORD1;
            };

            fixed4 _Color;
            fixed4 _TextureSampleAdd;


            vtf vert (appdata v)
            {
                vtf o;
                o.worldPos = v.vertex;
                o.vertex = UnityObjectToClipPos(o.worldPos);
                o.uv = v.uv;

                #ifdef UNITY_HALF_TEXEL_OFFSET
                o.vertex.xy += (_ScreenParams.zw - 1.0) * float2(-1, 1);
                #endif

                o.color = v.color * _Color;
                return o;
            }
            sampler2D _MaskTex;
            sampler2D _MainTex;
            float _Range;

            fixed4 frag (vtf i) : SV_Target
            {
                fixed4 col = _Color;
                float mask = tex2D(_MaskTex, i.uv).a - (-1 + _Range * 2);
                col.a = mask;
                clip(mask - 0.001);
                return col;
            }
            ENDCG
        }
    }
}
