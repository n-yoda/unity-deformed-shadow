using UnityEngine;
using UnityEditor;
using System.Collections;

public class EditUnityChan : EditorWindow {
    Transform boxRoot;
    Transform unityRoot;
    Material material;

    void OnGUI() {
        boxRoot = EditorGUILayout.ObjectField("Box", boxRoot, typeof(Transform), true) as Transform;
        unityRoot = EditorGUILayout.ObjectField("Unity Chan", unityRoot, typeof(Transform), true) as Transform;
        material = EditorGUILayout.ObjectField("Material", material, typeof(Material), true) as Material;
        if (GUILayout.Button("Make")) {
            Make();
        }
    }

    void Make() {
        Undo.RecordObjects(new Object[]{boxRoot, unityRoot}, "Edit Unity-chan");
        var childs = unityRoot.GetComponentsInChildren<Transform>();
        foreach(var renderer in boxRoot.GetComponentsInChildren<MeshRenderer>()){
            var newParent = System.Array.Find(childs, x => x.name == renderer.transform.parent.name);
            if (newParent == null) {
                throw new System.Exception("Not found");
            }
            renderer.transform.SetParent(newParent, false);
            renderer.gameObject.layer = LayerMask.NameToLayer("Cast Shadow");
            var materials = renderer.sharedMaterials;
            for (int i = 0; i < renderer.sharedMaterials.Length; i ++) {
                materials[i] = material;
            }
            renderer.sharedMaterials = materials;
        }
        EditorUtility.SetDirty(boxRoot);
        EditorUtility.SetDirty(unityRoot);
    }

    [MenuItem("Window/Deformed Shadow/Edit Unity-chan")]
    static void ShowWindow() {
        GetWindow<EditUnityChan>("Edit Unity-chan");
    }
}
