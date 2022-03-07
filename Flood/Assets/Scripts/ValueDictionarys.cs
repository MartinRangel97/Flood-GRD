using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueDictionarys
{

    public static Dictionary<string, CellData> valueDictionary = new Dictionary<string, CellData>();

    public static void SetupDictionarys()
    {
        valueDictionary.Add("hillslope", new CellData(0.1f, 0f));
        valueDictionary.Add("channel", new CellData(0f, 0f));
        valueDictionary.Add("Dam", new CellData(0.8f, 30f));
        valueDictionary.Add("Leaky Dam", new CellData(0.5f, 1f));
        valueDictionary.Add("Trees", new CellData(0.2f, 0f));
        valueDictionary.Add("Flood Wall", new CellData(0f, 2f));
        valueDictionary.Add("Dredging", new CellData(0f, 2f));
    }

}
