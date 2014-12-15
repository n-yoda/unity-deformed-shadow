Shader "Deformed Shadow/Gaussian Blur Y" {
Properties {_MainTex ("Source", 2D) = "white" {} }
SubShader {
Tags { "RenderType"="Opaque" }
Fog { Mode Off }
Lighting Off
ZWrite Off
ZTest Always
Pass {
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
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
o.uv = v.uv;}
fixed frag (vout i) : COLOR {
float a0 = tex2D(_MainTex, float2(i.uv.x, i.uv.y + (-2 * _MainTex_TexelSize.y))).r;
float a1 = tex2D(_MainTex, float2(i.uv.x, i.uv.y + (-1 * _MainTex_TexelSize.y))).r;
float a2 = tex2D(_MainTex, float2(i.uv.x, i.uv.y + (0 * _MainTex_TexelSize.y))).r;
float a3 = tex2D(_MainTex, float2(i.uv.x, i.uv.y + (1 * _MainTex_TexelSize.y))).r;
float a4 = tex2D(_MainTex, float2(i.uv.x, i.uv.y + (2 * _MainTex_TexelSize.y))).r;
return (a0 + a1 + a2 + a3 + a4) / 5;}
ENDCG
}
}
}
