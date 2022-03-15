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
    public GameObject Credits;

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

        Cell cell = SelectedCell.GetComponent<Cell>();

        if(!cell.FloodDefence.Equals("Trees"))
        {
            RefundCredits();
            cell.PlaceFloodDefence("Trees");
            RemoveCredits(ValueDictionarys.valueDictionary["Trees"].cost);
            
        }
    }

    public void PlaceDredging()
    {

        Cell cell = SelectedCell.GetComponent<Cell>();

        if (!cell.FloodDefence.Equals("Dredging"))
        {
            RefundCredits();
            cell.PlaceFloodDefence("Dredging");
            RemoveCredits(ValueDictionarys.valueDictionary["Dredging"].cost);
            
        }
    }

    public void PlaceDam()
    {
        Cell cell = SelectedCell.GetComponent<Cell>();

        if (!cell.FloodDefence.Equals("Dam"))
        {
            RefundCredits();
            cell.PlaceFloodDefence("Dam");
            RemoveCredits(ValueDictionarys.valueDictionary["Dam"].cost);
        }
    }

    public void PlaceLeakyDam()
    {
        Cell cell = SelectedCell.GetComponent<Cell>();

        if (!cell.FloodDefence.Equals("Leaky Dam"))
        {
            RefundCredits();
            cell.PlaceFloodDefence("Leaky Dam");
            RemoveCredits(ValueDictionarys.valueDictionary["Leaky Dam"].cost);
        }
    }

    public void PlaceFloodWall()
    {
        Cell cell = SelectedCell.GetComponent<Cell>();

        if (!cell.FloodDefence.Equals("Flood Wall"))
        {
            RefundCredits();
            cell.PlaceFloodDefence("Flood Wall");
            RemoveCredits(ValueDictionarys.valueDictionary["Flood Wall"].cost);
        }
        
    }

    public void PlaceFPUA() //Flood proofing urban areas
    {
        
        Cell cell = SelectedCell.GetComponent<Cell>();

        if (!cell.FloodDefence.Equals("Flood proofing urban areas"))
        {
            RefundCredits();
            cell.PlaceFloodDefence("Flood proofing urban areas");
            RemoveCredits(ValueDictionarys.valueDictionary["Flood proofing urban areas"].cost);
        }
    }

    public void RemoveDefence()
    {
        Cell cell = SelectedCell.GetComponent<Cell>();

        if (!cell.FloodDefence.Equals("Normal"))
        {
            RefundCredits();
            cell.PlaceFloodDefence("Normal");
        }
    }

    private void RemoveCredits(float credits)
    {
        Credits.GetComponent<Credits>().RemoveCredits(credits);
        
    }

    private void RefundCredits()
    { 

        switch (SelectedCell.GetComponent<Cell>().FloodDefence)
        {
            case "Trees":
                Credits.GetComponent<Credits>().AddCredits(ValueDictionarys.valueDictionary["Trees"].cost);
                break;

            case "Flood proofing urban areas":
                Credits.GetComponent<Credits>().AddCredits(ValueDictionarys.valueDictionary["Flood proofing urban areas"].cost);
                break;

            case "Flood Wall":
                Credits.GetComponent<Credits>().AddCredits(ValueDictionarys.valueDictionary["Flood Wall"].cost);
                break;

            case "Leaky Dam":
                Credits.GetComponent<Credits>().AddCredits(ValueDictionarys.valueDictionary["Leaky Dam"].cost);
                break;

            case "Dam":
                Credits.GetComponent<Credits>().AddCredits(ValueDictionarys.valueDictionary["Dam"].cost);
                break;

            case "Dredging":
                Credits.GetComponent<Credits>().AddCredits(ValueDictionarys.valueDictionary["Dredging"].cost);
                break;
        }
    }
}
