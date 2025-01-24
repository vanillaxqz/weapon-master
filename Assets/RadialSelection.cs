using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.PostProcessing;
using static Unity.VisualScripting.Metadata;

public class RadialSelection : MonoBehaviour
{
    public InputActionReference selectionSpawnButton;
    public int numberRadialParts;
    public GameObject radialPartPrefab;
    public Transform radialPartCanvas;
    public float angleBetweenPart = 10;
    public Transform handTransform;
    public UnityEvent<int> onPartSelected;
    public float transparency = 0.5f;
    public Sprite[] radialPartIcons;

    private List<GameObject> spawnedParts = new List<GameObject>();
    private int currentSelectedRadialPart = -1;

    void Start()
    {
        
    }

    void Update()
    {
        if (selectionSpawnButton.action.WasPressedThisFrame())
        {
            Debug.Log("Radial Selection Spawned");
            SpawnRadialPart();
        }
        if (selectionSpawnButton.action.IsPressed())
        {
            GetSelectedRadialPart();
            Debug.Log("Selection Part Selected");
        }
        if (selectionSpawnButton.action.WasReleasedThisFrame())
        {
            HideAndTriggerSelected();
            Debug.Log("Radial Selection Hidden");
        }
    }

    public void HideAndTriggerSelected()
    {
        onPartSelected.Invoke(currentSelectedRadialPart);
        radialPartCanvas.gameObject.SetActive(false);
    }

    public void GetSelectedRadialPart()
    {
        Vector3 centerToHand = handTransform.position - radialPartCanvas.position;
        Vector3 centerToHandProjected = Vector3.ProjectOnPlane(centerToHand, radialPartCanvas.forward);

        float angle = Vector3.SignedAngle(radialPartCanvas.up, centerToHandProjected, -radialPartCanvas.forward);
        if (angle < 0)
        {
            angle += 360;
        }
        currentSelectedRadialPart = (int)angle * numberRadialParts / 360;

        for (int i = 0; i < spawnedParts.Count; ++i)
        {
            if(i == currentSelectedRadialPart)
            {
                spawnedParts[i].GetComponent<Image>().color = Color.blue;
                spawnedParts[i].transform.localScale = 1.1f * Vector3.one;
            }
            else
            {
                spawnedParts[i].GetComponent<Image>().color = Color.white;
                spawnedParts[i].transform.localScale = Vector3.one;
            }
        }
    }

    public void SpawnRadialPart()
    {
        radialPartCanvas.gameObject.SetActive(true);
        radialPartCanvas.position = handTransform.position;
        radialPartCanvas.rotation = handTransform.rotation;

        foreach (var item in spawnedParts)
        {
            Destroy(item);
        }
        spawnedParts.Clear();

        for (int i = 0; i < numberRadialParts; i++)
        {
            float angle = -i * 360 / numberRadialParts - angleBetweenPart / 2;
            Vector3 radialPartEulerAngle = new Vector3(0, 0, angle);
            GameObject spawnedRadialPart = Instantiate(radialPartPrefab, radialPartCanvas);
            spawnedRadialPart.transform.position = radialPartCanvas.position;
            spawnedRadialPart.transform.localEulerAngles = radialPartEulerAngle;

            Image imageComponent = spawnedRadialPart.GetComponent<Image>();
            float fillAmount = (1 / (float)numberRadialParts) - (angleBetweenPart / 360);
            imageComponent.fillAmount = fillAmount;

            RectTransform radialRectTransform = spawnedRadialPart.GetComponent<RectTransform>();
            //Vector3 visualCenter = CalculateAdjustedVisualCenter(radialRectTransform, fillAmount, 0.71f);

            GameObject radialPartIcon = new GameObject("Radial Part Icon", typeof(Image));

            radialPartIcon.transform.SetParent(spawnedRadialPart.transform);
            Image image = radialPartIcon.GetComponent<Image>();
            image.sprite = radialPartIcons[0];
            RectTransform iconRectTransform = radialPartIcon.GetComponent<RectTransform>();
            iconRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            iconRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            iconRectTransform.anchoredPosition = Vector2.zero;
            iconRectTransform.pivot = new Vector2(0.5f, 0.5f);
            //radialPartIcon.transform.localPosition = visualCenter;
            //radialPartIcon.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
            //radialPartIcon.transform.localRotation = Quaternion.identity;
            /*RectTransform iconRectTransform = radialPartIcon.GetComponent<RectTransform>();
            iconRectTransform.anchorMin = new Vector2(1, 1);
            iconRectTransform.anchorMax = new Vector2(1, 1);*/

            spawnedParts.Add(spawnedRadialPart);
        }
    }

    Vector3 CalculateAdjustedVisualCenter(RectTransform rectTransform, float fillAmount, float weight)
    {
        // Assume the image is circular and uses radial fill
        // Get the radius of the RectTransform
        float radius = rectTransform.rect.width / 2f;

        // Calculate the angle in radians (middle of the filled arc)
        float angle = (fillAmount / 2f) * Mathf.PI * 2f;

        // Convert polar coordinates to Cartesian coordinates
        float x = Mathf.Sin(angle) * radius;
        float y = Mathf.Cos(angle) * radius;

        // Local position of the visual center
        Vector3 localVisualCenter = new Vector3(x, y, 0);

        // Weighted adjustment towards the original center
        Vector3 adjustedCenter = Vector3.Lerp(rectTransform.position, rectTransform.position + localVisualCenter, weight);

        return adjustedCenter;
    }
}
