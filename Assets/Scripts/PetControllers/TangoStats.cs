
using UnityEngine;

public class TangoStats : MonoBehaviour
{

    public float AvgIdleTime { get { return _avgIdleTime; } }
    [SerializeField] private float _avgIdleTime;

    public float ActionRandomness { get { return _actionRandomness; } }
    [SerializeField] private float _actionRandomness;
}