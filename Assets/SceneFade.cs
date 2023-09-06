using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneFade : MonoBehaviour
{
    private Material fadeMaterial;
    // Start is called before the first frame update
    void Start()
    {
        fadeMaterial = GetComponent<MeshRenderer>().material;
        FadeIn();
    }


    public void FadeIn()
    {
        StartCoroutine(InwardFade());
    }

    IEnumerator InwardFade()
    {
        //fade in over 1 second
        for (float i = 1; i >= 0; i -= Time.deltaTime)
        {
            Color c = new Color(0, 0, 0, i);
            fadeMaterial.color = c;
            yield return null;
        }
        Color col = new Color(0, 0, 0, 0);
        fadeMaterial.color = col;
    }

    public void FadeOut()
    {
        StartCoroutine(OutwardFade());
    }

    IEnumerator OutwardFade()
    {
        //fade out over 1 second
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            Color c = new Color(0, 0, 0, i);
            fadeMaterial.color = c;
            yield return null;
        }
        Color col = new Color(0, 0, 0, 1);
        fadeMaterial.color = col;
    }
}
