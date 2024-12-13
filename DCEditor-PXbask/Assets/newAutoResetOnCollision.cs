using UnityEngine;

[ExecuteInEditMode]
public class AutoResetOnCollision : MonoBehaviour
{
    private Vector3 initialPosition;

    void Start()
    {
        // 记录初始位置
        initialPosition = transform.position;
    }

    void Update()
    {
        if (!Application.isPlaying)
        {
            CheckCollisionAndReset();
        }
    }

    void CheckCollisionAndReset()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, transform.localScale / 2, transform.rotation);

        foreach (var collider in colliders)
        {
            if (collider.gameObject != gameObject && collider.CompareTag("Box"))
            {
                Vector3 direction = transform.position - collider.transform.position;
                if (Mathf.Abs(direction.x) < collider.bounds.size.x  && Mathf.Abs(direction.z) < collider.bounds.size.z )
                {
                    ResetPosition();
                    break;
                }
            }
        }
    }

    void ResetPosition()
    {
        transform.position = initialPosition;
    }
}
