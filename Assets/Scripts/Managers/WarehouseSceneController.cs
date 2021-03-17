using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class WarehouseSceneController : SceneController
{
    public static WarehouseSceneController Instance { get { return _instance; } }
    private static WarehouseSceneController _instance;

    [SerializeField] private Animator _droneAnimator;
    [SerializeField] private PlayableDirector _bootTimeline;
    [SerializeField] private LaptopController _laptop;
    [SerializeField] private GameObject[] _objectsToDisable;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        AppController.Instance.AddTangoScene();
        PlayEntryAnimation();
    }

    private void PlayEntryAnimation()
    {
        AudioController.Instance.EnableSceneAudio(SceneName.Warehouse);

        _droneAnimator.SetTrigger("zoom");

        _bootTimeline.time = 0;
        _bootTimeline.Play();
    }

    public override void PutInForeground()
    {
        PlayEntryAnimation();
        Debug.Log("putting warehouse in foreground");

        foreach (var ob in _objectsToDisable)
        {
            ob.SetActive(true);
        }

        //_laptop.ShowStats();
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
