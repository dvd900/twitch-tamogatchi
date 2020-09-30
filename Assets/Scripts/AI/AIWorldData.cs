using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIWorldData {

    public static AIWorldData Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new AIWorldData();
            }
            return _instance;
        }
    }

    private static AIWorldData _instance;

    private List<Item> _allItems;
    public List<Item> AllItems { get { return _allItems; } }

    private AIWorldData()
    {
        _allItems = new List<Item>();
    }
}
