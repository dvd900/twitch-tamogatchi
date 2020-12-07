using System.Collections;
using System.Collections.Generic;
using AIActions;
using BezierSolution;
using UnityEngine;

public class BeeController : MonoBehaviour, IBombable
{
    [SerializeField] private GameObject _psHoneySplat;
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
        StartCoroutine(DieRoutine());
    }

    private IEnumerator DieRoutine()
    {
        _isDying = true;
        _collider.enabled = true;
        _rigidbody.isKinematic = false;

        yield return new WaitForSeconds(1.0f);

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
    }

    void IBombable.Bomb(bool closeHit, Vector3 direction)
    {
        Die();
    }
}
