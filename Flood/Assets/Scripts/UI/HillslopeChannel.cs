using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HillslopeChannel : MonoBehaviour
{
    public GameObject CellType;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (CellType.GetComponent<Text>().text)
        {
            case "Channel":
                transform.Find("Hillslope").gameObject.SetActive(false);
                transform.Find("Channel").gameObject.SetActive(true);
                break;
            case "Hillslope":
                transform.Find("Channel").gameObject.SetActive(false);
                transform.Find("Hillslope").gameObject.SetActive(true);
                break;
            default:
                transform.Find("Channel").gameObject.SetActive(false);
                transform.Find("Hillslope").gameObject.SetActive(false);
                break;
        }
    }
}
