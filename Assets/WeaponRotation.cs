using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class WeaponRotation : MonoBehaviour
{
    public float rotationSpeed = 90f;
    public Vector3 initPosition;
    public Quaternion initRotation;
    private bool canRotate = false;
    private Rigidbody rb;
    private XRGrabInteractable grabInteractable;
    private bool isRepositioned = true;
    private bool turnOnGravity = false;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        grabInteractable = GetComponent<XRGrabInteractable>();
        initPosition = transform.position;
        initRotation = transform.rotation;
        grabInteractable.selectExited.AddListener(OnObjectReleased);
    }
    private void FixedUpdate()
    {
        if (canRotate && !grabInteractable.isSelected)
        {
            if (!isRepositioned)
            {
                transform.position = initPosition;
                transform.rotation = initRotation;
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.useGravity = false;
                rb.isKinematic = true;
                isRepositioned = true;
                Debug.Log("Repositioned");
            }
            Debug.Log("Rotates");
            transform.Rotate(Vector3.up, rotationSpeed * Time.fixedDeltaTime, Space.World);
        }
        /*else if(turnOnGravity)
        {
            rb.useGravity = true;
        }*/
    }
    private void OnObjectReleased(SelectExitEventArgs args)
    {
        Debug.Log("Object Released!");
        rb.isKinematic = false;
        rb.useGravity = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zone"))
        {
            Debug.Log("Entered");
            isRepositioned = false;
            turnOnGravity = false;
            canRotate = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Zone"))
        {
            Debug.Log("Exit");
            canRotate = false;
            turnOnGravity = true;
        }
    }
}