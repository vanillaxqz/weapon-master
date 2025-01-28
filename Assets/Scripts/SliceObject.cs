using UnityEngine;
using EzySlice;
using System.Collections;
using System.Collections.Generic;
using TMPro; // Import TextMeshPro

public class SliceObject : MonoBehaviour
{
    public Transform startSlicePoint;
    public Transform endSlicePoint;
    public VelocityEstimator velocityEstimator;
    public LayerMask sliceableLayer;
    public float cutForce = 2000f;
    public GameObject objectPrefab; // Assign the prefab in the inspector
    public TextMeshPro worldText; // Assign the existing world-space TextMeshPro object in Unity Inspector

    public List<GameObject> pObjects = new List<GameObject>(); // List for p1 to p5 (3D planes)

    void FixedUpdate()
    {
        bool hasHit = Physics.Linecast(startSlicePoint.position, endSlicePoint.position, out RaycastHit hit, sliceableLayer);
        if (hasHit)
        {
            GameObject target = hit.transform.gameObject;

            // Compute the slicing plane normal
            Vector3 velocity = velocityEstimator.GetVelocityEstimate();
            Vector3 sliceDirection = (endSlicePoint.position - startSlicePoint.position).normalized;
            Vector3 slicePlaneNormal = Vector3.Cross(sliceDirection, velocity).normalized;

            // Get active pObject for accuracy comparison
            GameObject activePObject = GetActivePObject();
            if (activePObject != null)
            {
                Vector3 pObjectNormal = GetPlaneNormal(activePObject);
                float angleDifference = ComputeAngleDifference(slicePlaneNormal, pObjectNormal);
                int accuracyScore = ComputeAccuracyScore(angleDifference);

                Debug.Log($"Slice Accuracy Score: {accuracyScore}/10 | Angle Difference: {angleDifference}° | Active Plane: {activePObject.name}");

                // Update the world text display with the score
                UpdateWorldText($"Accuracy: {accuracyScore}/10", hit.point);
            }
            else
            {
                Debug.LogWarning("No active pObject found! Accuracy score not computed.");
            }

            // Proceed with slicing
            Slice(target, slicePlaneNormal);
        }
    }

    public void Slice(GameObject target, Vector3 slicePlaneNormal)
    {
        SlicedHull hull = target.Slice(endSlicePoint.position, slicePlaneNormal);
        if (hull != null)
        {
            // Store object state before destruction
            Vector3 originalPosition = target.transform.position;
            Quaternion originalRotation = target.transform.rotation;
            int originalLayer = target.layer;

            // Create upper and lower hulls
            GameObject upperHull = hull.CreateUpperHull(target, target.GetComponent<Renderer>().material);
            GameObject lowerHull = hull.CreateLowerHull(target, target.GetComponent<Renderer>().material);

            SetupSlicedComponent(upperHull);
            SetupSlicedComponent(lowerHull);

            // Destroy the original object
            Destroy(target);

            // Start coroutine to destroy hulls after 5 seconds
            StartCoroutine(DestroyHulls(upperHull, lowerHull));

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

    private IEnumerator DestroyHulls(GameObject upperHull, GameObject lowerHull)
    {
        yield return new WaitForSeconds(5f);

        if (upperHull != null) Destroy(upperHull);
        if (lowerHull != null) Destroy(lowerHull);
    }

    private IEnumerator RespawnOriginal(Vector3 position, Quaternion rotation, int layer)
    {
        yield return new WaitForSeconds(2f);

        GameObject respawnedObject = Instantiate(objectPrefab, position, rotation);
        respawnedObject.layer = layer;
    }

    private GameObject GetActivePObject()
    {
        foreach (GameObject pObject in pObjects)
        {
            if (pObject != null && pObject.activeSelf)
            {
                return pObject;
            }
        }
        return null;
    }

    private Vector3 GetPlaneNormal(GameObject plane)
    {
        return plane.transform.up.normalized; // Correct orientation for planes
    }

    private int ComputeAccuracyScore(float angleDifference)
    {
        if (angleDifference < 5f) return 10;  // Perfectly aligned
        if (angleDifference < 10f) return 9;
        if (angleDifference < 15f) return 8;
        if (angleDifference < 20f) return 7;
        if (angleDifference < 30f) return 6;
        if (angleDifference < 40f) return 5;
        if (angleDifference < 50f) return 4;
        if (angleDifference < 60f) return 3;
        if (angleDifference < 70f) return 2;
        return 1;  // Almost perpendicular
    }

    private float ComputeAngleDifference(Vector3 normal1, Vector3 normal2)
    {
        float dotProduct = Mathf.Clamp(Vector3.Dot(normal1, normal2), -1f, 1f);
        return Mathf.Acos(dotProduct) * Mathf.Rad2Deg;
    }

    private void UpdateWorldText(string text, Vector3 position)
    {
        if (worldText != null)
        {
            worldText.text = text;
        }
        else
        {
            Debug.LogWarning("World Text is not assigned in the Inspector.");
        }
    }
}
