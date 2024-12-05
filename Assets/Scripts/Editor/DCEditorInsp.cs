using System;
using UnityEditor;
using UnityEngine;

namespace DCEditor
{
    [CustomEditor(typeof(DCEditorMgr))]
    public class DCEditorInsp : Editor
    {
        private DCEditorMgr mgr;

        private int tmp_layerCount;

        public override void OnInspectorGUI()
        {
            mgr = (DCEditorMgr)target; 
            GUILayout.BeginVertical();
            if (GUILayout.Button("生成新物体"))
            {
                mgr.CreateNew();
            }
            if (GUILayout.Button("**重置**(谨慎)"))
            {
                bool result = EditorUtility.DisplayDialog("注意", "确定要重置棋盘吗?", "OK", "Cancel");
                if (result)
                {
                    mgr.ResetBoard();
                }
            }
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("层级数");
            tmp_layerCount = EditorGUILayout.IntField(tmp_layerCount);
            if (GUILayout.Button("修改"))
            {
                mgr.UpdateLayerDetails(tmp_layerCount);
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            base.OnInspectorGUI();
        }
    }
}
