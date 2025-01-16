using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.UI;

public class DisplayWeaponInfo : MonoBehaviour
{
    public Sprite[] weaponInfoImgs;
    public InputActionReference hintSpawnButton;
    public GameObject katana;
    public GameObject naginata;
    public Transform handTransform;

    private XRGrabInteractable katanaGrabInteractable;
    private XRGrabInteractable naginataGrabInteractable;
    private bool isKatanaHeld = false;
    private bool isNaginataHeld = false;
    private bool isKatanaInfoSpawned = false;
    private bool isNaginataInfoSpawned = false;

    void Start()
    {
        katanaGrabInteractable = katana.GetComponent<XRGrabInteractable>();
        katanaGrabInteractable.selectEntered.AddListener(OnKatanaGrabbed);
        katanaGrabInteractable.selectExited.AddListener(OnKatanaReleased);

        naginataGrabInteractable = naginata.GetComponent<XRGrabInteractable>();
        naginataGrabInteractable.selectEntered.AddListener(OnNaginataGrabbed);
        naginataGrabInteractable.selectExited.AddListener(OnNaginataReleased);
    }

    void Update()
    {
        if (hintSpawnButton.action.WasPressedThisFrame())
        {
            Debug.Log("hintSpawnButton WasPressedThisFrame");
            if (isKatanaHeld && !isKatanaInfoSpawned)
            {
                GameObject katanaImageObject = new GameObject("Katana Info Image");
                katanaImageObject.transform.SetParent(gameObject.transform, false);

                Image katanaImageComponent = katanaImageObject.AddComponent<Image>();
                katanaImageComponent.sprite = weaponInfoImgs[0];

                RectTransform rectTransform = katanaImageObject.GetComponent<RectTransform>();
                rectTransform.anchorMin = Vector2.zero;
                rectTransform.anchorMax = Vector2.one;
                rectTransform.localPosition = handTransform.localPosition;

                isKatanaInfoSpawned = true;
            }
            else if ((isKatanaHeld && isKatanaInfoSpawned) || !isKatanaHeld)
            {
                var katanaImageObject = GameObject.Find("Katana Info Image");
                if (katanaImageObject != null)
                {
                    Destroy(katanaImageObject);
                }

                isKatanaInfoSpawned = false;
            }

            if (isNaginataHeld && !isNaginataInfoSpawned)
            {
                GameObject naginataImageObject = new GameObject("Naginata Info Image");
                naginataImageObject.transform.SetParent(gameObject.transform, false);

                Image naginataImageComponent = naginataImageObject.AddComponent<Image>();
                naginataImageComponent.sprite = weaponInfoImgs[1];

                RectTransform rectTransform = naginataImageObject.GetComponent<RectTransform>();
                rectTransform.anchorMin = Vector2.zero;
                rectTransform.anchorMax = Vector2.one;
                rectTransform.localPosition = handTransform.localPosition;

                isNaginataInfoSpawned = true;
            }
            else if ((isNaginataHeld && isNaginataInfoSpawned) || !isNaginataHeld)
            {
                var naginataImageObject = GameObject.Find("Naginata Info Image");
                if (naginataImageObject != null)
                {
                    Destroy(naginataImageObject);
                }

                isNaginataInfoSpawned = false;
            }
        }
    }


    private void OnKatanaGrabbed(SelectEnterEventArgs args)
    {
        isKatanaHeld = true;
    }

    private void OnKatanaReleased(SelectExitEventArgs args)
    {
        isKatanaHeld = false;
    }

    public bool IsKatanaHeld()
    {
        return isKatanaHeld;
    }

    private void OnNaginataGrabbed(SelectEnterEventArgs args)
    {
        isNaginataHeld = true;
    }

    private void OnNaginataReleased(SelectExitEventArgs args)
    {
        isNaginataHeld = false;
    }

    public bool IsNaginataHeld()
    {
        return isNaginataHeld;
    }

}
