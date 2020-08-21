using RootMotion.FinalIK;
using System.Collections;
using UnityEngine;

public class HeadController : MonoBehaviour
{
    [SerializeField] private LookAtIK _lookAtIK;
    [SerializeField] private Transform _ikTarget;

    /// <summary>
    /// How long a glance lasts
    /// </summary>
    [SerializeField] private float _glanceTime;
    /// <summary>
    /// Time it takes to lerp IK weight
    /// </summary>
    [SerializeField] private float _lerpIKTime;

    private Transform _currentTarget;
    private Vector3 _lastTargetPos;

    private Vector3 _neutralPosOffset;
    private float _offsetY;

    private bool _ignoreY;
    private float _switchWeight;
    private float _switchWeightV;

    private int _weightTweenID;
    private Coroutine _glanceRoutine;

    private void Start()
    {
        Debug.Log("Neutral pos: " + _lookAtIK.solver.IKPosition);
        _offsetY = _ikTarget.position.y - transform.position.y;
        _neutralPosOffset = transform.InverseTransformVector(_ikTarget.position - transform.position);
        _ikTarget.SetParent(null);
        _lastTargetPos = _ikTarget.position;
        _lookAtIK.solver.target = _ikTarget;

        // initialize tween id with a dummy tween
        _weightTweenID = LeanTween.value(0, 1, .001f).id;
    }

    private void OnDestroy()
    {
        if(_ikTarget != null)
        {
            Destroy(_ikTarget);
        }
    }

    private void Update()
    {
        //Debug.Log("Weight: " + _lookAtIK.solver.IKPositionWeight + " switch weight: " + _switchWeight + " target: " + _target);
        if (_lookAtIK.solver.IKPositionWeight >= 0.999f) _lookAtIK.solver.IKPositionWeight = 1f;
        if (_lookAtIK.solver.IKPositionWeight <= 0.001f) _lookAtIK.solver.IKPositionWeight = 0f;

        if (_lookAtIK.solver.IKPositionWeight <= 0f)
        {
            return;
        }

        // Smooth target switching
        _switchWeight = Mathf.SmoothDamp(_switchWeight, 1f, ref _switchWeightV, _lerpIKTime);
        if (_switchWeight >= 0.999f) _switchWeight = 1f;

        if (_currentTarget == null)
        {
            return;
        }

        Vector3 target = _currentTarget.position;
        if(_ignoreY)
        {
            target.y = transform.position.y + _offsetY;
        }

        Vector3 lerpedTarget = Vector3.Lerp(_lastTargetPos, target, _switchWeight);

        _ikTarget.position = lerpedTarget;

    }

    public void GlanceAtTarget(Transform target, bool ignoreY)
    {
        _glanceRoutine = StartCoroutine(GlanceAtTargetRoutine(target, ignoreY));
    }

    private IEnumerator GlanceAtTargetRoutine(Transform target, bool ignoreY)
    {
        Debug.Log("Starting glance");
        SetLookAtIK(target, ignoreY);
        yield return new WaitForSeconds(_glanceTime);
        ResetLookAtIK();
        _glanceRoutine = null;
        Debug.Log("done glance");
    }

    public void SetLookAtIK(Transform target, bool ignoreY)
    {
        if(_glanceRoutine != null)
        {
            StopCoroutine(_glanceRoutine);
            Debug.Log("Cancelling glance routine");
        }

        var tween = LeanTween.descr(_weightTweenID);
        if (tween != null)
        {
            Debug.Log("Cancelling old tween");
            LeanTween.cancel(_weightTweenID);
        }

        _switchWeight = 0;
        Vector3 neutralPos = transform.position + transform.TransformVector(_neutralPosOffset);
        _lastTargetPos = Vector3.Lerp(neutralPos, _ikTarget.position, _lookAtIK.solver.IKPositionWeight);

        _currentTarget = target;
        _ignoreY = ignoreY;
        _weightTweenID = LeanTween.value(gameObject, UpdateLookIKWeight, _lookAtIK.solver.IKPositionWeight, 1, _lerpIKTime).setEaseInOutQuad().id;
    }

    private IEnumerator WaitAndTweenWeight()
    {
        yield return null;
        _weightTweenID = LeanTween.value(gameObject, UpdateLookIKWeight, _lookAtIK.solver.IKPositionWeight, 1, _lerpIKTime).setEaseInOutQuad().id;
    }

    public void ResetLookAtIK()
    {
        var tween = LeanTween.descr(_weightTweenID);
        if(tween != null)
        {
            //Debug.Log("Still tweening, so return");
            return;
        }

        _weightTweenID = LeanTween.value(gameObject, UpdateLookIKWeight, _lookAtIK.solver.IKPositionWeight, 0, _lerpIKTime).setEaseInOutQuad().id;
    }

    private void UpdateLookIKWeight(float value)
    {
        _lookAtIK.solver.IKPositionWeight = value;
    }
}
