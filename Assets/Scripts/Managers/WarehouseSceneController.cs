using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class WarehouseSceneController : SceneController
{
    public static new WarehouseSceneController Instance { get { return _instance as WarehouseSceneController; } }

    private void Start()
    {
        PutInForeground();
    }

    public override void PutInForeground()
    {
        AudioController.Instance.EnableSceneAudio(SceneName.Warehouse);
    }
}
