using UnityEditor;
using UnityEngine;

namespace DCEditor
{
    [CustomEditor(typeof(DCLayer))]
    public class DCLayerInsp : Editor
    {
        private DCLayer layer;

        public override void OnInspectorGUI()
        {
            layer = (DCLayer)target; 
            
            GUILayout.BeginVertical();
            GUILayout.Label("对称操作");
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("沿x轴对称"))
            {
                layer.SymmetricalAlongX();
            }
            if (GUILayout.Button("沿z轴对称"))
            {
                layer.SymmetricalAlongZ();
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }
    }
}
