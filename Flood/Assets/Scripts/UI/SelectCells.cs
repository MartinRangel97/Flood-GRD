using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectCells : MonoBehaviour
{
    public GameObject CellInformation;
    public GameObject SelectedCell;
    public GameObject DefenceType;
    public GameObject ContributingCells;

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

        if(SelectedCell != null)
        {
            DefenceType.GetComponent<Text>().text = SelectedCell.GetComponent<Cell>().FloodDefence;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (hit.collider)
            {
                SelectedCell = hit.collider.gameObject;

                CellInformation.transform.GetChild(1).GetChild(1).GetComponent<Text>().text = SelectedCell.GetComponent<Cell>().GetCellType().ToString();

                ContributingCells.GetComponent<Text>().text = SelectedCell.gameObject.GetComponent<Cell>().ContributingCells().ToString();

                foreach (Transform child in transform)
                {
                    if (hit.collider.name == child.name)
                    {
                        child.GetComponent<Cell>().isSelected = true;
                        ClearFlowArrows();
                        for(var i = 0; i < child.GetComponent<Cell>().GetFlow().Count; i++)
                        {
                            CellInformation.transform.Find("Flow").Find("Cell").Find(child.GetComponent<Cell>().GetFlow()[i]).gameObject.SetActive(true);

                        }
                    }
                    else
                    {
                        child.GetComponent<Cell>().isSelected = false;
                    }
                }
            }
        }
    }

    void ClearFlowArrows()
    {
        foreach (Transform child in CellInformation.transform.Find("Flow").Find("Cell"))
        {
            child.gameObject.SetActive(false);
        }
    }

    public void PlaceTrees()
    {
        SelectedCell.GetComponent<Cell>().FloodDefence = "Trees";
        SelectedCell.GetComponent<Cell>().PlaceFloodDefence();
    }

    public void PlaceDredging()
    {
        SelectedCell.GetComponent<Cell>().FloodDefence = "Dredging";
        SelectedCell.GetComponent<Cell>().PlaceFloodDefence();
    }

    public void PlaceDam()
    {
        SelectedCell.GetComponent<Cell>().FloodDefence = "Dam";
        SelectedCell.GetComponent<Cell>().PlaceFloodDefence();
    }

    public void PlaceLeakyDam()
    { 
        SelectedCell.GetComponent<Cell>().FloodDefence = "Leaky Dam";
        SelectedCell.GetComponent<Cell>().PlaceFloodDefence();
    }

    public void PlaceFloodWall()
    {
        SelectedCell.GetComponent<Cell>().FloodDefence = "Flood Wall";
        SelectedCell.GetComponent<Cell>().PlaceFloodDefence();
    }

    public void PlaceFPUA() //Flood proofing urban areas
    {
        SelectedCell.GetComponent<Cell>().FloodDefence = "Flood proofing urban areas";
        SelectedCell.GetComponent<Cell>().PlaceFloodDefence();
    }

    public void RemoveDefence()
    {
        SelectedCell.GetComponent<Cell>().FloodDefence = "Normal";
        SelectedCell.GetComponent<Cell>().PlaceFloodDefence();
    }
}
