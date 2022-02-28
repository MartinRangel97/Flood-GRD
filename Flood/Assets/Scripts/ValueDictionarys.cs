using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueDictionarys
{

    public static Dictionary<string, CellData> valueDictionary = new Dictionary<string, CellData>();

    public static void SetupDictionarys()
    {
        valueDictionary.Add("hillslope", new CellData(0.1f, 0f));
        valueDictionary.Add("channel", new CellData(0f, 1f));
    }

}
