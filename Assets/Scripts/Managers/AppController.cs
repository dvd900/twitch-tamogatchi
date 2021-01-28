using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppController : MonoBehaviour
{
    public static AppController Instance { get { return _instance; } }
    private static AppController _instance;

    [SerializeField] private string _mainSceneName;
    [SerializeField] private string _warehouseSceneName;

    [SerializeField] private Camera _sceneCam;
    [SerializeField] private RenderTexture _renderTexture;

    private void Awake()
    {
        if(_instance != null)
        {
            _sceneCam.targetTexture = _renderTexture;
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
