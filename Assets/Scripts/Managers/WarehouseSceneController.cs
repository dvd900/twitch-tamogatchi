using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class WarehouseSceneController : SceneController
{
    public static WarehouseSceneController Instance { get { return _instance; } }
    private static WarehouseSceneController _instance;

    [SerializeField] private LaptopController _laptop;
    [SerializeField] private GameObject[] _objectsToDisable;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        AppController.Instance.AddTangoScene();
    }

    public override void PutInForeground()
    {
        AudioController.Instance.EnableSceneAudio(SceneName.Warehouse);

        foreach (var ob in _objectsToDisable)
        {
            ob.SetActive(true);
        }

        _laptop.ShowStats();
    }

    public override void PutInBackground()
    {
        AudioController.Instance.DisableSceneAudio(SceneName.Warehouse);

        foreach(var ob in _objectsToDisable)
        {
            ob.SetActive(false);
        }
    }
}
