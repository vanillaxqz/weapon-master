using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Swing : MonoBehaviour
{
    private Animator mAnimator;
    private XRGrabInteractable grabInteractable;
    private bool isHeld = false;

    void Start()
    {
        mAnimator = GetComponent<Animator>();
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnGrabbed);
        grabInteractable.selectExited.AddListener(OnReleased);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SwingTheKatana()
    {
        if (isHeld)
        {
            mAnimator.SetTrigger("KatanaSwingTrigger");
            Renderer renderer = gameObject.GetComponent<Renderer>();

            if (renderer != null)
            {
                // Получаем материал
                Material material = renderer.material;

                // Устанавливаем режим рендера на Transparent (прозрачный)
                material.SetFloat("_Mode", 3); // 3 соответствует Transparent
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.EnableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;

                // Меняем альфа-канал цвета
                Color color = material.color;
                color.a = 0.5f; // Устанавливаем прозрачность
                material.color = color;
            }
        }
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        isHeld = true;
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        isHeld = false;
    }

    public bool IsHeld()
    {
        return isHeld;
    }
}
