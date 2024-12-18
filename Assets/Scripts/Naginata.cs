using UnityEngine;

public class Naginata : MonoBehaviour
{
    private AudioSource audioSource;
    public GameObject particlePrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //to add separate sound for hitting table,rock,metal
        audioSource.Play();
        Vector3 collisionPoint = collision.contacts[0].point;
        GameObject particle = Instantiate(particlePrefab, collisionPoint, Quaternion.identity);
    }
}
