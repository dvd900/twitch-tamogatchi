using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovementController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _navMeshAgent;
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
            float rotAmt = _turnSpeed * Time.deltaTime;
            transform.rotation *= Quaternion.AngleAxis(rotAmt, _rotDir * Vector3.up);
        }
    }

    public void FaceCamera()
    {
        LookAtPosition(LevelRefs.Instance.WorldCam.transform.position);
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

    public void LookAtPosition(Vector3 dest) {
        Quaternion endRot = Quaternion.LookRotation(dest - transform.position, Vector3.up);
        float da = Mathf.DeltaAngle(endRot.eulerAngles.y, transform.rotation.eulerAngles.y);
        if(da < 15)
        {
            return;
        }
        _turnTimer = Quaternion.Angle(transform.rotation, endRot) / _turnSpeed;
        _rotDir = -(int)Mathf.Sign(da);
        Debug.Log("Turning, time: " + _turnTimer);
    }

    private bool IsInRange() {
        return !_navMeshAgent.pathPending && 
            (_navMeshAgent.destination - transform.position).magnitude < _wpRange;
    }
}