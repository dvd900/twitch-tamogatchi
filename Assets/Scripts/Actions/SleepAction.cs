
using System.Collections;
using UnityEngine;

namespace AIActions
{
    public class SleepAction : CoroutineAction
    {
        private Skin _skin;

        public SleepAction(Skin skin)
        {
            _skin = skin;
        }

        protected override IEnumerator DoAction()
        {
            yield return new WaitForSeconds(.4f);
            _skin.emoteController.StartSleep();
            while (_skin.statsController.Stamina < 100)
            {
                yield return null;
            }
            _skin.emoteController.StopSleep();
            yield return new WaitForSeconds(4.0f);
        }

        protected override void CancelAction()
        {
            _skin.emoteController.StopSleep();
        }
    }
}
