using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateBGIsland : MonoBehaviour
{

    [SerializeField]
    private GameObject[] topDecos;
    [SerializeField]
    private GameObject aboveRocks;
    [SerializeField]
    private GameObject belowRocks;

    // Start is called before the first frame update
    void Start()
    {
        Generate();
    }

    private void Generate()
    {
        GameObject go = Instantiate(aboveRocks, transform);
        go.transform.parent = transform;
        go.transform.localPosition = Vector3.zero;
        go.transform.Rotate(new Vector3(0f, 90f * Random.Range(0, 4), 0f));

        GameObject go2 = Instantiate(belowRocks, transform);
        go2.transform.parent = transform;
        go2.transform.localPosition = Vector3.zero;
        go2.transform.Rotate(new Vector3(0f, 90f * Random.Range(0, 4), 0f));

        GameObject go3 = Instantiate(topDecos[Random.Range(0, topDecos.Length)], transform);
        go3.transform.parent = transform;
        go3.transform.localPosition = Vector3.zero;
        go3.transform.GetChild(1).Rotate(new Vector3(0f, 90f * Random.Range(0, 4), 0f));
    }
}
