using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    [SerializeField] private InputActionProperty jumpButton;
    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private CharacterController cc;
    [SerializeField] private LayerMask groundLayers;

    private float gravity = Physics.gravity.y;
    private Vector3 movement;

    private bool IsGrounded()
    {
        return Physics.CheckSphere(transform.position, 2.0f, groundLayers);
    }
    
    /*void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + Vector3.down * 0.5f, 1.0f);
    }*/

    void Update()
    {
        bool isPlayerGrounded = IsGrounded();
        /*if (jumpButton.action.WasPressedThisFrame())
        {
            Debug.Log("jumpButton pressed");
        }*/

        if (jumpButton.action.WasPressedThisFrame() && isPlayerGrounded)
        {
            Jump();
            Debug.Log("JUMP");
        }
        movement.y += gravity * Time.deltaTime;
        cc.Move(movement * Time.deltaTime);
    }

    private void Jump()
    {
        movement.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
    }
}
