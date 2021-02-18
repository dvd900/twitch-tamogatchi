using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public enum SceneName
{
    Tango,
    Warehouse
}

public class AppController : MonoBehaviour
{
    public static AppController Instance { get { return _instance; } }
    private static AppController _instance;

    [SerializeField] private string _tangoSceneName;
    [SerializeField] private string _warehouseSceneName;

    public SceneController ActiveScene { get { return _activeScene; } }
    private SceneController _activeScene;

    private void Awake()
    {
        if(_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if(WarehouseSceneController.Instance != null)
        {
            _activeScene = WarehouseSceneController.Instance;
        }
        else
        {
            _activeScene = TangoSceneController.Instance;
        }
    }

    public SceneController GetSceneController(SceneName scene)
    {
        switch (scene)
        {
            case SceneName.Tango:
                return TangoSceneController.Instance;
            case SceneName.Warehouse:
                return WarehouseSceneController.Instance;
        }

        return null;
    }

    public void AddTangoScene()
    {
        SceneManager.LoadSceneAsync(_tangoSceneName, LoadSceneMode.Additive);
    }

    public void ChangeActiveScene(SceneName scene)
    {
        _activeScene.PutInBackground();
        var newScene = GetSceneController(scene);
        newScene.PutInForeground();

        _activeScene = newScene;
    }
}
