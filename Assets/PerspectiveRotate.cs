using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PerspectiveRotate : MonoBehaviour
{
    public InputActionReference rightStickReference = null;

    public float angleMain = 0f;

    [SerializeField]
    private float distance = 16f;

    [SerializeField]
    private float yPos = 0f;

    [SerializeField]
    public Vector2 yBounds = new Vector2(-1f, 1f);

    private float yOffset;

    // Start is called before the first frame update
    void Start()
    {
        yOffset = transform.position.y;
        yBounds.y += SceneData.Instance.levels[SceneData.Instance.level].extraHeight;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = rightStickReference.action.ReadValue<Vector2>();

        angleMain += input.x * Time.deltaTime * 90f;

        yPos += input.y * Time.deltaTime * 10f;

        if (yPos < yBounds.x) yPos = yBounds.x;
        if (yPos > yBounds.y) yPos = yBounds.y;


        transform.eulerAngles = new Vector3(0f, 0 - angleMain, 0f);

        float x = Mathf.Sin(Mathf.Deg2Rad * angleMain) * distance;
        float z = Mathf.Cos(Mathf.Deg2Rad * angleMain) * distance * -1;

        transform.position = new Vector3(x, yOffset + yPos, z);

    }
}
