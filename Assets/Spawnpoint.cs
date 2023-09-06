using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnpoint : MonoBehaviour
{
    public static Spawnpoint Instance { get; private set; }
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

    public Vector3 spawnpoint;
    private ParticleSystem ps;

    // Start is called before the first frame update
    void Start()
    {
        spawnpoint = transform.position;
        ps = GetComponent<ParticleSystem>();
    }

    public void PlayParticles()
    {
        if (!ps) ps = GetComponent<ParticleSystem>();
        ps.Play();
    }
}
