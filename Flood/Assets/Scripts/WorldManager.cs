using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public class WorldManager : MonoBehaviour
{

    [SerializeField] private GameObject cellGO;
    [SerializeField] private Transform cache;

    [Range(0.05f, 2f)]
    public float timeBetweenSteps = 1f;

    private int width, height;
    private int cellWidth = 1;
    private GameObject[,] cells;
    private bool hasRained = false;
    public List<(int, int)> waterLocations = new List<(int, int)>();

    private List<GameObject> alreadyClicked = new List<GameObject>();
    private Vector2 outletLocation;
    public static int channelElevationValue = 3;


    private List<Vector2> ResidentialCells = new List<Vector2>();



    private bool autoSimulate = false;
    private bool canSimulate = false;
    private int step = 0;
    private bool simFinished = false;

    private void Awake() {
        ValueDictionarys.SetupDictionarys();
    }

    private void Start() {
        width = 51;
        height = 51;
        cells = new GameObject[width, height];
        InitialiseWorld();
        //DrawRandomLake((15, 3));
    }

    private void Update()
    {
        switch (PhaseManager.instance.currentPhase) {
            case Phase.MapEditor:
                DrawWater();
                DrawResidential();
                CalculateSlopes();
                break;

            case Phase.DefenceSetup:

                // INSERT THE DEFENCES AT THIS TIME

                if (Input.GetKeyDown(KeyCode.P)) {
                    EndDefenceSetupPhase();
                }

                break;

            case Phase.Simulation:
                if (Input.GetKeyDown(KeyCode.S)) {
                    autoSimulate = true;
                }
                if (canSimulate) {
                    if ((Input.GetKeyDown(KeyCode.Space) || autoSimulate) && !simFinished) {
                        StepThroughSimulation();
                        
                    }
                }
                break;


        }

    }

    private void CalculateSlopes() {
        if (Input.GetKeyDown(KeyCode.P)) {
            int runs = 0;
            //CalculateHillslopes(waterLocations);

            List<(int, int)> path = waterLocations;



            // Calculates the elevation of each tile based on the path of the river
            while (path.Count != 0) {
                runs++;
                path = CalculateHillslopes(path);   // THIS FUNCTION CAN BE RECURSIVE
                //Debug.Log("Run: " + runs);
            }

            //Resets the 'Activated' variable 
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    GetCellScript(x, y).Activation(false);
                }
            }
            CalculateWorldFlow();


            PhaseManager.NextPhase();
        }
    }

    public float ResidentialHealth() {
        float health = 0;
        foreach (Vector2 position in ResidentialCells) {
            health += GetCellScript((int)position.x, (int)position.y).gameObject.GetComponent<Residential>().Health;
        }
        return health;
    }

    private void DrawResidential() {
        if (Input.GetMouseButtonDown(1)) {
            GameObject clickedCell = GetTileFromClick();
            if (clickedCell == null) {
                return;
            }

            Cell cellScript = GetCellScript((int)clickedCell.transform.position.x, (int)clickedCell.transform.position.y);

            if (cellScript.GetCellType() == CellType.Channel) {
                return;
            }

            Vector2 position = (Vector2)clickedCell.transform.position;
            ResidentialCells.Add(position);
            cellScript.ChangeCellType(CellType.Urban);
            Residential residentialScript = clickedCell.AddComponent<Residential>();
            residentialScript.Setup(position);



        }
    }


    private void StepThroughSimulation() {
        StartCoroutine(Co_StepThroughSimulation(timeBetweenSteps));
        step++;
        Debug.Log("Step: " + step);
    }

    private IEnumerator Co_StepThroughSimulation(float waitTimeSeconds) {
        if (!hasRained) {
            foreach (GameObject c in cells) {
                int x = (int)c.transform.position.x;
                int y = (int)c.transform.position.y;

                GetCellScript(x, y).SetWaterLevel(1);
                //GetCellScript(x, y).ChangeColour(0, 0, 255 - 10);
            }

            hasRained = true;



        } else {
            float waterPerTile = 0;
            List<GameObject> curFlowList;
            int x;
            int y;

            // Calculate the water passed downhill

            foreach (GameObject cell in cells) {
                x = (int)cell.transform.position.x;
                y = (int)cell.transform.position.y;
                curFlowList = GetCellScript(x, y).GetFlowList();

                float waterLevel = GetCellScript(x, y).GetWaterLevel();
                if (curFlowList.Count > 0) {


                    //Calculation
                    waterPerTile = waterLevel * (1 - GetCellScript(x, y).attenuation) / curFlowList.Count;



                    foreach (GameObject c in curFlowList) {
                        c.GetComponent<Cell>().waterGainedThisCycle += waterPerTile;
                        GetCellScript(x, y).ChangeWaterLevel(-waterPerTile);
                    }

                }
            }

            ApplyWaterLevelChanges();

            // Calculate Flooding values

            foreach (GameObject cell in cells) {
                x = (int)cell.transform.position.x;
                y = (int)cell.transform.position.y;
                Cell cellScript = GetCellScript(x, y);

                List<Vector2> neighbours = GetNeighbours(new Vector2(x, y));

                if (cellScript.waterLevel > cellScript.capacity) {
                    waterPerTile = (cellScript.waterLevel - cellScript.capacity) / neighbours.Count;
                    foreach (Vector2 n in neighbours) {
                        GetCellScript((int)n.x, (int)n.y).waterGainedThisCycle += waterPerTile;
                        cellScript.waterLevel -= waterPerTile;
                    }
                }
            }

            ApplyWaterLevelChanges();


            GetCellScript((int)outletLocation.x, (int)outletLocation.y).waterLevel = 0f;

            foreach (Vector2 position in ResidentialCells) {
                Cell c = GetCellScript((int)position.x, (int)position.y);
                Residential r = c.gameObject.GetComponent<Residential>();

                r.ReduceHealth(c.GetWaterLevel());

            }

            canSimulate = false;
            yield return new WaitForSeconds(waitTimeSeconds);
            canSimulate = true;

            

        }
    }

    private void ApplyWaterLevelChanges() {
        foreach (GameObject cell in cells) {

            Cell cScript = cell.GetComponent<Cell>();


            cScript.ChangeWaterLevel(cScript.waterGainedThisCycle);
            cScript.waterGainedThisCycle = 0;
            if (cScript.waterLevel < 0.05f) {      // THRESHOLD VALUE for Flood Visual
                cScript.waterLevel = 0;
                cScript.ChangeElevation(cScript.elevation);
            } 
            
            /*else {
                cScript.ChangeColour(0, 0, (byte)(255 - (10 * cScript.GetWaterLevel())));
            }
            */

        }
    }

    private void InitialiseWorld()
    {
        cells = new GameObject[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject go = Instantiate(cellGO, new Vector2(x * cellWidth, y * cellWidth), Quaternion.identity, cache);
                cells[x, y] = go;
                go.name = "Cell_" + x + "_" + y;
                go.transform.parent = cache;
            }
        }

    }

    private Cell GetCellScript(int xIndex, int yIndex)
    {
        return cells[xIndex, yIndex].GetComponent<Cell>();
    }

    private void DrawWater()
    {



        if (Input.GetMouseButton(0) && !hasRained)
        {

            canSimulate = false;

            GameObject cellClicked = GetTileFromClick();
            if (cellClicked == null)
            {
                Debug.Log("No Cell Clicked");
                return;
            }

            if ((Vector2)cellClicked.transform.position == outletLocation)
            {
                Debug.Log("Cannot Change the Fixed River end point");
                return;
            }

            Vector3 position = cellClicked.transform.position;
            if (alreadyClicked.Contains(cellClicked))
            {
                return;
            }

            Cell c = GetCellScript((int)position.x, (int)position.y);
            switch (c.GetCellType()) {
                
                //Change hillslope to channel
                case CellType.Hillslope:
                    c.ChangeCellType(CellType.Channel);
                    waterLocations.Add(((int)position.x, (int)position.y));
                    
                    if (waterLocations.Count == 1) {
                        outletLocation = new Vector2(position.x, position.y);
                        c.isRiverEnd = true;
                    }
                    break;

                //Change urban to channel
                case CellType.Urban:
                    c.ChangeCellType(CellType.Channel);
                    waterLocations.Add(((int)position.x, (int)position.y));

                    if (waterLocations.Count == 1) {
                        outletLocation = new Vector2(position.x, position.y);
                        c.isRiverEnd = true;
                    }
                    break;

                //Change channel to hillslope
                case CellType.Channel:
                    c.ChangeCellType(CellType.Hillslope);
                    waterLocations.Remove(((int)position.x, (int)position.y));
                    break;
            }

            alreadyClicked.Add(cellClicked);
        }

        if (Input.GetMouseButtonUp(0)) {
            alreadyClicked = new List<GameObject>();
        }

    }

    

    public List<(int, int)> CalculateHillslopes(List<(int, int)> listOfLocations)
    {

        if (listOfLocations.Count == 0)
        {

            return new List<(int, int)>();

        }

        List<(int, int)> pathEdges = new List<(int, int)>();

        foreach ((int, int) cell in listOfLocations)
        {

            int currentElevation = GetCellScript(cell.Item1, cell.Item2).GetElevation();

            // TODO: refactor to use the GetNeighbours function
            List<(int, int)> surrounding = new List<(int, int)>();
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {


                    //Checking for IndexOutOfBounds -> Could also use exceptions
                    if (cell.Item1 + x >= width || cell.Item1 + x < 0 || cell.Item2 + y >= height || cell.Item2 + y < 0)
                    {
                        continue;
                    }

                    //Checking if the cell being checked has already been altered, does not need to be altered again
                    if (GetCellScript(cell.Item1 + x, cell.Item2 + y).isActive())
                    {
                        continue;
                    }

                    //Checking if the cell being checked is a channel cell -> does not need to be changed.
                    if (GetCellScript(cell.Item1 + x, cell.Item2 + y).GetCellType() == CellType.Channel)
                    {
                        continue;
                    }

                    //Center cell is the current cell, does not need to be checked
                    if (x == 0 && y == 0)
                    {
                        continue;
                    }


                    /*
                    //Checking whether the cell being checked is lower than the current elevation of the origin cell
                    if (GetCellScript(cell.Item1 + x, cell.Item2 + y).GetElevation() < currentElevation) {
                        continue;
                    }
                    */

                    GetCellScript(cell.Item1 + x, cell.Item2 + y).Activation(true);
                    surrounding.Add((cell.Item1 + x, cell.Item2 + y));
                }
            }

            foreach ((int, int) nearbyCell in surrounding)
            {
                GetCellScript(nearbyCell.Item1, nearbyCell.Item2).ChangeElevation(currentElevation + 1);
                //Debug.Log(GetCellScript(nearbyCell.Item1, nearbyCell.Item2).GetElevation());
                if (!pathEdges.Contains(nearbyCell))
                {
                    pathEdges.Add(nearbyCell);
                }
            }
        }

        //Debug.Log("Length: " + pathEdges.Count);
        return pathEdges;

        //CalculateHillslopes(pathEdges);


    }

    // Goes through each cell and determines which neighbour cells each cell will send its water to
    // NOTE: Does not work with water tiles... Or maybe it does? :D
    // Also: Gotta love that 4x for loop - we can sort this with the GetNeighbours func
    // Sidebar: This function is going to be a mess -- Less messy than it was a little while ago

    //TODO: Gotta refactor this to use the GetNeighbours function!
    private void CalculateWorldFlow() {
        foreach (GameObject c in cells) {
            c.GetComponent<Cell>().flowsTo = new List<GameObject>();
        }
        
        
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {

                Cell curCell = GetCellScript(x, y);
                int curElevation = curCell.GetElevation();

                List<(int, int)> channelNeighbours = new List<(int, int)>();    //Only relevant if the tile is a channel

                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {

                        if (x + i >= width || x + i < 0 || y + j >= height || y + j < 0)
                        {
                            continue;
                        }

                        if (i == 0 && j == 0)
                        {
                            continue;
                        }

                        // What to do if the current cell is a hillslope
                        if (curCell.GetCellType() == CellType.Hillslope)
                        {
                            if (GetCellScript(x + i, y + j).GetElevation() < curElevation)
                            {
                                curCell.SetFlowList(cells[x + i, y + j]);
                            }

                        }
                    }
                }

            }
        }

        foreach ((int, int) w in waterLocations)
        {
            GetCellScript(w.Item1, w.Item2).ChangeElevation(width * height + 1);
        }


        CalculateRiverCellElevationForFlow(outletLocation, 1);

        foreach ((int, int) w in waterLocations)
        {

            Cell c = GetCellScript(w.Item1, w.Item2);

            c.ChangeElevation(channelElevationValue);
            c.capacity = 0.01f * c.upstreamCells;   //HARDCODED VALUE
        }

        canSimulate = true;

    }

    // Recursive function for calculating the elevation of rivers when determining the direction of flow.

    public int CalculateRiverCellElevationForFlow(Vector2 cell, int elevation)
    {


        List<Vector2> temp = GetNeighbours(cell);
        List<Vector2> neighbours = new List<Vector2>();
        List<Vector2> unsetNeighbours = new List<Vector2>();
        foreach (Vector2 n in temp)
        {
            if (GetCellScript((int)n.x, (int)n.y).GetCellType() == CellType.Channel)
            {
                neighbours.Add(n);
            }
        }

        Cell c = GetCellScript((int)cell.x, (int)cell.y);

        c.ChangeElevation(elevation);
        c.flowElevation = elevation;
        foreach (Vector2 n in neighbours)
        {
            Cell cellScript = GetCellScript((int)n.x, (int)n.y);

            if (cellScript.GetElevation() == (width * height) + 1)
            {
                cellScript.ChangeElevation(elevation + 1);
                unsetNeighbours.Add(n);
                cellScript.SetFlowList(cells[(int)cell.x, (int)cell.y]);
            }
        }


        foreach (Vector2 n in unsetNeighbours)
        {
            c.upstreamCells += CalculateRiverCellElevationForFlow(n, elevation + 1);
            c.upstreamCells++;
        }

        Debug.Log("elevation: " + elevation + " upstreamCells: " + c.upstreamCells);

        return c.upstreamCells;

    }


    // Able to sort a list of Vector2s, in decending order, based on the Y value
    // Uses an insertion sort
    public List<Vector2> SortByY(List<Vector2> list)
    {
        List<Vector2> sortedList = new List<Vector2>();


        foreach (Vector2 item in list)
        {

            bool isInserted = false;
            int index = 0;

            while (!isInserted)
            {
                if (sortedList.Count == index)
                {
                    sortedList.Add(item);
                    isInserted = true;
                }
                else
                {
                    if (item.y > sortedList[index].y)
                    {
                        sortedList.Insert(index, item);
                        isInserted = true;
                    }
                    else
                    {
                        index++;
                    }
                }
            }

        }
        return sortedList;
    }

    // Gets a list of Vector2s representing the neighbours of the given cell.
    // Needs to be applied all over the document... at some point.
    public List<Vector2> GetNeighbours(Vector2 cell)
    {

        List<Vector2> neighbours = new List<Vector2>();

        for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
            {
                if (cell.x + x >= width || cell.x + x < 0 || cell.y + y >= height || cell.y + y < 0)
                {
                    continue;
                }

                if (x == 0 && y == 0)
                {
                    continue;
                }
                neighbours.Add(new Vector2(cell.x + x, cell.y + y));
            }
        }

        return neighbours;
    }



    private GameObject GetTileFromClick()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider != null)
        {

            return hit.collider.gameObject;
        }
        else
        {
            Debug.Log("No tile was clicked");

            return null;
        }
    }

    public void EndDefenceSetupPhase() {

        if (PhaseManager.instance.currentPhase == Phase.DefenceSetup) {
            simFinished = false;
            PhaseManager.NextPhase();
        }
        
    }


}