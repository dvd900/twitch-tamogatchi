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

    public void AddMainScene()
    {
        SceneManager.LoadSceneAsync(_mainSceneName, LoadSceneMode.Additive);
    }
}
