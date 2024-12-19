using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class RHandAnimation : MonoBehaviour
{
    [SerializeField] private InputActionProperty grabButton;
    private Animator animator;
    private bool onceGrabbed = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (grabButton.action.WasPressedThisFrame())
        {
            Debug.Log("Right hand grabbing");
            if (onceGrabbed)
            {
                Debug.Log("onceGrabbed");
                animator.ResetTrigger("TrGrabStop");
                animator.SetTrigger("TrGrab");
                onceGrabbed = false;
            }
        }
        else if(grabButton.action.WasReleasedThisFrame())
        {
            Debug.Log("NOT onceGrabbed");
            onceGrabbed = true;
            animator.ResetTrigger("TrGrab");
            animator.SetTrigger("TrGrabStop");
        }
    }
}
