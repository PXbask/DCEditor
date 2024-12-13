using UnityEngine;

public class BoxColliderSeparation : MonoBehaviour
{
    public float separationForce = 10.0f; // 分离的力度

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Box"))
        {
            Rigidbody rbOther = other.GetComponent<Rigidbody>();
            Rigidbody rbThis = GetComponent<Rigidbody>();

            if (rbOther != null && rbThis != null)
            {
                Vector3 direction = (other.transform.position - transform.position).normalized;
                rbOther.AddForce(direction * separationForce);
                rbThis.AddForce(-direction * separationForce);
            }
        }
    }
}
