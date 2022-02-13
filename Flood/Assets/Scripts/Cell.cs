using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CellType { Channel , Hillslope}

public class Cell : MonoBehaviour {

    public int elevation;

    public int flowElevation;


    public float waterLevel = 0;    // level of water on this tile
    public float waterGainedThisCycle = 0; // The amount of water this tile has gained before the end of a cycle


    private static int colourMultiplier = 3;
    private static int maxColourElevation = 255 / colourMultiplier;
    private static int elevationSharedColourMultiplier = 1; //colourMultiplier * 2 - 4;

    public List<GameObject> flowsTo = new List<GameObject>();
    public List<Vector2> directionFlowsFrom = new List<Vector2>();



    public bool isRiverStart;   // Relavent if the cell is a channel -- OBSOLETE
    public bool isRiverEnd;


    private CellType cellType;
    private bool isActivated = false;   //Is used when calculating elevation of each tile and in the Dijkstra's algorithm

    private void Start() {
        cellType = CellType.Hillslope;
        ChangeElevation(maxColourElevation);
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
            return true;
        } else {
            cellType = CellType.Hillslope;
            ChangeElevation(255);
            
            return false;
        }
    }

    public void ChangeCellType(CellType type) {
        cellType = type;
        if (cellType == CellType.Channel) {
            ChangeElevation(WorldManager.channelElevationValue);
            ChangeColour(0, 8, 0);
        } else {
            ChangeElevation(255);
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



}
