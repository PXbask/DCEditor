using UnityEditor;
using UnityEngine;

namespace DCEditor
{
    [InitializeOnLoad]
    public class PreventObjectDeletion
    {
        static PreventObjectDeletion()
        {
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyWindowItemOnGUI;
        }

        static void OnHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
        {
            Event currentEvent = Event.current;
            if (currentEvent.type == EventType.KeyDown && currentEvent.keyCode == KeyCode.Delete)
            {
                // 阻止删除操作
                currentEvent.Use();
                EditorUtility.DisplayDialog("警告", "你需要通过Inspector中的Delete删除此物体", "我再也不敢了");
            }
        }
    }
}
