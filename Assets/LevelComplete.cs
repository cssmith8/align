using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelComplete : MonoBehaviour
{
    private GameObject text1;
    private GameObject text2;

    [SerializeField]
    public TextMeshProUGUI textMeshProObject;
    public Color glowColor = Color.white;
    public float glowPower = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        text1 = transform.GetChild(0).gameObject;
        text2 = transform.GetChild(1).gameObject;
    }

    IEnumerator Animate()
    {
        float t = 0f;
        while (t < 1)
        {
            SetOpacity(1, 1 - t);
            SetOpacity(2, 1 - t);
            t += Time.deltaTime;
            yield return null;
        }

        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private bool SetOpacity(int num, float value)
    {
        switch (num)
        {
            case 1:
                if (!text1) return false;
                Color c = text1.GetComponent<TMP_Text>().color;
                c.a = Mathf.Clamp(value, 0f, 1f);
                text1.GetComponent<TMP_Text>().color = c;
                break;
            case 2:
                if (!text1) return false;
                Color c2 = text2.GetComponent<TMP_Text>().color;
                c.a = Mathf.Clamp(value, 0f, 1f);
                text1.GetComponent<TMP_Text>().color = c2;
                break;
            default:
                return false;
        }
        return true;
    }
}
