using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System.Collections;

public class SwingDemoController : MonoBehaviour
{
    public void PlayKatanaAnimation()
    {
        StartCoroutine(PlayKatanaAnimationCoroutine());
    }

    private IEnumerator PlayKatanaAnimationCoroutine()
    {
        if (gameObject != null)
        {
            gameObject.SetActive(true); // Enable the object
            Animator animator = gameObject.GetComponent<Animator>();
            if (animator != null)
            {
                animator.Play("katana|Swing");
            }

            yield return new WaitForSeconds(1f);

            gameObject.SetActive(false); // Hide the object
        }
    }
}
