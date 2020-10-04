Shader "Unlit/Block"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Seed("Seed", Int) = 0
		_Size("Size", Int) = 1
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" }

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

				int _Seed;
				int _Size;

				float random(float2 st, int seed)
				{
					return frac(sin(dot(st.xy, float2(12.9898, 78.233)) + seed) * 43758.5453123);
				}

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					fixed4 col;

					float2 st = i.uv * max(_Size, 1);
					float2 ipos = floor(st);
					col.rgb = random(ipos, _Seed);
					col.a = 1;
					return col;
				}
				ENDCG
			}
		}
}
