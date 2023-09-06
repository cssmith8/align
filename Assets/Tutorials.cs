using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorials : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Display(List<int> list)
    {
        foreach (int i in list)
        {
            switch (i)
            {
                case 1:
                    GameObject.FindGameObjectWithTag("Leftwand").transform.GetChild(0).GetChild(2).GetChild(0).gameObject.SetActive(true);
                    break;
                case 2:
                    GameObject.FindGameObjectWithTag("Rightwand").transform.GetChild(0).GetChild(2).GetChild(0).gameObject.SetActive(true);
                    break;
                case 3:
                    GameObject.FindGameObjectWithTag("Leftwand").transform.GetChild(0).GetChild(2).GetChild(1).gameObject.SetActive(true);
                    break;
                case 4:
                    GameObject.FindGameObjectWithTag("Rightwand").transform.GetChild(0).GetChild(2).GetChild(1).gameObject.SetActive(true);
                    break;
            }
        }
    }
}
