using UnityEngine;

public class ParentCollisionDetector : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        foreach (var contact in collision.contacts)
        {
            Collider childCollider = contact.thisCollider;

            if (childCollider != null)
            {
                Debug.Log("Child collided: " + childCollider.gameObject.name);
            }
        }
    }
}
