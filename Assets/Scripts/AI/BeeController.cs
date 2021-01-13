﻿using System.Collections;
using System.Collections.Generic;
using AIActions;
using BezierSolution;
using UnityEngine;

public class BeeController : MonoBehaviour, IBombable
{
    [SerializeField] private GameObject _psHoneySplat;
    [SerializeField] private GameObject _psSting;
    [SerializeField] private float _speed;
    [SerializeField] private float _chaseRange;
    [SerializeField] private float _timeUntilChase;
    [SerializeField] private ActionController _actionController;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _targetTransform;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Collider _collider;
    [SerializeField] private Renderer _renderer;

    public float Speed { get { return _speed; } }
    public float ChaseRange { get { return _chaseRange; } }
    public float TimeUntilChase { get { return _timeUntilChase; } }
    public Beehive Hive { get { return _hive; } }
    public bool IsDying { get { return _isDying; } }

    private Beehive _hive;
    private BezierSpline _spline;
    private bool _isDying;
    private bool _isDebugBee;

    public void Init(Beehive hive, bool isDebugBee)
    {
        _hive = hive;
        _isDebugBee = isDebugBee;
    }

    private IEnumerator Start()
    {
        _psHoneySplat.transform.SetParent(null);
        gameObject.transform.localScale = new Vector3(0, 0, 0);
        LeanTween.scale(gameObject, new Vector3(0.5f,0.5f,0.5f), 1f).setEase(LeanTweenType.easeOutBack);

        // randomize speed slightly
        _speed *= 1.0f + .2f * (Random.value - .5f);

        _spline = new GameObject("spline").AddComponent<BezierSpline>();
        yield return null;

        _spline.loop = true;

        if (_isDebugBee)
        {
            _actionController.DoAction(new BeeChaseAction(this, _animator, Skin.CurrentTango, _targetTransform));
        }
        else
        {
            _actionController.DoAction(new BeeIdleAction(this, _spline));
        }
    }

    void Update()
    {
        if (_isDying)
        {
            return;
        }

        if(_actionController.CurrentAction == null)
        {
            if(_actionController.LastAction is BeeIdleAction)
            {
                _actionController.DoAction(new BeeChaseAction(this, _animator, Skin.CurrentTango, _targetTransform));
            }
            else if(_actionController.LastAction is BeeReturnToHiveAction)
            {
                _actionController.DoAction(new BeeIdleAction(this, _spline));
            }
            else if(_actionController.LastAction is BeeChaseAction)
            {
                _actionController.DoAction(new BeeReturnToHiveAction(this));
            }
        }
    }

    public void Die()
    {
        if(IsDying)
        {
            return;
        }

        Debug.Log("Dying!");
        StartCoroutine(DieRoutine());
    }
    public void Sting()
    {
        _animator.SetBool("Sting", true);
        gameObject.transform.Rotate(180f, 0.0f, 0.0f, Space.Self);
        LeanTween.scaleZ(gameObject, (transform.localScale * 3f).z, 0.3f).setEase(LeanTweenType.punch);
        LeanTween.scaleX(gameObject, (transform.localScale * 2f).x, 0.75f).setEase(LeanTweenType.punch);
        LeanTween.scaleY(gameObject, (transform.localScale * 2f).y, 0.75f).setEase(LeanTweenType.punch);
        _psSting.transform.SetParent(null);
        _psSting.SetActive(true);
    }

    private IEnumerator DieRoutine()
    {
        _animator.SetBool("Sting", false);
        _isDying = true;
        _collider.isTrigger = false;
        _rigidbody.isKinematic = false;
        _rigidbody.constraints = RigidbodyConstraints.None;
        _rigidbody.AddTorque(Vector3.back * 9500, ForceMode.Impulse);

        yield return new WaitForSeconds(0.25f);

        var fadeTween = LeanTween.alpha(_renderer.gameObject, 0, 1.0f);
        while(LeanTween.isTweening(fadeTween.id))
        {
            yield return null;
        }

        GameObject.Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if(_hive != null)
        {
            _hive.OnBeeDestroy();
        }

        if(_spline != null)
        {
            Destroy(_spline.gameObject);
        }

        Destroy(_psHoneySplat.gameObject);
        Destroy(_psSting.gameObject);
    }

    void IBombable.Bomb(bool closeHit, Vector3 direction)
    {
        Die();
    }
}
