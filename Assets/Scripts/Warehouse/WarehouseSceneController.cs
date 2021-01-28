using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarehouseSceneController : MonoBehaviour
{
    [SerializeField] private LaptopController _laptop;
    
    private void Start()
    {
        AppController.Instance.AddMainScene();

        _laptop.TurnOn();
    }
}
