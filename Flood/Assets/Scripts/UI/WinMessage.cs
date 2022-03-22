using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class WinMessage : MonoBehaviour
{
    public GameObject Credits;
    void Start()
    {
        GetComponent<TextMeshProUGUI>().text = "You have " + Credits.GetComponent<Text>().text + " credits remaining";
    }


}
