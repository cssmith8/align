using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Level
{
    [SerializeField]
    public GameObject terrain;
    [SerializeField]
    public Vector3[] stars;
    [SerializeField]
    public List<int> tutorials = new List<int>();
    [SerializeField]
    public int extraHeight = 0;
    [SerializeField]
    public int environment = 0;
    // 0 purple, 1 orange, 2 blue
}
