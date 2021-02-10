using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AppController : MonoBehaviour
{
    public static AppController Instance { get { return _instance; } }
    private static AppController _instance;

    [SerializeField] private string _mainSceneName;
    [SerializeField] private string _warehouseSceneName;

    [SerializeField] private Camera _sceneCam;
    [SerializeField] private RenderTexture _renderTexture;
    [SerializeField] private AudioListener _audioListener;
    [SerializeField] private AudioMixer _sceneMixer;

    [SerializeField] private LayerMask _screenCameraLayerMask;

    private void Awake()
    {
        if(_instance != null)
        {
            _sceneCam.targetTexture = _renderTexture;
            _sceneCam.cullingMask &= _screenCameraLayerMask.value;
            _audioListener.enabled = false;
            _sceneMixer.SetFloat("MasterVolume", -80);
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddMainScene()
    {
        SceneManager.LoadSceneAsync(_mainSceneName, LoadSceneMode.Additive);
    }
}
