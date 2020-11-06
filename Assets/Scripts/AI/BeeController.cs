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
    [SerializeField] private Transform _targetTransform;

    public float Speed { get { return _speed; } }
    public float ChaseRange { get { return _chaseRange; } }
    public float TimeUntilChase { get { return _timeUntilChase; } }
    public Beehive Hive { get { return _hive; } }

    private Beehive _hive;
    private BezierSpline _spline;

    public void Init(Beehive hive)
    {
        _hive = hive;
    }

    private IEnumerator Start()
    {
        _psHoneySplat.transform.SetParent(null);
        gameObject.transform.localScale = new Vector3(0, 0, 0);
        LeanTween.scale(gameObject, new Vector3(1,1,1), 1f).setEase(LeanTweenType.easeOutBack);

        // randomize speed slightly
        _speed *= 1.0f + .2f * (Random.value - .5f);

        _spline = new GameObject("spline").AddComponent<BezierSpline>();
        yield return null;

        _spline.loop = true;

        _actionController.DoAction(new BeeIdleAction(this, _spline));
    }

    void Update()
    {
        if(_actionController.CurrentAction == null)
        {
            if(_actionController.LastAction is BeeIdleAction)
            {
                _actionController.DoAction(new BeeChaseAction(this, Skin.CurrentTango, _targetTransform));
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
        GameObject.Destroy(gameObject);
    }

    private void OnDestroy()
    {
        _hive.OnBeeDestroy();
        Destroy(_psHoneySplat.gameObject);
        if(_spline != null)
        {
            Destroy(_spline.gameObject);
        }
    }

    void IBombable.Bomb(bool closeHit, Vector3 direction)
    {
        Die();
    }
}
