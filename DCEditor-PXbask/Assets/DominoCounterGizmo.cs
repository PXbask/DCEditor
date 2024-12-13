using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class DominoCounterGizmo : MonoBehaviour
{
    void OnDrawGizmos()
    {
        int dominoCount = CountDominoObjects();

        // 设置 Gizmo 的颜色
        Gizmos.color = Color.red;

        // 在场景中绘制文本
        Handles.Label(transform.position, $"{dominoCount}", new GUIStyle
        {
            fontSize = 20,
            normal = new GUIStyleState { textColor = Color.red }
        });
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
}
