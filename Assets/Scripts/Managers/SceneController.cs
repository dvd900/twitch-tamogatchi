using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SceneController : MonoBehaviour
{
    public Transform AudioListenerTarget { get { return _audioListenerTarget; } }
    [SerializeField] protected Transform _audioListenerTarget;

    public Camera MainCam { get { return _mainCam; } }
    [SerializeField] protected Camera _mainCam;

    public SceneAudioEffects SceneAudioEffects { get { return _sceneAudioEffects; } }
    [SerializeField] private SceneAudioEffects _sceneAudioEffects;

    public virtual void PutInBackground() { }
    public virtual void PutInForeground() { }
}
