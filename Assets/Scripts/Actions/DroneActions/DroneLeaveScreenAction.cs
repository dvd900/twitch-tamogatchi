using System;
using UnityEngine;

public class DroneLeaveScreenAction : AIAction
{
    private DroneController _drone;

    public DroneLeaveScreenAction(DroneController drone)
    {
        _drone = drone;
    }

    public override void Interrupt()
    {
        _drone.ResetDest();
    }

    public override bool IsFinished()
    {
        return _drone.IsMoving;
    }

    public override void StartAction()
    {
        Vector3 dest = CoordsUtils.RandomWorldPointOffScreen();
        _drone.FlyToDest(dest);
    }

    public override void UpdateAction()
    {
    }
}
