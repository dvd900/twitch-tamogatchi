using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class TangoSceneController : SceneController
{
    public static new TangoSceneController Instance { get { return _instance as TangoSceneController; } }

    [SerializeField] private RenderTexture _renderTexture;
    [SerializeField] private LayerMask _screenCameraLayerMask;

    public override void PutInBackground()
    {
        _mainCam.targetTexture = _renderTexture;
        _mainCam.cullingMask &= _screenCameraLayerMask.value;

        AudioController.Instance.DisableSceneAudio(SceneName.Tango);
    }

    public override void PutInForeground()
    {
        AudioController.Instance.EnableSceneAudio(SceneName.Tango);
    }

    private void Start()
    {
        if (WarehouseSceneController.Instance != null)
        {
            PutInBackground();
        }
        else
        {
            PutInForeground();
        }
    }
}
