using System;
using UnityEngine;

public class DroneScanAction : AIAction
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

    public override AIAction Generate()
    {
        throw new NotImplementedException();
    }

    public override void Interrupt()
    {
        
    }

    public override bool IsFinished()
    {
        return _scanTimer < 0;
    }

    public override float Score(AIWorldData data)
    {
        throw new NotImplementedException();
    }

    public override void StartAction()
    {
        _drone.FlyToDest(_targetItem.transform.position);
    }

    public override void UpdateAction()
    {
        _scanTimer -= Time.deltaTime;

        if(_drone.IsMoving || _targetItem == null)
        {
            return;
        }

        if(!_hasScanned)
        {
            _drone.DoScan(_targetItem);
            _hasScanned = true;
        }

        _drone.HoverOver(_targetItem.transform.position);
        
    }
}
