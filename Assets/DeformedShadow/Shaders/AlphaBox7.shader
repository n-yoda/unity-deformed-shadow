Shader "Deformed Shadow/Alpha Only Sparse Box Filter 7" {
	Properties {
		_MainTex ("Source", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		Fog { Mode Off }
		Lighting Off
		ZWrite Off
		ZTest Always
		Pass{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			uniform float2 _MainTex_TexelSize;

			struct vin {
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			struct vout {
				float4 position : POSITION;
				float2 uv;
			};

			void vert(vin v, inout vout o) {
				o.position = mul (UNITY_MATRIX_MVP, v.position);
				o.uv = v.uv;
			}

			fixed frag (vout i) : COLOR {
				float d = _MainTex_TexelSize.x * 10;
				float n3 = tex2D(_MainTex, float2(i.uv.y, i.uv.x - d - d - d)).r;
				float n2 = tex2D(_MainTex, float2(i.uv.y, i.uv.x - d - d)).r;
				float n1 = tex2D(_MainTex, float2(i.uv.y, i.uv.x - d)).r;
				float p0 = tex2D(_MainTex, float2(i.uv.y, i.uv.x)).r;
				float p1 = tex2D(_MainTex, float2(i.uv.y, i.uv.x + d)).r;
				float p2 = tex2D(_MainTex, float2(i.uv.y, i.uv.x + d + d)).r;
				float p3 = tex2D(_MainTex, float2(i.uv.y, i.uv.x + d + d + d)).r;
				return (n3 + n2 + n1 + p0 + p1 + p2 + p3) / 7;
			}
			ENDCG
		}
	}
}
