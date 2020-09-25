
using UnityEngine;

public class SleepAction : SweeTangoAction
{
    public SleepAction(Skin skin) : base(skin)
    {
    }

    public override AIAction Generate()
    {
        return new SleepAction(_skin);
    }

    public override void Interrupt()
    {
        _skin.emoteController.StopSleep();
    }

    public override bool IsFinished()
    {
        return false;
    }

    public override float Score(AIWorldData data)
    {
        return 0;
    }

    public override void StartAction()
    {
        _skin.emoteController.StartSleep();
    }

    public override void UpdateAction()
    {
    }
}

