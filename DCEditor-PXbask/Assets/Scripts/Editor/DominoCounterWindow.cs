using UnityEditor;
using UnityEngine;

public class DominoCounterWindow : EditorWindow
{
    [MenuItem("Window/Domino Counter")]
    public static void ShowWindow()
    {
        GetWindow<DominoCounterWindow>("Domino Counter");
    }

    void OnGUI()
    {
        GUILayout.Label("Domino Objects in Scene", EditorStyles.boldLabel);

        int dominoCount = CountDominoObjects();
        GUILayout.Label($"Number of Domino Objects: {dominoCount}");
    }

    int CountDominoObjects()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        int count = 0;

        foreach (GameObject obj in allObjects)
        {
            if (obj.name.Contains("domino"))
            {
                count++;
            }
        }

        return count;
    }

    void Update()
    {
        // 强制重绘窗口以更新显示
        Repaint();
    }
}
