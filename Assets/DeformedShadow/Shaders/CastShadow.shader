Shader "Deformed Shadow/Cast Shadow" {
	Properties {
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
        Fog { Mode Off }
		Lighting Off
		Cull Off
		ZWrite On
		ZTest Less
        Pass{
		    CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			#include "UnityCG.cginc"

            struct vin {
				float4 position : SV_POSITION;
			};

            struct vout {
				float4 position : POSITION;
				fixed near;
			};

            void vert(vin i, inout vout o) {
                o.position = mul (UNITY_MATRIX_MVP, i.position);
                fixed absz = abs(o.position.z);
                o.near = 1 - absz;
                o.position.xy *= 1 + absz * 0.1;
                o.position.z = - o.position.z;
            }

            fixed frag (vout i) : COLOR {
                return i.near;
            }
            ENDCG
    	}
	}
}
