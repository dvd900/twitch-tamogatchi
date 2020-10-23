using AIActions;

public class SleepEffect : StatusEffect
{
    public override EffectType Type { get { return EffectType.Sleep; } }

    protected override void DoStartEffect()
    {
        _skin.actionController.DoAction(new SleepAction(_skin));
    }

    protected override void DoStopEffect()
    {
        if(_skin.actionController.CurrentAction is SleepAction)
        {
            _skin.actionController.DoAction(new IdleAction(_skin));
        }
    }

    protected override void DoUpdateEffect()
    {
    }
}