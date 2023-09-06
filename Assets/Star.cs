using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    // Start is called before the first frame update
    private PlayerMain pm;

    [SerializeField]
    private Shader s;
    private Material m;

    private float targetEmission;

    private float minSpinSpeed = -60f;
    private float maxSpinSpeed = 60f;

    // The current spin speeds for the collectible item along each axis
    float spinSpeedX, spinSpeedY, spinSpeedZ;

    // The time elapsed since the collectible item was created
    float elapsedTime;

    // The seed value used to generate the perlin noise
    float noiseSeed;


    void Start()
    {
        pm = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMain>();

        m = new Material(s);
        GetComponent<MeshRenderer>().material = m;
        SetEmission(1f);


        //rotation
        // Set the initial spin speeds to the minimum value
        spinSpeedX = minSpinSpeed;
        spinSpeedY = minSpinSpeed;
        spinSpeedZ = minSpinSpeed;

        // Set the elapsed time to 0
        elapsedTime = 0.0f;

        // Generate a random seed value for the perlin noise
        noiseSeed = Random.Range(0.0f, 100.0f);
    }

    // Update is called once per frame
    void Update()
    {
        // Increment the elapsed time
        elapsedTime += Time.deltaTime / 5f;

        // Generate noise values using the elapsed time and noise seed as the input
        float noiseX = Mathf.PerlinNoise(elapsedTime + noiseSeed, 0.0f);
        float noiseY = Mathf.PerlinNoise(0.0f, elapsedTime - noiseSeed);
        float noiseZ = Mathf.PerlinNoise(elapsedTime - noiseSeed, elapsedTime + noiseSeed);

        // Lerp the spin speeds from the minimum to the maximum value using the noise values as the t parameters
        spinSpeedX = Mathf.Lerp(minSpinSpeed, maxSpinSpeed, noiseX);
        spinSpeedY = Mathf.Lerp(minSpinSpeed, maxSpinSpeed, noiseY);
        spinSpeedZ = Mathf.Lerp(minSpinSpeed, maxSpinSpeed, noiseZ);

        // Rotate the collectible item by its spin speeds along each axis every frame
        transform.Rotate(Vector3.right, spinSpeedX * Time.deltaTime);
        transform.Rotate(Vector3.up, spinSpeedY * Time.deltaTime);
        transform.Rotate(Vector3.forward, spinSpeedZ * Time.deltaTime);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.transform.parent.parent.tag != "Leftwand" && other.gameObject.transform.parent.parent.tag != "Rightwand")
        {
            Debug.Log("not a wand");
            return;
        }
        
        int strength = pm.TryStar(gameObject);
        if (strength > 0)
        {
            Music.Instance.PlayNote(strength);
            SetEmission(5f);
        }
        if (strength == 1)
        {
            Arrow.Instance.SetGameObjectTarget(other.gameObject);
            Arrow.Instance.FadeIn();
        }
        if (strength > 1)
        {
            Arrow.Instance.SetTracking(transform.position, other.gameObject);
        }
        
    }

    public void SetEmission(float e)
    {
        StartCoroutine(Emiss(e));
    }

    IEnumerator Emiss(float e)
    {
        float initialEmission = m.GetFloat("_Emission");
        targetEmission = e;

        float initTime = Time.timeSinceLevelLoad;

        while (Time.timeSinceLevelLoad < initTime + 1f)
        {
            if (targetEmission != e) break;
            m.SetFloat("_Emission", Mathf.Lerp(initialEmission, targetEmission, (Time.timeSinceLevelLoad - initTime)));
            yield return null;
        }
        if (targetEmission == e) m.SetFloat("_Emission", e);
    }

}
