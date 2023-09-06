using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBG : MonoBehaviour
{

    [SerializeField]
    private int numIslands = 50;

    [SerializeField]
    private GameObject island;

    [SerializeField]
    private List<GameObject> placedIslands = new List<GameObject>();

    [SerializeField]
    private Material[] skyboxes;

    [SerializeField]
    private Color[] lightColors;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numIslands; i++)
        {
            NewIsland();
        }

        int environment = SceneData.Instance.levels[SceneData.Instance.level].environment;
        RenderSettings.skybox = skyboxes[environment];


        //change the emission color of the directional light
        GameObject.Find("Directional Light").GetComponent<Light>().color = lightColors[environment];
    }

    private void NewIsland()
    {
        float x = 0;
        float z = 0;
        float y = 0;

        Vector2 xz = new Vector2(0f, 0f);
        while(Vector2.Distance(xz, Vector2.zero) < 100f)
        {
            x = Random.Range(Random.Range(-500f, -50f), Random.Range(50f, 500f));
            z = Random.Range(Random.Range(-500f, -50f), Random.Range(50f, 500f));
            y = Random.Range(Random.Range(-100f, -40f), Random.Range(40f, 100f));

            xz = new Vector2 (x, z);
        }


        //for each placed island, check if it's too close to the new island
        int tries = 40;
        for (int i = 0; i < placedIslands.Count; i++)
        {
            if (Vector3.Distance(placedIslands[i].transform.position, new Vector3(x, y, z)) < 100f)
            {
                xz = new Vector2(0f, 0f);
                while (Vector2.Distance(xz, Vector2.zero) < 100f)
                {
                    x = Random.Range(Random.Range(-500f, -50f), Random.Range(50f, 500f));
                    z = Random.Range(Random.Range(-500f, -50f), Random.Range(50f, 500f));
                    y = Random.Range(Random.Range(-100f, -40f), Random.Range(40f, 100f));

                    xz = new Vector2(x, z);
                }

                tries--;
                if (tries <= 0)
                {
                    Debug.Log("Failed to place an island");
                    return;
                }
                i = 0;
            }
        }
        GameObject go = Instantiate(island, transform);
        go.transform.position = new Vector3(x, y, z);

        //add the new island to the list of placed islands
        placedIslands.Add(go);
    }

}
