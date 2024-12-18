using UnityEngine;

public class Real_Katana_Hit : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AudioSource hitSfx;
    [SerializeField] private ParticleSystem basicHitParticle;

    [Header("Settings")]
    [SerializeField] private float cooldown = 0.5f;

    private float lastHitTime = -Mathf.Infinity;

    private void OnCollisionEnter(Collision collision)
    {

        if (Time.time - lastHitTime < cooldown)
        {
            return;
        }

        if (collision.contactCount > 0)
        {
            Vector3 hitPoint = collision.contacts[0].point;

            if (basicHitParticle != null)
            {
                basicHitParticle.transform.position = hitPoint;
                basicHitParticle.Play();
            }

            if (hitSfx != null)
            {
                hitSfx.transform.position = hitPoint;
                hitSfx.Play();
            }

            lastHitTime = Time.time;
        }
    }
}
