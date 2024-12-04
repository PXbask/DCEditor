using System;
using UnityEditor;
using UnityEngine;

namespace DCEditor
{
    [CustomEditor(typeof(DCEditorMgr))]
    public class DCEditorInsp : Editor
    {
        private DCEditorMgr mgr;

        public override void OnInspectorGUI()
        {
            mgr = (DCEditorMgr)target; 
            GUILayout.BeginVertical();
            if (GUILayout.Button("生成新物体"))
            {
                mgr.CreateNew();
            }
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("层级数");
            mgr.LayerCount = EditorGUILayout.IntField(mgr.LayerCount);
            if (GUILayout.Button("修改"))
            {
                mgr.UpdateLayerDetails(mgr.LayerCount);
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            base.OnInspectorGUI();
        }
    }
}
