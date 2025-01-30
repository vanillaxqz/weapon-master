using UnityEngine;

public class ConfettiController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ParticleSystem particles = GetComponent<ParticleSystem>();
        particles.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
