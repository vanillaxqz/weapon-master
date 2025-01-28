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
            Debug.Log("ANGLE: " + angle.ToString() + " | FILL AMOUNT: " + fillAmount.ToString());
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
            iconRectTransform.localScale = spawnedRadialPart.transform.localScale;
            iconRectTransform.rotation = spawnedRadialPart.transform.rotation;
            iconRectTransform.position = spawnedRadialPart.transform.position;
            iconRectTransform.anchoredPosition = new Vector2(58.6f, 102.7f);
            iconRectTransform.sizeDelta = new Vector2(75, 75);
            spawnedParts.Add(spawnedRadialPart);
        }
    }

    
}
