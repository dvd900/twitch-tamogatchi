using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class WarehouseSceneController : MonoBehaviour
{
    [SerializeField] private PlayableDirector _bootTimeline;
    [SerializeField] private LaptopController _laptop;
    
    private void Start()
    {
        AppController.Instance.AddMainScene();
    }
}
