
public class SpeedEffect : StatusEffect
{
    public override EffectType Type { get { return EffectType.Speed; } }

    public float _speedMod;

    protected override void DoStartEffect()
    {
        _skin.movementController.AddSpeedMod(_speedMod);
    }

    public override void StackEffect()
    {
        base.StackEffect();
        _skin.movementController.AddSpeedMod(_speedMod);
    }

    protected override void DoStopEffect()
    {
        _skin.movementController.AddSpeedMod(_speedMod);
    }

    protected override void DoUpdateEffect()
    {
    }
}