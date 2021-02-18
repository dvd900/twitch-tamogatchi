using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class TangoSceneController : SceneController
{
    public static TangoSceneController Instance { get { return _instance; } }
    private static TangoSceneController _instance;

    [SerializeField] private RenderTexture _renderTexture;
    [SerializeField] private LayerMask _screenCameraLayerMask;

    private LayerMask _defaultCullingMask;
    private LayerMask _disabledCullingMask;

    private void Awake()
    {
        _instance = this;
        _defaultCullingMask = _mainCam.cullingMask;
        _disabledCullingMask = _defaultCullingMask & _screenCameraLayerMask;
    }

    public override void PutInBackground()
    {
        _mainCam.targetTexture = _renderTexture;
        _mainCam.cullingMask = _disabledCullingMask;

        AudioController.Instance.DisableSceneAudio(SceneName.Tango);
    }

    public override void PutInForeground()
    {
        _mainCam.targetTexture = null;
        _mainCam.cullingMask = _defaultCullingMask;
        AudioController.Instance.EnableSceneAudio(SceneName.Tango);
    }

    private void Start()
    {
        if (WarehouseSceneController.Instance != null)
        {
            Debug.Log("PUTTING IN BG");
            PutInBackground();
        }
        else
        {
            if(Skin.CurrentTango == null)
            {
                LevelRefs.Instance.Spawner.Spawn();
            }

            PutInForeground();
        }
    }
}
