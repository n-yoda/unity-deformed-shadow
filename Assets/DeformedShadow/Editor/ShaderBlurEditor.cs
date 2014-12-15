using UnityEditor;
using UnityEngine;
using System.IO;

public class ShaderBlurEditor : EditorWindow
{
    int samplingCount = 9;
    float minSampleScale = 1f;
    float maxSampleScale = 3f;

    void OnGUI()
    {
        samplingCount = EditorGUILayout.IntField("Sampling Count", samplingCount);
        minSampleScale = EditorGUILayout.FloatField("Min Sample Scale", minSampleScale);
        maxSampleScale = EditorGUILayout.FloatField("Max Sample Scale", maxSampleScale);
        if (GUILayout.Button("Create")) {
            var save = EditorUtility.SaveFilePanel("Save Shaders", "Assets/DeformedShadow/Shaders", "Box", "shader");
            if (!string.IsNullOrEmpty(save)) {
                var shaders = ShaderBlur.MakeBoxFilter(ShaderBlur.MakeLinearOffsets(samplingCount, minSampleScale, maxSampleScale));
                var pathX = Path.ChangeExtension(save, ".X.shader");
                File.WriteAllText(pathX, shaders.xShader);
                var pathY = Path.ChangeExtension(save, ".Y.shader");
                File.WriteAllText(pathY, shaders.yShader);
                AssetDatabase.Refresh();
            }
        }
    }

    [MenuItem("Window/Deformed Shadow/Blur")]
    static void ShowWindow() {
        GetWindow<ShaderBlurEditor>("Blur");
    }
}
