using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonHover : MonoBehaviour
{
    void Update()
    {
        Vector2 screenPosition = new Vector2(Input.mousePosition.x - 950f, Input.mousePosition.y - 500f);
        transform.localPosition = screenPosition;
    }
}
