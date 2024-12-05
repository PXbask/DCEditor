using QFramework;
using UnityEditor;
using UnityEngine;

namespace DCEditor
{
    [CustomEditor(typeof(DCDominoCard))]
    public class DCDominoCardInsp : Editor
    {
        private DCDominoCard card;

        private int tmp_layer;
        
        public override void OnInspectorGUI()
        {
            card = (DCDominoCard)target; 
            GUILayout.BeginVertical();
            if (GUILayout.Button("删除物体"))
            {
                card.DestroyObj();
                return;
            }
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("层级");
            tmp_layer = EditorGUILayout.IntField(tmp_layer);
            if (GUILayout.Button("修改"))
            {
                card.UpdateLayer(tmp_layer);
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            base.OnInspectorGUI();
        }
    }
}
