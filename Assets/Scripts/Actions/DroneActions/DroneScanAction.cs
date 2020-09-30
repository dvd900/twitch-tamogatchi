using System;
using System.Collections;
using UnityEngine;

public class DroneScanAction : CoroutineAction
{
    private DroneController _drone;
    private Item _targetItem;
    private float _scanTimer;
    private bool _hasScanned;

    public DroneScanAction(DroneController drone, Item targetItem, float scanTime)
    {
        _drone = drone;
        _targetItem = targetItem;
        _scanTimer = scanTime;
    }

    protected override IEnumerator DoAction()
    {
        _drone.HoverOver(_targetItem.transform.position);
        yield return new WaitForSeconds(1.0f);
        _drone.DoScan(_targetItem);
    }
}
