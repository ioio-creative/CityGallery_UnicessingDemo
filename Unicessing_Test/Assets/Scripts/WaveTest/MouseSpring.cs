using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseSpring : MonoBehaviour
{
    public Vector3 CursorPos
    {
        get; private set;
    }

    public float CursorDX
    {
        get; private set;
    }

    [SerializeField]
    [Range(0f, 1f)]
    private float easingFactor = 0.2f;

    private Vector3 screenSizeOffset;

    private void Start()
    {
        CursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        screenSizeOffset = new Vector3(Screen.width / 2, Screen.height / 2);
    }

    private void FixedUpdate()
    {
        Vector3 newCursorPos = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - CursorPos) * easingFactor + CursorPos;

        if (IsMouseInsideScreenWidth())
        {
            CursorDX = Mathf.Abs((newCursorPos.x - CursorPos.x)) / Time.fixedDeltaTime;
            //Debug.Log("dX: " + CursorDX);
        }
        else
        {
            CursorDX = 0;
        }
        CursorPos = newCursorPos;
        GetComponentInChildren<SphereCollider>().transform.position = CursorPos;
    }

    private bool IsMouseInsideScreenWidth()
    {
        return (Input.mousePosition.x >= 0) && (Input.mousePosition.x <= Screen.width);
    }
}
