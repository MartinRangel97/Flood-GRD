using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HillslopeChannel : MonoBehaviour
{
    public Dropdown CellType;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(CellType.value == 0)
        {
            transform.Find("Hillslope").gameObject.SetActive(false);
            transform.Find("Channel").gameObject.SetActive(true);
        }

        if (CellType.value == 1)
        {
            transform.Find("Channel").gameObject.SetActive(false);
            transform.Find("Hillslope").gameObject.SetActive(true);
        }
    }
}
