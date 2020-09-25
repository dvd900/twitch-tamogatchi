
using UnityEngine;

public abstract class AIAction
{
    public abstract void StartAction();
    public abstract void UpdateAction();
    public abstract bool IsFinished();
    public abstract void Interrupt();

    public abstract float Score(AIWorldData data);
    public abstract AIAction Generate();
}
