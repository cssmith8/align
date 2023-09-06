using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Complete : MonoBehaviour
{
    private GameObject xr;

    void Start()
    {
        xr = GameObject.FindGameObjectWithTag("XROrigin");
        transform.position = new Vector3(0, SceneData.Instance.levels[SceneData.Instance.level].extraHeight, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (xr)
        {
            transform.eulerAngles = new Vector3(0, 0 - xr.GetComponent<PerspectiveRotate>().angleMain, 0);
        }
    }

    public void CompleteLevel()
    {
        int seconds = (int)Time.timeSinceLevelLoad;
        int minutes = seconds / 60;
        seconds -= minutes * 60;

        string displayMinutes = (minutes < 10) ? "0" + minutes.ToString() : minutes.ToString();
        string displaySeconds = (seconds < 10) ? "0" + seconds.ToString() : seconds.ToString();


        transform.GetChild(0).GetChild(1).gameObject.GetComponent<TMP_Text>().text = "time - " + displayMinutes + ":" + displaySeconds;

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }
}
