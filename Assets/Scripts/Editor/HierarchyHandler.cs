using UnityEditor;
using UnityEngine;

namespace DCEditor
{
    [InitializeOnLoad]
    public class HierarchyHandler
    {
        static HierarchyHandler()
        {
            EditorApplication.hierarchyChanged += OnHierarchyChanged;
        }

        private static void OnHierarchyChanged()
        {
            DCEditorMgr.Instance.RefreshInspector();
        }
    }
}
