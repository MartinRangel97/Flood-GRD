using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HillslopeChannel : MonoBehaviour
{
    public GameObject CellType;
    public Transform FloodDefences;
    public GameObject ContributingCells;

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
                ResetFloodDefences();
                FloodDefences.Find("Dredging").gameObject.SetActive(true);
                FloodDefences.Find("Leaky Dam").gameObject.SetActive(true);
                FloodDefences.Find("Dam").gameObject.SetActive(true);
                FloodDefences.Find("Flood Wall").gameObject.SetActive(true);
                ContributingCells.SetActive(true);
                break;
            case "Hillslope":
                ResetFloodDefences();
                FloodDefences.Find("Trees").gameObject.SetActive(true);
                ContributingCells.SetActive(false);
                break;
            case "Urban":
                ResetFloodDefences();
                FloodDefences.Find("Flood Proofing Urban Areas").gameObject.SetActive(true);
                ContributingCells.SetActive(false);
                break;
            default:
                ContributingCells.SetActive(false);
                ResetFloodDefences();
                break;
        }
    }

    void ResetFloodDefences()
    {
        foreach(Transform child in FloodDefences)
        {
            child.gameObject.SetActive(false);
        }
    }
}
