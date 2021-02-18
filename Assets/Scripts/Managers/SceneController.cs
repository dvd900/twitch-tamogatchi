using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SceneController : MonoBehaviour
{
    public static SceneController Instance { get { return _instance; } }
    protected static SceneController _instance;

    public Transform AudioListenerTarget { get { return _audioListenerTarget; } }
    [SerializeField] protected Transform _audioListenerTarget;

    public Camera MainCam { get { return _mainCam; } }
    [SerializeField] protected Camera _mainCam;

    public virtual void PutInBackground() { }
    public virtual void PutInForeground() { }

    protected virtual void Awake()
    {
        _instance = this;
    }
}
