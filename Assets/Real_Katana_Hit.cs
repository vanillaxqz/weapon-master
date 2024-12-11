using UnityEngine;

public class Real_Katana_Hit : MonoBehaviour
{
    public AudioSource hitSfx;
    public GameObject basicHitParticlePrefab;

    void Start()
    {
        hitSfx = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hitSfx != null)
        {
            hitSfx.Play();
        }

        if (basicHitParticlePrefab != null)
        {
            Vector3 collisionPoint = collision.contacts[0].point;
            Instantiate(basicHitParticlePrefab, collisionPoint, Quaternion.identity);
        }
    }
}