using DCEditor.Data;
using UnityEditor;
using UnityEngine;

namespace DCEditor.Drawer
{
    [CustomPropertyDrawer(typeof(DominoData))]
    public class DominoCardDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // 获取title属性
            SerializedProperty titleProp = property.FindPropertyRelative("id");
            // 设置自定义标题
            GUIContent customLabel = new GUIContent("骨牌id");
            // 绘制属性
            EditorGUI.PropertyField(position, property, customLabel, true);
            EditorGUI.EndProperty();
        }
    }
}
