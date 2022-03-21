using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueDictionarys
{

    public static Dictionary<string, CellData> valueDictionary = new Dictionary<string, CellData>();

    public static void SetupDictionarys()
    {
        valueDictionary.Add("hillslope", new CellData(0.1f, 0f, 0));
        valueDictionary.Add("channel", new CellData(0f, 0f, 0));
        valueDictionary.Add("Dam", new CellData(0.8f, 10f, 100));
        valueDictionary.Add("Leaky Dam", new CellData(0.5f, 1f, 10));
        valueDictionary.Add("Trees", new CellData(0.2f, 0f, 1));
        valueDictionary.Add("Flood Wall", new CellData(0f, 2f, 20));
        valueDictionary.Add("Dredging", new CellData(0f, 2f, 50));
        valueDictionary.Add("Flood proofing urban areas", new CellData(0, 0, 10));
    }

}
