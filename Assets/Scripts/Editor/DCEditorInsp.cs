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
            if (GUILayout.Button("**重置**(谨慎)"))
            {
                bool result = EditorUtility.DisplayDialog("注意", "确定要重置棋盘吗?", "OK", "Cancel");
                if (result)
                {
                    mgr.ResetBoard();
                    EditorUtility.SetDirty(mgr);
                }
            }
            if (GUILayout.Button("获取所有遮挡关系"))
            {
                mgr.GetAllColliderRelation();
            }
            
            GUILayout.Label("对称");
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("沿x轴对称"))
            {
                mgr.SymmetricalAlongX();
            }
            if (GUILayout.Button("沿z轴对称"))
            {
                mgr.SymmetricalAlongZ();
            }
            GUILayout.EndHorizontal();
            
            if (GUILayout.Button("导出配置"))
            {
                mgr.ExportCfg();
            }
            if (GUILayout.Button("导入配置"))
            {
                mgr.ImportCfg();
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
