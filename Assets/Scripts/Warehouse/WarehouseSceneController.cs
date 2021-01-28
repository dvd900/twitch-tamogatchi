using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarehouseSceneController : MonoBehaviour
{
    [SerializeField] private LaptopController _laptop;
    
    private void Start()
    {
        _laptop.TurnOn();

        //AppController.Instance.AddMainScene();
    }
}
