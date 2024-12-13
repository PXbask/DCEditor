using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject objectPrefab; // 预制体
    public int X = 5; // 要生成的物体数量
    public int Z = 5; // 要生成的物体数量

     void OnEnable()
    {
        if (objectPrefab == null)
        {
            objectPrefab = Resources.Load<GameObject>("domino");
        }
    }

    public void CreateX()
    {

        Vector3 spacing = objectPrefab.GetComponent<Renderer>().bounds.extents;

        // 在指定轴上生成多个物体
        for (int i = 0; i < X; i++)
        {
            if (Mathf.Approximately(objectPrefab.transform.eulerAngles.y, 90f))
            {
                Vector3 positionOffset = new Vector3((i+1) * (spacing.z+16), 0, 0);
                Instantiate(objectPrefab, transform.position + positionOffset, transform.rotation, transform.parent);
            }
            else{
                Vector3 positionOffset = new Vector3((i+1) * (spacing.x+7), 0, 0);
                Instantiate(objectPrefab, transform.position + positionOffset, transform.rotation, transform.parent);
            }
            
        }
    }
    public void CreateZ()
    {
        Vector3 spacing = objectPrefab.GetComponent<Renderer>().bounds.extents;

        for (int i = 0; i < Z; i++)
        {
            // Vector3 positionOffset = new Vector3(0, 0, (i+1) * (spacing.x+16));
            // Instantiate(objectPrefab, transform.position + positionOffset, transform.rotation, transform.parent);

            if (Mathf.Approximately(objectPrefab.transform.eulerAngles.y, 90f))
            {
                Vector3 positionOffset = new Vector3(0, 0,(i+1) * (spacing.x+2));
                Instantiate(objectPrefab, transform.position + positionOffset, transform.rotation, transform.parent);
            }
            else{
                Vector3 positionOffset = new Vector3(0, 0, (i+1) * (spacing.x+16));
                Instantiate(objectPrefab, transform.position + positionOffset, transform.rotation, transform.parent);
            }
        }
    }
}
