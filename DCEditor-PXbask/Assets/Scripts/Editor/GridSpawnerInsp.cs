using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ObjectSpawner))]
public class GridSpawnerInsp : Editor
{
    public override void OnInspectorGUI()
    {

        ObjectSpawner myComponent = (ObjectSpawner)target;

        EditorGUILayout.BeginHorizontal();

        myComponent.X = EditorGUILayout.IntField("X", myComponent.X);

        if (GUILayout.Button("生成XXXX"))
        {
            myComponent.CreateX();
        }
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();
        myComponent.Z = EditorGUILayout.IntField("Z", myComponent.Z);
        if (GUILayout.Button("生成ZZZZ"))
        {
            myComponent.CreateZ();
        }
        EditorGUILayout.EndHorizontal();

        myComponent.objectPrefab = (GameObject)EditorGUILayout.ObjectField("Prefab", myComponent.objectPrefab, typeof(GameObject), false);
        
    }
}
