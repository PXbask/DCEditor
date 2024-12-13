using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class MirrorChildren : MonoBehaviour
{
    public void Mirror()
    {
        // 检查是否在编辑模式下运行
        if (!Application.isPlaying)
        {
            // Debug.Log("22222");
            // 遍历所有子物体
            foreach (Transform child in transform)
            {
                Debug.Log($"transform: {child.transform.position}");
                // 创建子物体的镜像副本
                // Vector3 positionOffset = child.transform;
                GameObject mirroredChild = Instantiate(child.gameObject, 
                new Vector3(child.position.x*-1,child.position.y,child.position.z), 
                Quaternion.identity,
                transform.parent);
            }
        }
    }
}
