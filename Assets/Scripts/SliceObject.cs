using UnityEngine;
using EzySlice;
using System.Collections;

public class SliceObject : MonoBehaviour
{
    public Transform startSlicePoint;
    public Transform endSlicePoint;
    public VelocityEstimator velocityEstimator;
    public LayerMask sliceableLayer;
    public float cutForce = 2000f;
    public GameObject objectPrefab; // Assign the prefab in the inspector

    void FixedUpdate()
    {
        bool hasHit = Physics.Linecast(startSlicePoint.position, endSlicePoint.position, out RaycastHit hit, sliceableLayer);
        if (hasHit)
        {
            GameObject target = hit.transform.gameObject;
            Slice(target);
        }
    }

    public void Slice(GameObject target)
    {
        Vector3 velocity = velocityEstimator.GetVelocityEstimate();
        Vector3 planeNormal = Vector3.Cross(endSlicePoint.position - startSlicePoint.position, velocity);
        planeNormal.Normalize();

        SlicedHull hull = target.Slice(endSlicePoint.position, planeNormal);

        if (hull != null)
        {
            // Store object state before destruction
            Vector3 originalPosition = target.transform.position;
            Quaternion originalRotation = target.transform.rotation;
            int originalLayer = target.layer;

            // Create upper and lower hulls
            GameObject upperHull = hull.CreateUpperHull(target, target.GetComponent<Renderer>().material);
            SetupSlicedComponent(upperHull);

            GameObject lowerHull = hull.CreateLowerHull(target, target.GetComponent<Renderer>().material);
            SetupSlicedComponent(lowerHull);

            // Destroy the original object
            Destroy(target);

            // Respawn the original object after 2 seconds using the prefab
            StartCoroutine(RespawnOriginal(originalPosition, originalRotation, originalLayer));
        }
    }

    public void SetupSlicedComponent(GameObject slicedObject)
    {
        Rigidbody rb = slicedObject.AddComponent<Rigidbody>();
        MeshCollider mc = slicedObject.AddComponent<MeshCollider>();
        mc.convex = true;
        rb.AddExplosionForce(cutForce, slicedObject.transform.position, 1);
    }

    private IEnumerator RespawnOriginal(Vector3 position, Quaternion rotation, int layer)
    {
        // Wait for 2 seconds
        yield return new WaitForSeconds(2f);

        // Instantiate a new object from the prefab
        GameObject respawnedObject = Instantiate(objectPrefab, position, rotation);
        respawnedObject.layer = layer;
    }
}
