using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTree : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //set y rotation to a rnadom value between 0 and 360   
        transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

        //multiply size by a random value between 0.9 and 1.1
        transform.localScale *= Random.Range(0.9f, 1.1f);
    }

}
