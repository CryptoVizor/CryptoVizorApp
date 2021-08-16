using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;


public class cardAnimation : MonoBehaviour
{
    public Material material;
    public Material startMaterial;

    public Material originalMaterial;
    public string status = "start";
    private float deletionFloat = 1.0f;

    public VisualEffect effect;
    // Update is called once per frame
    void Update()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        switch (status)
        {
            case "delete":
                if (renderer.material.Equals(material) == false)
                {
                    renderer.material = material;
                }
                deletionFloat = Mathf.Clamp01(deletionFloat - (0.8f * Time.deltaTime));
                material.SetFloat("_deletionFloat",deletionFloat);
                if (deletionFloat.Equals(0.0f))
                {
                    gameObject.transform.parent.gameObject.SetActive(false);
                }
                break;
            default:
                break;
        }
    }
}
