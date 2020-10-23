
using UnityEngine;

namespace AIActions
{
    public abstract class AIAction
    {
        public abstract void StartAction();
        public abstract void UpdateAction();
        public abstract bool IsFinished();
        public abstract void Interrupt();
    }
}
