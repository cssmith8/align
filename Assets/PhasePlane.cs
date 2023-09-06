using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhasePlane : MonoBehaviour
{
    //shader
    [SerializeField] private Shader shader;
    private Material material;

    // Start is called before the first frame update
    void Start()
    {
        //create material with shader
        material = new Material(shader);
        //set material
        GetComponent<MeshRenderer>().material = material;

        material.SetFloat("_glow", 0f);
        material.SetFloat("_xScale", transform.parent.localScale.x);
        material.SetFloat("_yScale", transform.parent.localScale.z);

    }

    private void Update()
    {
        
    }

    public void GlowActivate()
    {
        StartCoroutine(Activate());
    }

    IEnumerator Activate(float seconds = 1)
    {
        
        float t = Time.timeSinceLevelLoad;
        while (Time.timeSinceLevelLoad - t < 1f)
        {
            material.SetFloat("_glow", Time.timeSinceLevelLoad - t);
            if (Time.timeSinceLevelLoad - t > 0.5f)
            {
                GetComponent<MeshCollider>().enabled = false;
            }
            yield return null;
        }
        //wait seconds
        yield return new WaitForSeconds(seconds);
        //reset
        t = Time.timeSinceLevelLoad;
        while (Time.timeSinceLevelLoad - t < 1f)
        {
            material.SetFloat("_glow", 1f - (Time.timeSinceLevelLoad - t));
            if (Time.timeSinceLevelLoad - t > 0.5f)
            {
                GetComponent<MeshCollider>().enabled = true;
            }
            yield return null;
        }
        material.SetFloat("_glow", 0f);
        GetComponent<MeshCollider>().enabled = true;
    }
}
