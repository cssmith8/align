using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WandTip : MonoBehaviour
{
    public bool rightWand;

    public float threshold = .5f;

    public InputActionReference gripReference = null;
    public InputActionReference triggerReference = null;

    [SerializeField]
    private Shader s;
    private Material m;

    public bool activeWand = false;

    private GameObject other;

    private PlayerMain pm = null;

    // Start is called before the first frame update
    void Start()
    {
        m = new Material(s);
        GetComponent<MeshRenderer>().material = m;

        if (GameObject.FindGameObjectWithTag("Player"))
        pm = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMain>();

        if (rightWand)
        {
            other = GameObject.FindGameObjectWithTag("Leftwand").transform.GetChild(1).GetChild(1).gameObject;
        } else
        {
            other = GameObject.FindGameObjectWithTag("Rightwand").transform.GetChild(1).GetChild(1).gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
        transform.eulerAngles += Vector3.up * 90f;

        float g = gripReference.action.ReadValue<float>();
        float t = triggerReference.action.ReadValue<float>();

        float input = Mathf.Max(g, t);


        //set wand active
        bool lastActive = activeWand;
        activeWand = (input > threshold);

        

        if (activeWand != lastActive)
        {
            if (activeWand)
            {
                OnWandActive();
            } else
            {
                OnWandDeactive();
            }
        }


        //set main trail
        GetComponent<TrailRenderer>().time = 0.2f * input;

        //set alpha trail
        transform.GetChild(0).GetComponent<TrailRenderer>().time = 5f * input;

        m.SetFloat("_Emission", Mathf.Lerp(0.3f, 1.3f, input));

    }

    private void OnWandActive()
    {

        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.1f);
        //print the number of colliders in array
        //UnityEngine.Debug.Log(colliders.Length);


        foreach (Collider c in colliders)
        {
            if (c.gameObject.tag == "Button")
            {
                c.gameObject.GetComponent<SphereButton>().OnClick();
            }
        }

    }

    private void OnWandDeactive()
    {
        if (!other.GetComponent<WandTip>().activeWand && pm)
        {
            //both wands have turned off
            pm.ReleaseStars();
        }
    }

    
}
