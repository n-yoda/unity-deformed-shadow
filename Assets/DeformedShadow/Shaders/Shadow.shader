Shader "Deformed Shadow/Shadow" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Color ("Color", Color) = (0, 0, 0, 0.5)
	}
	SubShader {
		Tags { "RenderType"="Transparent" "Queue"="Transparent" "IgnoreProjector"="True" }
		LOD 200
		ZTest LEqual
		Blend One OneMinusSrcAlpha
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			sampler2D _MainTex;
			uniform float2 _MainTex_TexelSize;
			fixed4 _Color;

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
				o.position.z -= 0.001;
				o.uv = v.uv;
			}

			fixed4 frag (vout i) : COLOR {
				fixed4 c = _Color;
				float2 d = _MainTex_TexelSize * 0.25;
				c.a *= (tex2D(_MainTex, i.uv + d).r + tex2D(_MainTex, i.uv - d).r) * 0.5;
				c.rgb *= c.a;
				return c;
			}
			ENDCG
		}
	}
}
