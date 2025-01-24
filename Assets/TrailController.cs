using UnityEngine;

public class TrailController : MonoBehaviour
{
    public ParticleSystem currentTrail;
    public float speedThreshold;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (currentTrail != null)
        {
            currentTrail.Stop();
        }
   
    }

    private void Update()
    {
        float speed = rb.linearVelocity.magnitude;
        if (speed > speedThreshold)
        {
            Debug.Log("Speed:" + speed.ToString() + " | " + speedThreshold.ToString());

            // Enable trail
            if (currentTrail != null)
            {
                currentTrail.Play();
            }
        }
        else
        {   // Disable trail
            if (currentTrail != null)
            {
                currentTrail.Stop();
            }
        }
    }
}
