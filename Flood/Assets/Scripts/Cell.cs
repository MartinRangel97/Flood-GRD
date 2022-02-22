using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CellType { Channel , Hillslope, Urban}

public class Cell : MonoBehaviour {

    public int elevation;
    public int flowElevation;
    public float waterLevel = 0;    // level of water on this tile
    public float waterGainedThisCycle = 0; // The amount of water this tile has gained before the end of a cycle

    public float capacity = 0;
    public float attenuation = 0;

    private static int colourMultiplier = 3;
    private static int maxColourElevation = 255 / colourMultiplier;
    private static int elevationSharedColourMultiplier = 1; //colourMultiplier * 2 - 4;

    public List<GameObject> flowsTo = new List<GameObject>();

    public string FloodDefence = "Normal";

    public bool isRiverEnd;

    private CellType cellType;
    private bool isActivated = false;   //Is used when calculating elevation of each tile
    public bool isSelected = false;

    private void Start() {
        cellType = CellType.Channel;    //Botch
        ChangeElevation(maxColourElevation);
        ChangeCellType();
    }

    private void Update()
    {
        if (isSelected)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }

        ChangeAttenuation();
    }

    private void ChangeAttenuation()
    {
        switch (FloodDefence)
        {
            case "Trees":
                attenuation = 0.2f;
                break;
            case "Leaky Dam":
                attenuation = 0.5f;
                break;
            case "Dam":
                attenuation = 0.8f;
                break;
        }
    }

    // Changes the elevation of this Cell: -1 = -1 elevation, 0 = +1 elevation
    public void ChangeElevation(int newElevation) {

        if (newElevation < -1) {
            //Debug.Log("Invalid: New Elevation is too low");
            return;

        } else if (newElevation == -1 && elevation > 1) {
            elevation--;

        } else if (newElevation == 0) {
            elevation++;

        } else {
            elevation = newElevation;
        }

        if (cellType == CellType.Hillslope) {
            
            //Changing the colour of the cell based on its elevation
            //As colour cannot exceed 255, if elevation * colourMultiplier exceeds 255,
            // the cell's colour is set to max green value
            int colourElevation = Mathf.CeilToInt( elevation / elevationSharedColourMultiplier);
            if (colourElevation * colourMultiplier >= maxColourElevation) {
                colourElevation = maxColourElevation;
            } else {
                colourElevation = colourElevation * colourMultiplier;
            }
            ChangeColour(0, (byte)(colourElevation * colourMultiplier), 0);
        }
        
    }

    public bool ChangeCellType() {
        if (cellType == CellType.Hillslope) {
            cellType = CellType.Channel;
            ChangeElevation(WorldManager.channelElevationValue);
            ChangeColour(0, 8, 0);

            attenuation = ValueDictionarys.valueDictionary["channel"].attenuation;
            capacity = ValueDictionarys.valueDictionary["channel"].capacity;

            return true;
        } else {
            cellType = CellType.Hillslope;
            ChangeElevation(255);


            attenuation = ValueDictionarys.valueDictionary["hillslope"].attenuation;
            capacity = ValueDictionarys.valueDictionary["hillslope"].capacity;

            return false;
        }
    }

    public void SetFlowList(GameObject item) {
        flowsTo.Add(item);
    }

    public List<GameObject> GetFlowList() {
        return flowsTo;
    }

    public void ChangeColour(byte r, byte g, byte b) {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.color = new Color32(r, g, b, 255);
    }

    public CellType GetCellType() {
        return cellType;
    }

    public int GetElevation() {
        return elevation;
    }

    public void Activation(bool value) {
        isActivated = value;
    }

    public bool isActive() {
        return isActivated;
    }

    // Changes the waterLevel to a set value
    public void SetWaterLevel(float value) {
        if (value >= 0) {
            waterLevel = value;
        }
    }

    // Increases (or decreases with negative param) the water level by a value
    public void ChangeWaterLevel(float changeValue) {
        if (changeValue + waterLevel >= 0) {
            waterLevel += changeValue;
        } else {
            waterLevel = 0;
        }
    }

    public float GetWaterLevel() {
        return waterLevel;
    }


    public List<string> GetFlow() {
        //Debug.Log(gameObject.name);
        string[] cellname = gameObject.name.Split('_');
        int x = int.Parse(cellname[1]);
        int y = int.Parse(cellname[2]);
        //Debug.Log(x + "  " + y);
        int min = 0;
        int max = 50;

        List<string> CellFlow = new List<string>();

        //Left
        if (x - 1 >= min)
        {
            GameObject LeftCell = GameObject.Find("Cell_" + (x-1).ToString() + "_" + y.ToString());
            if (elevation >= LeftCell.GetComponent<Cell>().GetElevation())
            {
                CellFlow.Add("Left");
            }
        }

        //Right
        if (x + 1 <= max)
        {
            GameObject RightCell = GameObject.Find("Cell_" + (x + 1).ToString() + "_" + y.ToString());
            if (elevation >= RightCell.GetComponent<Cell>().GetElevation())
            {
                CellFlow.Add("Right");
            }
        }

        //Down
        if (y - 1 >= min)
        {
            GameObject DownCell = GameObject.Find("Cell_" + x.ToString() + "_" + (y -1).ToString());
            if (elevation >= DownCell.GetComponent<Cell>().GetElevation())
            {
                CellFlow.Add("Down");
            }
        }

        //Up
        if (y + 1 <= max)
        {
            GameObject UpCell = GameObject.Find("Cell_" + x.ToString() + "_" + (y + 1).ToString());
            if (elevation >= UpCell.GetComponent<Cell>().GetElevation())
            {
                CellFlow.Add("Up");
            }
        }

        //UpLeft
        if (x - 1 >= min && y + 1 <= max)
        {
            GameObject UpLeftCell = GameObject.Find("Cell_" + (x - 1).ToString() + "_" + (y + 1).ToString());
            if (elevation >= UpLeftCell.GetComponent<Cell>().GetElevation())
            {
                CellFlow.Add("UpLeft");
            }
        }

        //DownLeft
        if (x - 1 >= min && y - 1 >= min)
        {
            GameObject DownLeftCell = GameObject.Find("Cell_" + (x - 1).ToString() + "_" + (y - 1).ToString());
            if (elevation >= DownLeftCell.GetComponent<Cell>().GetElevation())
            {
                CellFlow.Add("DownLeft");
            }
        }

        //UpRight
        if (x + 1 <= max && y + 1 <= max)
        {
            GameObject UpRightCell = GameObject.Find("Cell_" + (x + 1).ToString() + "_" + (y + 1).ToString());
            if (elevation >= UpRightCell.GetComponent<Cell>().GetElevation())
            {
                CellFlow.Add("UpRight");
            }
        }

        //DownRight
        if (x + 1 <= max && y - 1 >= min)
        {
            GameObject DownRightCell = GameObject.Find("Cell_" + (x + 1).ToString() + "_" + (y - 1).ToString());
            if (elevation >= DownRightCell.GetComponent<Cell>().GetElevation())
            {
                CellFlow.Add("DownRight");
            }
        }

        return CellFlow;

    }

    public int ContributingCells()
    {
        string[] cellname = gameObject.name.Split('_');
        int x = int.Parse(cellname[1]);
        int y = int.Parse(cellname[2]);
        int ContCells = 0;
        int min = 0;
        int max = 50;

        if(cellType == CellType.Channel)
        {
            if (x - 1 >= min)
            {
                GameObject LeftCell = GameObject.Find("Cell_" + (x - 1).ToString() + "_" + y.ToString());
                if (elevation < LeftCell.GetComponent<Cell>().GetElevation())
                {
                    ContCells++;
                }
            }

            //Right
            if (x + 1 <= max)
            {
                GameObject RightCell = GameObject.Find("Cell_" + (x + 1).ToString() + "_" + y.ToString());
                if (elevation < RightCell.GetComponent<Cell>().GetElevation())
                {
                    ContCells++;
                }
            }

            //Down
            if (y - 1 >= min)
            {
                GameObject DownCell = GameObject.Find("Cell_" + x.ToString() + "_" + (y - 1).ToString());
                if (elevation < DownCell.GetComponent<Cell>().GetElevation())
                {
                    ContCells++;
                }
            }

            //Up
            if (y + 1 <= max)
            {
                GameObject UpCell = GameObject.Find("Cell_" + x.ToString() + "_" + (y + 1).ToString());
                if (elevation < UpCell.GetComponent<Cell>().GetElevation())
                {
                    ContCells++;
                }
            }

            //UpLeft
            if (x - 1 >= min && y + 1 <= max)
            {
                GameObject UpLeftCell = GameObject.Find("Cell_" + (x - 1).ToString() + "_" + (y + 1).ToString());
                if (elevation < UpLeftCell.GetComponent<Cell>().GetElevation())
                {
                    ContCells++;
                }
            }

            //DownLeft
            if (x - 1 >= min && y - 1 >= min)
            {
                GameObject DownLeftCell = GameObject.Find("Cell_" + (x - 1).ToString() + "_" + (y - 1).ToString());
                if (elevation < DownLeftCell.GetComponent<Cell>().GetElevation())
                {
                    ContCells++;
                }
            }

            //UpRight
            if (x + 1 <= max && y + 1 <= max)
            {
                GameObject UpRightCell = GameObject.Find("Cell_" + (x + 1).ToString() + "_" + (y + 1).ToString());
                if (elevation < UpRightCell.GetComponent<Cell>().GetElevation())
                {
                    ContCells++;
                }
            }

            //DownRight
            if (x + 1 <= max && y - 1 >= min)
            {
                GameObject DownRightCell = GameObject.Find("Cell_" + (x + 1).ToString() + "_" + (y - 1).ToString());
                if (elevation < DownRightCell.GetComponent<Cell>().GetElevation())
                {
                    ContCells++;
                }
            }
        }

        return ContCells;
    }

}
