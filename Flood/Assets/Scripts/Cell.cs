using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CellType { Channel , Hillslope, Urban}

public class Cell : MonoBehaviour {

    SpriteRenderer _sr;


    public int elevation;
    public int flowElevation;
    public float waterLevel = 0;    // level of water on this tile
    public float waterGainedThisCycle = 0; // The amount of water this tile has gained before the end of a cycle

    public float capacity = 0;
    public float attenuation = 0;
    public int upstreamCells = 0;


    private static int colourMultiplier = 3;
    private static int maxColourElevation = 255 / colourMultiplier;
    private static int elevationSharedColourMultiplier = 1; //colourMultiplier * 2 - 4;

    public List<GameObject> flowsTo = new List<GameObject>();

    public string FloodDefence;

    public bool isRiverEnd;

    private CellType cellType;
    private bool isActivated = false;   //Is used when calculating elevation of each tile
    public bool isSelected = false;

    private void Start() {
        _sr = GetComponent<SpriteRenderer>();
        ChangeElevation(maxColourElevation);
        ChangeCellType(CellType.Hillslope);
        FloodDefence = "Normal";
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

        //PlaceFloodDefence();

        ChangeCellColourWithWater();

    }

    public void PlaceFloodDefence()
    {
        switch (FloodDefence)
        {
            case "Trees":
                attenuation = ValueDictionarys.valueDictionary["Trees"].attenuation;
                capacity = ValueDictionarys.valueDictionary["Trees"].capacity;
                ChangeColour(0, 51, 25);
                break;
            case "Leaky Dam":
                attenuation = ValueDictionarys.valueDictionary["Leaky Dam"].attenuation;
                capacity = ValueDictionarys.valueDictionary["Leaky Dam"].capacity;
                ChangeColour(160, 160, 160);
                break;
            case "Dam":
                attenuation = ValueDictionarys.valueDictionary["Dam"].attenuation;
                capacity = ValueDictionarys.valueDictionary["Dam"].capacity;
                ChangeColour(64, 64, 64);
                break;
            case "Flood Wall":
                attenuation = ValueDictionarys.valueDictionary["Flood Wall"].attenuation;
                capacity = ValueDictionarys.valueDictionary["Flood Wall"].capacity;
                ChangeColour(63, 0, 0);
                break;
            case "Dredging":
                attenuation = ValueDictionarys.valueDictionary["Dredging"].attenuation;
                capacity = ValueDictionarys.valueDictionary["Dredging"].capacity;
                ChangeColour(51, 25, 0);
                break;
            case "Flood proofing urban areas":
                ChangeColour(0, 255, 255);
                break;
            case "Normal":
                if (cellType == CellType.Hillslope)
                {
                    attenuation = ValueDictionarys.valueDictionary["hillslope"].attenuation;
                    capacity = ValueDictionarys.valueDictionary["hillslope"].capacity;
                    ChangeElevation(elevation);
                }

                if (cellType == CellType.Channel)
                {
                    attenuation = ValueDictionarys.valueDictionary["channel"].attenuation;
                    //capacity = ValueDictionarys.valueDictionary["channel"].capacity;
                    ChangeColour(0, 0, 0);
                }
                if (cellType == CellType.Urban)
                {
                    ChangeColour(255, 0, 0);
                }
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

    private void ChangeCellColourWithWater() {

        if (waterLevel == 0 || cellType == CellType.Urban || FloodDefence != "Normal") {
            return;
        }


        
        float minWater = 0;
        float maxWater = 15;    //HARDCODED VALUE
        float green = 0;
        float blue = 0;

        float tempWaterLevel = waterLevel;
        if (tempWaterLevel > maxWater) {
            tempWaterLevel = maxWater;
        }

        float lerpValue = Mathf.InverseLerp(maxWater, minWater, tempWaterLevel);
        lerpValue = Mathf.Lerp(0, 510, lerpValue);

        if (lerpValue <= 255) {
            green = 0;
            blue = lerpValue;
        } else {
            blue = 255;
            green = lerpValue - 255;
        }

        if (blue < 20) {
            blue = 20;
        }

        ChangeColour(0, (byte)green, (byte)blue);

    }

    public bool ChangeCellType(CellType newType) {

        switch (newType) {
            case CellType.Channel:
                cellType = CellType.Channel;
                ChangeElevation(WorldManager.channelElevationValue);
                ChangeColour(0, 0, 80);

                attenuation = ValueDictionarys.valueDictionary["channel"].attenuation;
                capacity = ValueDictionarys.valueDictionary["channel"].capacity;

                return true;

            case CellType.Hillslope:
                cellType = CellType.Hillslope;
                ChangeElevation(255);


                attenuation = ValueDictionarys.valueDictionary["hillslope"].attenuation;
                capacity = ValueDictionarys.valueDictionary["hillslope"].capacity;

                return false;

            case CellType.Urban:
                cellType = CellType.Urban;
                ChangeColour(255, 0, 0);

                attenuation = ValueDictionarys.valueDictionary["hillslope"].attenuation;
                capacity = ValueDictionarys.valueDictionary["hillslope"].capacity;

                return false;
        }

        return false;
        
        
    }

    

    public void SetFlowList(GameObject item) {
        flowsTo.Add(item);
    }

    public List<GameObject> GetFlowList() {
        return flowsTo;
    }

    public void ChangeColour(byte r, byte g, byte b) {
        _sr.color = new Color32(r, g, b, 255);
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
        string[] cellname = gameObject.name.Split('_');
        int x = int.Parse(cellname[1]);
        int y = int.Parse(cellname[2]);
        int min = 0;
        int max = 50;

        List<string> CellFlow = new List<string>();

        foreach (GameObject cell in flowsTo)
        {
            string[] FlowCellName = cell.name.Split('_');
            
            if (x + 1 == int.Parse(FlowCellName[1]) && y + 1 == int.Parse(FlowCellName[2]))
            {
                CellFlow.Add("UpRight");
            }
            else if (x + 1 == int.Parse(FlowCellName[1]) && y - 1 == int.Parse(FlowCellName[2]))
            {
                CellFlow.Add("DownRight");
            }
            else if (x - 1 == int.Parse(FlowCellName[1]) && y + 1 == int.Parse(FlowCellName[2]))
            {
                CellFlow.Add("UpLeft");
            }
            else if (x - 1 == int.Parse(FlowCellName[1]) && y - 1 == int.Parse(FlowCellName[2]))
            {
                CellFlow.Add("DownLeft");
            } 
            else if (x + 1 == int.Parse(FlowCellName[1]))
            {
                CellFlow.Add("Right");
            }
            else if (x - 1 == int.Parse(FlowCellName[1]))
            {
                CellFlow.Add("Left");
            }
            else if (y + 1 == int.Parse(FlowCellName[2]))
            {
                CellFlow.Add("Up");
            }
            else if (y - 1 == int.Parse(FlowCellName[2]))
            {
                CellFlow.Add("Down");
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
