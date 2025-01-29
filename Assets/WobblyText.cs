using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class WobblyText : MonoBehaviour
{
    public TMP_Text textComponent;
    public Transform player;
    [Range(0.01f, 1f)]
    public float wobbliness;
    [Range(0.01f, 5f)]
    public float jumpHeight;

    // Update is called once per frame
    void Update()
    {
        textComponent.ForceMeshUpdate();
        var textInfo = textComponent.textInfo;
        for (int i = 0; i < textInfo.characterCount; i++)
        {
            var charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible)
            {
                continue;
            }

            var verts = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;
            for (int j = 0; j < 4; ++j)
            {
                var origin = verts[charInfo.vertexIndex + j];
                verts[charInfo.vertexIndex + j] = origin + new Vector3(0, Mathf.Sin(Time.time * 2f + ((i + 1) * wobbliness) * 0.1f) * jumpHeight, 0);
            }
        }
        for (int i = 0; i < textInfo.meshInfo.Length; ++i)
        {
            var meshInfo = textInfo.meshInfo[i];
            meshInfo.mesh.vertices = meshInfo.vertices;
            textComponent.UpdateGeometry(meshInfo.mesh, i);
        }

        Vector3 lookDirection = transform.position - player.position;
        lookDirection.y = 0;
        transform.rotation = Quaternion.LookRotation(lookDirection);
    }
}
