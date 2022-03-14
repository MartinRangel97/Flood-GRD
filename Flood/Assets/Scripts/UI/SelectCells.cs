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
        if(SelectedCell.GetComponent<Cell>().FloodDefence != "Trees")
        {
            SelectedCell.GetComponent<Cell>().FloodDefence = "Trees";
            RemoveCredits();
            SelectedCell.GetComponent<Cell>().PlaceFloodDefence();
        }
    }

    public void PlaceDredging()
    {
        if(SelectedCell.GetComponent<Cell>().FloodDefence != "Dredging")
        {
            SelectedCell.GetComponent<Cell>().FloodDefence = "Dredging";
            RemoveCredits();
            SelectedCell.GetComponent<Cell>().PlaceFloodDefence();
        }
    }

    public void PlaceDam()
    {
        if(SelectedCell.GetComponent<Cell>().FloodDefence != "Dam")
        {
            SelectedCell.GetComponent<Cell>().FloodDefence = "Dam";
            RemoveCredits();
            SelectedCell.GetComponent<Cell>().PlaceFloodDefence();
        }
    }

    public void PlaceLeakyDam()
    { 
        if(SelectedCell.GetComponent<Cell>().FloodDefence != "Leaky Dam")
        {
            SelectedCell.GetComponent<Cell>().FloodDefence = "Leaky Dam";
            RemoveCredits();
            SelectedCell.GetComponent<Cell>().PlaceFloodDefence();
        }
    }

    public void PlaceFloodWall()
    {
        if(SelectedCell.GetComponent<Cell>().FloodDefence != "Flood Wall")
        {
            SelectedCell.GetComponent<Cell>().FloodDefence = "Flood Wall";
            RemoveCredits();
            SelectedCell.GetComponent<Cell>().PlaceFloodDefence();
        }
        
    }

    public void PlaceFPUA() //Flood proofing urban areas
    {
        if (SelectedCell.GetComponent<Cell>().FloodDefence != "Flood proofing urban areas")
        {
            SelectedCell.GetComponent<Cell>().FloodDefence = "Flood proofing urban areas";
            RemoveCredits();
            SelectedCell.GetComponent<Cell>().PlaceFloodDefence();
        }
    }

    public void RemoveDefence()
    {
        if(SelectedCell.GetComponent<Cell>().FloodDefence != "Normal")
        {
            RefundCredits();
            SelectedCell.GetComponent<Cell>().FloodDefence = "Normal";
            SelectedCell.GetComponent<Cell>().PlaceFloodDefence();
        }
    }

    private void RemoveCredits()
    {
        int money = int.Parse(Credits.GetComponent<Text>().text);

        switch (SelectedCell.GetComponent<Cell>().FloodDefence)
        {
            case "Trees":
                money = money - 1;
                break;
            case "Flood proofing urban areas":
                money = money - 10;
                break;
            case "Flood Wall":
                money = money - 20;
                break;
            case "Leaky Dam":
                money = money - 10;
                break;
            case "Dam":
                money = money - 100;
                break;
            case "Dredging":
                money = money - 50;
                break;
        }

        Credits.GetComponent<Text>().text = money.ToString();
    }

    private void RefundCredits()
    {
        int money = int.Parse(Credits.GetComponent<Text>().text);

        switch (SelectedCell.GetComponent<Cell>().FloodDefence)
        {
            case "Trees":
                money = money + 1;
                break;
            case "Flood proofing urban areas":
                money = money + 10;
                break;
            case "Flood Wall":
                money = money = 20;
                break;
            case "Leaky Dam":
                money = money + 10;
                break;
            case "Dam":
                money = money + 100;
                break;
            case "Dredging":
                money = money + 50;
                break;
        }

        Credits.GetComponent<Text>().text = money.ToString();
    }
}
