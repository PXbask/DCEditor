using UnityEngine;

[ExecuteInEditMode]
public class AttractionNode : MonoBehaviour
{
    public GameObject nodePrefab; // 节点的预制体
    public int numberOfNodes = 5; // 节点的数量
    public float attractionRadius = 1.0f; // 吸附的半径
    public float snapSpeed = 5.0f; // 吸附的速度

    private Transform[] nodes;

    void Start()
    {
        if (!Application.isPlaying)
        {
            CreateNodes();
        }
    }

    void CreateNodes()
    {
        // 清除旧节点
        if (nodes != null)
        {
            foreach (Transform node in nodes)
            {
                if (node != null)
                    DestroyImmediate(node.gameObject);
            }
        }

        nodes = new Transform[numberOfNodes];
        for (int i = 0; i < numberOfNodes; i++)
        {
            Vector3 position = new Vector3(
                Random.Range(-transform.localScale.x / 2, transform.localScale.x / 2),
                Random.Range(-transform.localScale.y / 2, transform.localScale.y / 2),
                0
            );

            GameObject node = Instantiate(nodePrefab, transform);
            node.transform.localPosition = position;
            nodes[i] = node.transform;
        }
    }

    void Update()
    {
        if (!Application.isPlaying)
        {
            foreach (Transform node in nodes)
            {
                if (node == null) continue;

                Collider2D[] colliders = Physics2D.OverlapCircleAll(node.position, attractionRadius);
                foreach (Collider2D collider in colliders)
                {
                    if (collider.CompareTag("Box") )
                    {
                        Vector3 direction = (node.position - collider.transform.position).normalized;
                        collider.transform.position = Vector3.MoveTowards(
                            collider.transform.position,
                            node.position,
                            snapSpeed * Time.deltaTime
                        );
                    }
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        if (nodes != null)
        {
            Gizmos.color = Color.cyan;
            foreach (Transform node in nodes)
            {
                if (node != null)
                {
                    Gizmos.DrawWireSphere(node.position, attractionRadius);
                }
            }
        }
    }
}
