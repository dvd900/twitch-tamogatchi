using RootMotion.FinalIK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovementController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private HeadController _headController;

    /// <summary>
    /// How close he has to be to the wp to stop
    /// </summary>
    [SerializeField] private float _wpRange;
    public float WPRange { get { return _wpRange; } }

    /// <summary>
    /// speed at which he turns to look at something
    /// </summary>
    [SerializeField] private float _turnSpeed;

    public bool IsWalking { get { return _isWalking; } }
    private bool _isWalking;

    public bool IsTurning { get { return _turnTimer > 0; } }

    private int _rotDir;
    private float _turnTimer;

    private Coroutine _lookRoutine;

    private Skin _skin;

    private void Awake() {
        _skin = GetComponent<Skin>();
    }

    private void Update() {
        if(_isWalking && IsInRange()) {
            StopWalking();
        }
    }

    private void LateUpdate() {
        if (_turnTimer > 0) {
            _turnTimer -= Time.deltaTime;
        }
    }

    public void FaceCamera()
    {
        FaceTarget(LevelRefs.Instance.WorldCam.transform);
    }

    public void StopWalking() {
        _navMeshAgent.SetDestination(transform.position);
        _skin.animator.SetBool("isIdle", true);
        _isWalking = false;
    }

    public void WalkToPosition(Vector3 dest) {
        _navMeshAgent.SetDestination(dest);
        _skin.animator.SetBool("isIdle", false);
        //_skin.faceController.DoLookAt(dest);
        _isWalking = true;
    }

    public void FaceTarget(Transform target) {
        Quaternion endRot = Quaternion.LookRotation(target.position - transform.position, Vector3.up);
        Vector3 endRotEuler = endRot.eulerAngles;
        endRotEuler.x = transform.rotation.eulerAngles.x;
        endRotEuler.z = transform.rotation.eulerAngles.z;
        
        _turnTimer = Quaternion.Angle(transform.rotation, endRot) / _turnSpeed;

        var rotTween = LeanTween.rotate(gameObject, endRotEuler, _turnTimer).setEaseInOutQuad();
        rotTween.setOnComplete(_headController.ResetLookAtIK);

        _headController.SetLookAtIK(target, true);
        //_rotDir = -(int)Mathf.Sign(da);
    }
    
    private bool IsInRange() {
        return !_navMeshAgent.pathPending && 
            (_navMeshAgent.destination - transform.position).magnitude < _wpRange;
    }
}