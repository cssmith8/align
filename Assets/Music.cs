using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Music : MonoBehaviour
{
    public static Music Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.Log("Destroying duplicate Music instance");
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private float complete = 0f;
    private float completeTarget = 0f;

    public int aliass = 1;

    public FMOD.Studio.EventInstance musicEvent;

    public void setComplete(float setting)
    {
        completeTarget = setting;
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        //musicEvent = FMODUnity.RuntimeManager.CreateInstance("event:/Music");
        //musicEvent.start();
        //musicEvent.release();
    }

    private void OnDestroy()
    {
        //musicEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (complete > completeTarget)
        {
            complete -= 0.5f * Time.fixedDeltaTime;
            complete = Mathf.Clamp(completeTarget, 0f, 1f);
            //musicEvent.setParameterByName("Complete", complete);
        }
        else if (complete < completeTarget)
        {
            complete += 0.5f * Time.fixedDeltaTime;
            complete = Mathf.Clamp(completeTarget, 0f, 1f);
            //musicEvent.setParameterByName("Complete", complete);
        }
    }

    public void PlayNote(int strength)
    {
        if (strength < 1 || strength > 5)
        {
            UnityEngine.Debug.LogError("Invalid strength value");
            return;
        }

        string[] notes = { "C", "D", "E", "F", "G" };
        string note = notes[strength - 1];

        //FMODUnity.RuntimeManager.PlayOneShot("event:/" + note);
    }

    public void PlaySequence(int strength, float speed = 0.12f, bool instant = false)
    {
        if (strength < 1 || strength > 5)
        {
            UnityEngine.Debug.LogError("Invalid strength value");
            return;
        }
        StartCoroutine(Sequence(strength, speed, instant));
    }

    private IEnumerator Sequence(int strength, float speed, bool instant)
    {
        float current = Time.timeSinceLevelLoad;

        if (!instant) yield return new WaitForSeconds(0.5f);

        string[] notes = { "C", "D", "E", "F", "G" };

        int i = 0;

        while (i < strength)
        {
            if (Time.timeSinceLevelLoad - current > speed)
            {
                current = Time.timeSinceLevelLoad;
                string note = notes[i];
                //FMODUnity.RuntimeManager.PlayOneShot("event:/" + note);
                i++;
            }
            yield return null;
        }
    }
}
