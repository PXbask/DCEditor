using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MirrorChildren))]
public class MirrorInsp:Editor
{
    public override void OnInspectorGUI()
    {

        MirrorChildren myComponent = (MirrorChildren)target;

        EditorGUILayout.BeginHorizontal();


        if (GUILayout.Button("镜像XXXX"))
        {
            myComponent.Mirror();
        }
        EditorGUILayout.EndHorizontal();
        
    }
}
