
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
            while (true)
            {
                yield return null;
            }
        }

        protected override void CancelAction()
        {
            _skin.emoteController.StopSleep();
        }
    }
}
