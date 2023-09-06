using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using Unity.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SphereButton : MonoBehaviour
{
    private GameObject displaySphere;

    private GameObject leftWandSphere;
    private GameObject rightWandSphere;

    [SerializeField]
    private int level = 0;

    [SerializeField]
    private bool toMenu = false;

    [SerializeField]
    private bool nextLevel = false;

    // Start is called before the first frame update
    void Start()
    {
        displaySphere = transform.GetChild(0).gameObject;
        if (!toMenu && !nextLevel)
        transform.GetChild(1).GetChild(0).gameObject.GetComponent<TMP_Text>().text = level.ToString();

        leftWandSphere = GameObject.FindGameObjectWithTag("Leftwand").transform.GetChild(1).GetChild(1).gameObject;
        rightWandSphere = GameObject.FindGameObjectWithTag("Rightwand").transform.GetChild(1).GetChild(1).gameObject;
    }

    public void OnClick()
    {
        
        Music.Instance.PlayNote(2);
        GameObject.FindGameObjectWithTag("Fade").GetComponent<SceneFade>().FadeOut();
        if (toMenu)
        {
            Music.Instance.setComplete(0);
            Invoke("LoadMenu", 1.2f);
        }
        else
        {
            if (nextLevel)
            {
                SceneData.Instance.level++;
            }
            else
            {
                SceneData.Instance.level = level;
            }
            Music.Instance.setComplete(1);
            Invoke("LoadNext", 1.2f);
        }
    }

    private void LoadNext()
    {
        SceneManager.LoadScene("SampleScene");
    }

    private void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    // Update is called once per frame
    void Update()
    {
        GameObject wandSphere = null;
        //set wandSphere to whichever is closer
        if (leftWandSphere && rightWandSphere)
        {
            if (Vector3.Distance(leftWandSphere.transform.position, transform.position) < Vector3.Distance(rightWandSphere.transform.position, transform.position))
            {
                wandSphere = leftWandSphere;
            }
            else
            {
                wandSphere = rightWandSphere;
            }
        }
        else if (leftWandSphere)
        {
            wandSphere = leftWandSphere;
        }
        else if (rightWandSphere)
        {
            wandSphere = rightWandSphere;
        }

        if (wandSphere && displaySphere)
        {
            float dis = Vector3.Distance(transform.position, wandSphere.transform.position);
            displaySphere.transform.localScale = new Vector3(dis, dis, dis) * 2;
            Color c = Color.white;
            float alpha = 0.1f * Mathf.Pow(3, (1f/dis)) - (dis * 0.035f);
            c.a = Mathf.Clamp(alpha, 0f, 1f);
            displaySphere.GetComponent<MeshRenderer>().material.color = c;
        }
    }
}
