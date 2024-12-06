using UnityEngine;

public class Real_Katana_Hit : MonoBehaviour
{
    private AudioSource audioSource;
    public GameObject particlePrefab;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //to add separate sound for hitting table,rock,metal
        audioSource.Play();
        Vector3 collisionPoint = collision.contacts[0].point;
        Instantiate(particlePrefab, collisionPoint, Quaternion.identity);
    }
}
