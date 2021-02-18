using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Audio;

[Serializable]
public class SceneAudioEffects
{
    public float FilterCutoff;
}

[RequireComponent(typeof(AppController))]
public class AudioController : MonoBehaviour
{
    private const float MAX_SCENE_DB = 0;
    private const float MIN_SCENE_DB = -80;

    public static AudioController Instance { get { return _instance; } }
    private static AudioController _instance;

    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private GameObject _audioListenerPrefab;

    private Transform _audioListener;
    private AudioLowPassFilter _lowPassFilter;

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        var listenerObj = GameObject.Instantiate(_audioListenerPrefab, AppController.Instance.ActiveScene.AudioListenerTarget);
        listenerObj.name = "AUDIO_LISTENER";

        _audioListener = listenerObj.transform;
        _lowPassFilter = listenerObj.GetComponent<AudioLowPassFilter>();
    }

    public void EnableSceneAudio(SceneName scene)
    {
        var sceneController = AppController.Instance.GetSceneController(scene);

        _mixer.SetFloat(GetMixerParam(scene), MAX_SCENE_DB);
        _audioListener.SetParent(sceneController.AudioListenerTarget);
        _audioListener.localPosition = Vector3.zero;
        _lowPassFilter.cutoffFrequency = sceneController.SceneAudioEffects.FilterCutoff;
    }

    public void DisableSceneAudio(SceneName scene)
    {
        var sceneController = AppController.Instance.GetSceneController(scene);

        _mixer.SetFloat(GetMixerParam(scene), MIN_SCENE_DB);
    }

    private IEnumerator FadeVolume(string paramName, float target, Transform listenerTarget, float fadeTime)
    {
        if(fadeTime == 0)
        {
            _mixer.SetFloat(paramName, target);
            _audioListener.SetParent(listenerTarget);
            _audioListener.localPosition = Vector3.zero;
            yield break;
        }

        float time = 0;

        float startParam;
        Assert.IsTrue(_mixer.GetFloat(paramName, out startParam));

        Vector3 listenerStartPos = _audioListener.position;

        while(time < fadeTime)
        {
            float t = time / fadeTime;
            _mixer.SetFloat(paramName, Mathf.Lerp(startParam, target, t));
            _audioListener.position = Vector3.Lerp(listenerStartPos, listenerTarget.position, t);

            time += Time.deltaTime;
            yield return null;
        }

        _mixer.SetFloat(paramName, target);
        _audioListener.SetParent(listenerTarget);
        _audioListener.localPosition = Vector3.zero;
    }

    private string GetMixerParam(SceneName scene)
    {
        switch(scene)
        {
            case SceneName.Tango:
                return "TangoMasterVolume";
            case SceneName.Warehouse:
                return "WarehouseMasterVolume";
        }

        return null;
    }
}
