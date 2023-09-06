using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Arrow : MonoBehaviour
{
    public static Arrow Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    [SerializeField]
    private Shader s;
    private Material m;

    private Vector3 vectorTarget = Vector3.zero;
    private GameObject goTarget = null;
    private Vector3 direction = Vector3.up;
    private string targetType = "vector";

    // Start is called before the first frame update
    void Start()
    {
        m = new Material(s);
        transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = m;
        transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().material = m;
        transform.GetChild(2).gameObject.GetComponent<MeshRenderer>().material = m;
        transform.GetChild(3).gameObject.GetComponent<MeshRenderer>().material = m;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 target = (targetType == "vector") ? vectorTarget : goTarget.transform.position;

        if (targetType == "tracking")
        {
            int accuracy = 6;
            float step = 2f;

            target = vectorTarget;

            for (int i = 0; i < accuracy; i++)
            {
                Vector3 addition = step * direction.normalized;

                float lastDistance = Vector3.Distance(target, goTarget.transform.position);

                int countAdd = 0;

                while (true)
                {
                    target += addition;
                    countAdd++;
                    float distance = Vector3.Distance(target, goTarget.transform.position);
                    if (distance > lastDistance)
                    {
                        target -= addition * 2;
                        countAdd -= 2;
                        break;
                    }
                    lastDistance = distance;
                }

                if (countAdd == -1)
                {
                    target += addition;
                }

                step /= 2;
            }
        }
        
        transform.LookAt(target);
        transform.eulerAngles += new Vector3(90, 0, 0);
        transform.localScale = new Vector3(1, Vector3.Distance(transform.position, target), 1);
        
        m.SetFloat("_scale", transform.localScale.y * 2);
    }

    public void SetOrigin(Vector3 origin)
    {
        transform.position = origin;
    }

    public void SetVectorTarget(Vector3 t)
    {
        vectorTarget = t;
        targetType = "vector";
    }

    public void SetGameObjectTarget(GameObject t)
    {
        goTarget = t;
        targetType = "go";
    }

    public void SetDirection(Vector3 dir)
    {
        direction = dir;
    }

    public void SetTracking(Vector3 star, GameObject go)
    {
        vectorTarget = star;
        goTarget = go;
        targetType = "tracking";
    }


    public void FadeOut()
    {
        if (!transform.GetChild(0).gameObject.activeSelf)
        {
            return;
        }
        StopCoroutine(FadeOutCoroutine());
        StopCoroutine(FadeInCoroutine());
        StartCoroutine(FadeOutCoroutine());
    }

    IEnumerator FadeInCoroutine()
    {
        for (int i = 0; i < 4; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            m.SetFloat("_alpha", t);
            yield return null;
        }
    }

    public void FadeIn()
    {
        if (transform.GetChild(0).gameObject.activeSelf)
        {
            return;
        }
        StopCoroutine(FadeOutCoroutine());
        StopCoroutine(FadeInCoroutine());
        StartCoroutine(FadeInCoroutine());
    }

    IEnumerator FadeOutCoroutine()
    {
        float t = 1;
        while (t > 0)
        {
            t -= Time.deltaTime;
            m.SetFloat("_alpha", t);
            yield return null;
        }
        for (int i = 0; i < 4; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
