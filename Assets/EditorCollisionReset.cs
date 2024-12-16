using UnityEngine;

[ExecuteInEditMode]
public class EditorCollisionPreventOverlap : MonoBehaviour
{
    private BoxCollider boxCollider;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        if (!Application.isPlaying)
        {
            PreventOverlap();
        }
    }

    void PreventOverlap()
    {
        if (boxCollider == null) return;

        // 获取碰撞体的世界坐标大小和中心
        Vector3 size = boxCollider.size;
        Vector3 center = transform.TransformPoint(boxCollider.center);

        Collider[] colliders = Physics.OverlapBox(center, size / 2, transform.rotation);

        foreach (var collider in colliders)
        {
            if (collider.gameObject != gameObject && collider.CompareTag("Box"))
            {
                Vector3 direction = transform.position - collider.transform.position;
                Vector3 offset = Vector3.zero;

                float overlapX = (size.x / 2 + collider.bounds.size.x / 2) - Mathf.Abs(direction.x);
                float overlapZ = (size.z / 2 + collider.bounds.size.z / 2) - Mathf.Abs(direction.z);

                if (overlapX < overlapZ)
                {
                    offset.x = overlapX * Mathf.Sign(direction.x);
                }
                else
                {
                    offset.z = overlapZ * Mathf.Sign(direction.z);
                }

                transform.position += offset;
                break;
            }
        }
    }
}
