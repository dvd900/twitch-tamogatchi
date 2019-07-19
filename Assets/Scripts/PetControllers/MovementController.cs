using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovementController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private float wpRange;

    public bool isWalking { get; private set; }

    private Skin skin;

    private void Start() {
        skin = GetComponent<Skin>();
    }

    private void Update() {
        if(isWalking && IsInRange()) {
            StopWalking();
        }
    }

    public void StopWalking() {
        navMeshAgent.SetDestination(transform.position);
        skin.animator.SetBool("isIdle", true);
        isWalking = false;
    }

    public void WalkToPosition(Vector3 dest) {
        navMeshAgent.SetDestination(dest);
        skin.animator.SetBool("isIdle", false);
        isWalking = true;
    }

    private bool IsInRange() {
        return (navMeshAgent.destination - transform.position).magnitude < wpRange;
    }
}