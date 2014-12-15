using UnityEngine;
using System.Text;

public static class ShaderBlur
{
    public static float[] MakeRegularOffsets(int sampleCount)
    {
        var offsets = new float[sampleCount];
        var max = sampleCount % 2 == 0 ? sampleCount / 2 - 0.5f : (sampleCount - 1) * 0.5f;
        for (int i = 0; i < sampleCount; i++)
        {
            offsets[i] = i - max;
        }
        return offsets;
    }

    public static float[] MakeLinearOffsets(int sampleCount, float min, float max)
    {
        var offsets = new float[sampleCount];
        int half = sampleCount / 2;
        int plus = half + (sampleCount % 2);
        int minus = half - 1;
        var last = 0f;
        for (int i = 0; i < half; i++)
        {
            var t = half == 1 ? 0.5f : i / (half - 1f);
            last += Mathf.Lerp(min, max, t);
            offsets[plus + i] = last;
            offsets[minus - i] = - last;
        }
        return offsets;
    }

    public static Shaders MakeBoxFilter(float[] sampleOffsets)
    {
        var shaders = new Shaders();
        shaders.xShader = MakeBoxFilter("X", templateSampleX, sampleOffsets);
        shaders.yShader = MakeBoxFilter("Y", templateSampleY, sampleOffsets);
        return shaders;
    }

    static string MakeBoxFilter(string name, string format, float[] offsets)
    {
        var result = new StringBuilder();
        result.AppendFormat(templateHead, name);
        result.Append(templateBegin);
        var vars = new string[offsets.Length];
        for (int i = 0; i < offsets.Length; i++)
        {
            result.AppendFormat(format, i, offsets[i]);
            vars[i] = "a" + i;
        }
        result.AppendFormat(templateReturn, string.Join(" + ", vars), offsets.Length);
        result.Append(templateEnd);
        return result.ToString();
    }

    [System.Serializable]
    public class Shaders
    {
        [Multiline] public string xShader;
        [Multiline] public string yShader;
    }

    const string templateHead = "Shader \"Deformed Shadow/Gaussian Blur {0}\" {{\n";

    const string templateBegin =
        "Properties {_MainTex (\"Source\", 2D) = \"white\" {} }\n" +
        "SubShader {\n" +
        "Tags { \"RenderType\"=\"Opaque\" }\n" +
        "Fog { Mode Off }\n" +
        "Lighting Off\n" +
        "ZWrite Off\n" +
        "ZTest Always\n" +
        "Pass {\n" +
        "CGPROGRAM\n" +
        "#pragma vertex vert\n" +
        "#pragma fragment frag\n" +
        "sampler2D _MainTex;\n" +
        "uniform float2 _MainTex_TexelSize;\n" +
        "struct vin {\n" +
        "float4 position : SV_POSITION;\n" +
        "float2 uv : TEXCOORD0;\n" +
        "};\n" +
        "struct vout {\n" +
        "float4 position : POSITION;\n" +
        "float2 uv;" +
        "\n};\n" +
        "void vert(vin v, inout vout o) {\n" +
        "o.position = mul (UNITY_MATRIX_MVP, v.position);\n" +
        "o.uv = v.uv;}\n" +
        "fixed frag (vout i) : COLOR {\n";

    const string templateSampleX =
        "float a{0} = tex2D(_MainTex, float2(i.uv.x + ({1} * _MainTex_TexelSize.x), i.uv.y)).r;\n";

    const string templateSampleY =
        "float a{0} = tex2D(_MainTex, float2(i.uv.x, i.uv.y + ({1} * _MainTex_TexelSize.y))).r;\n";

    const string templateReturn =
        "return ({0}) / {1};";

    const string templateEnd =
        "}\n" +
        "ENDCG\n" +
        "}\n" +
        "}\n" +
        "}\n";
}
