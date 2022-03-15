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
        if(!SelectedCell.GetComponent<Cell>().FloodDefence.Equals("Trees"))
        {
            RefundCredits();
            SelectedCell.GetComponent<Cell>().FloodDefence = "Trees";
            RemoveCredits();
            SelectedCell.GetComponent<Cell>().PlaceFloodDefence();
        }
    }

    public void PlaceDredging()
    {
        if(!SelectedCell.GetComponent<Cell>().FloodDefence.Equals("Dredging"))
        {
            RefundCredits();
            SelectedCell.GetComponent<Cell>().FloodDefence = "Dredging";
            RemoveCredits();
            SelectedCell.GetComponent<Cell>().PlaceFloodDefence();
        }
    }

    public void PlaceDam()
    {
        if(!SelectedCell.GetComponent<Cell>().FloodDefence.Equals("Dam"))
        {
            RefundCredits();
            SelectedCell.GetComponent<Cell>().FloodDefence = "Dam";
            RemoveCredits();
            SelectedCell.GetComponent<Cell>().PlaceFloodDefence();
        }
    }

    public void PlaceLeakyDam()
    { 
        if(!SelectedCell.GetComponent<Cell>().FloodDefence.Equals("Leaky Dam"))
        {
            RefundCredits();
            SelectedCell.GetComponent<Cell>().FloodDefence = "Leaky Dam";
            RemoveCredits();
            SelectedCell.GetComponent<Cell>().PlaceFloodDefence();
        }
    }

    public void PlaceFloodWall()
    {
        if(!SelectedCell.GetComponent<Cell>().FloodDefence.Equals("Flood Wall"))
        {
            RefundCredits();
            SelectedCell.GetComponent<Cell>().FloodDefence = "Flood Wall";
            RemoveCredits();
            SelectedCell.GetComponent<Cell>().PlaceFloodDefence();
        }
        
    }

    public void PlaceFPUA() //Flood proofing urban areas
    {
        if (!SelectedCell.GetComponent<Cell>().FloodDefence.Equals("Flood proofing urban areas"))
        {
            RefundCredits();
            SelectedCell.GetComponent<Cell>().FloodDefence = "Flood proofing urban areas";
            RemoveCredits();
            SelectedCell.GetComponent<Cell>().PlaceFloodDefence();
        }
    }

    public void RemoveDefence()
    {
        if(!SelectedCell.GetComponent<Cell>().FloodDefence.Equals("Normal"))
        {
            RefundCredits();
            SelectedCell.GetComponent<Cell>().FloodDefence = "Normal";
            SelectedCell.GetComponent<Cell>().PlaceFloodDefence();
        }
    }

    private void RemoveCredits()
    {
        switch (SelectedCell.GetComponent<Cell>().FloodDefence)
        {
            case "Trees":
                Credits.GetComponent<Credits>().RemoveCredits(1);
                break;
            case "Flood proofing urban areas":
                Credits.GetComponent<Credits>().RemoveCredits(10);
                break;
            case "Flood Wall":
                Credits.GetComponent<Credits>().RemoveCredits(20);
                break;
            case "Leaky Dam":
                Credits.GetComponent<Credits>().RemoveCredits(10);
                break;
            case "Dam":
                Credits.GetComponent<Credits>().RemoveCredits(100);
                break;
            case "Dredging":
                Credits.GetComponent<Credits>().RemoveCredits(50);
                break;
        }

        
    }

    private void RefundCredits()
    { 

        switch (SelectedCell.GetComponent<Cell>().FloodDefence)
        {
            case "Trees":
                Credits.GetComponent<Credits>().AddCredits(1);
                break;
            case "Flood proofing urban areas":
                Credits.GetComponent<Credits>().AddCredits(10);
                break;
            case "Flood Wall":
                Credits.GetComponent<Credits>().AddCredits(20);
                break;
            case "Leaky Dam":
                Credits.GetComponent<Credits>().AddCredits(10);
                break;
            case "Dam":
                Credits.GetComponent<Credits>().AddCredits(100);
                break;
            case "Dredging":
                Credits.GetComponent<Credits>().AddCredits(50);
                break;
        }
    }
}
