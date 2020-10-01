using System;
using System.Collections;
using UnityEngine;

public class DroneScanAction : CoroutineAction
{
    private DroneController _drone;
    private Item _targetItem;
    private float _scanTime;

    public DroneScanAction(DroneController drone, Item targetItem, float scanTime)
    {
        _drone = drone;
        _targetItem = targetItem;
        _scanTime = scanTime;
    }

    protected override IEnumerator DoAction()
    {
        if(_targetItem != null)
        {
            _drone.HoverOver(_targetItem.transform);
        }
        
        yield return new WaitForSeconds(_scanTime);

        if(_targetItem == null)
        {
            yield break;
        }

        _drone.DoScan(_targetItem);

        yield return new WaitForSeconds(8.0f);

        _drone.HoverOver(null);
    }

    protected override void CancelAction()
    {
        _drone.HoverOver(null);
    }
}
