using UnityEngine;
using System.Collections;

public class AE_Drone : MonoBehaviour
{
    [SerializeField] private DroneController _drone;

    public void AE_Zap()
    {
        Debug.Log("ZAP AE");
        _drone.DoZap();
    }
}
