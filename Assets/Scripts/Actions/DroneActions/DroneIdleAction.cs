﻿using System;
using UnityEngine;

public class DroneIdleAction : AIAction
{
    private float _idleTimer;

    public DroneIdleAction(float idleTime)
    {
        _idleTimer = idleTime;
    }

    public override void Interrupt()
    {
        
    }

    public override bool IsFinished()
    {
        return _idleTimer <= 0;
    }

    public override void StartAction()
    {
    }

    public override void UpdateAction()
    {
        _idleTimer -= Time.deltaTime;
    }
}
