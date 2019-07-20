using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovementController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private float _wpRange;

    public float walkTargetRange { get { return _walkTargetRange; } }
    [SerializeField] private float _walkTargetRange;

    public bool isWalking { get { return _isWalking; } }
    private bool _isWalking;

    private Skin _skin;

    private void Start() {
        _skin = GetComponent<Skin>();
    }

    private void Update() {
        if(_isWalking && IsInRange()) {
            StopWalking();
        }
    }

    public void StopWalking() {
        _navMeshAgent.SetDestination(transform.position);
        _skin.animator.SetBool("isIdle", true);
        _isWalking = false;
    }

    public void WalkToPosition(Vector3 dest) {
        _navMeshAgent.SetDestination(dest);
        _skin.animator.SetBool("isIdle", false);
        _isWalking = true;
    }

    private bool IsInRange() {
        return (_navMeshAgent.destination - transform.position).magnitude < _wpRange;
    }
}