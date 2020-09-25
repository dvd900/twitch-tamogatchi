using System;
using UnityEngine;

public class DeathAction : SweeTangoAction
{
    private const float DEATH_TIME = 5.5f;

    private float _timer;

    public DeathAction(Skin skin) : base(skin)
    {
    }

    public override AIAction Generate()
    {
        return new DeathAction(_skin);
    }

    public override void Interrupt()
    {
    }

    public override bool IsFinished()
    {
        return _timer <= 0;
    }

    public override float Score(AIWorldData data)
    {
        return -1;
    }

    public override void StartAction()
    {
        _timer = DEATH_TIME;
        _skin.movementController.StopWalking();
        _skin.movementController.FaceCamera();
        _skin.emoteController.DieEmote();
    }

    public override void UpdateAction()
    {
        _timer -= Time.deltaTime;
        if(_timer <= 0)
        {
            if(_skin.itemController.HeldItem != null)
            {
                GameObject.Destroy(_skin.itemController.HeldItem.gameObject);
            }
            GameObject.Destroy(_skin.gameObject);
            LevelRefs.Instance.Spawner.Spawn();
        }
    }
}
