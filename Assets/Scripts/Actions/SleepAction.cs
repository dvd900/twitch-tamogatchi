
using UnityEngine;

namespace AIActions
{
    public class SleepAction : SweeTangoAction
    {
        public SleepAction(Skin skin) : base(skin)
        {
        }

        public override void Interrupt()
        {
            _skin.emoteController.StopSleep();
        }

        public override bool IsFinished()
        {
            return false;
        }

        public override void StartAction()
        {
            _skin.emoteController.StartSleep();
        }

        public override void UpdateAction()
        {
        }
    }
}
