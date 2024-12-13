using System;
using UnityEditor;
using UnityEngine;


namespace DCEditor.Drawer
{
    public class DisplayNameAttribute : PropertyAttribute
    {
        public string displayName;

        public DisplayNameAttribute(string content)
        {
            displayName = content;
        }
    }
    
    [CustomPropertyDrawer(typeof(DisplayNameAttribute))]
    public class DisplayNameDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            DisplayNameAttribute displayName = attribute as DisplayNameAttribute;
            label.text = displayName.displayName;
            EditorGUI.PropertyField(position, property, label, true);
        }
    }
}
