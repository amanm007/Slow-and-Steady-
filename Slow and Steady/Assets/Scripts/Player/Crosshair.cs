using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
     void Awake()
    {
        Cursor.visible = false;

    }
     void Update()
    {
        Vector2 mouseCursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3((float)(mouseCursorPos.x + 0.063), (float)(mouseCursorPos.y - 0.1228));

    }
}
