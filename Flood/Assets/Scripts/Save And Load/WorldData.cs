using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class WorldData {

    public int worldWidth;
    public int worldHeight;

    public int[] riverPositions;
    public int[] residentialPositions;


    
    public WorldData(WorldManager world) {

        worldWidth = world.width;
        worldHeight = world.height;

        riverPositions = new int[world.waterLocations.Count * 2];
        for (int i = 0; i < world.waterLocations.Count; i++) {
            riverPositions[i * 2] = world.waterLocations[i].Item1;
            riverPositions[(i * 2) + 1] = world.waterLocations[i].Item2;

            //Debug.Log(world.waterLocations[i]);

        }

        residentialPositions = new int[world.ResidentialCells.Count * 2];
        for (int i = 0; i < world.ResidentialCells.Count; i++) {
            residentialPositions[i * 2] = (int)world.ResidentialCells[i].x;
            residentialPositions[(i * 2) + 1] = (int)world.ResidentialCells[i].y;
        }

    }

    
}
