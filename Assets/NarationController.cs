using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class NarationController : MonoBehaviour
{
    public AudioSource introductionSpeech;
    void Start()
    {
        if (introductionSpeech != null)
        {
            StartCoroutine(WaitSceneLoad());
        }
    }

    IEnumerator WaitSceneLoad()
    {
        yield return new WaitForSeconds(10f);
        introductionSpeech.Play();
    }
}
