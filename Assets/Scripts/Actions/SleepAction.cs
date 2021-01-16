
using System.Collections;
using UnityEngine;

namespace AIActions
{
    public class SleepAction : CoroutineAction
    {
        private Skin _skin;
        private int _numItemsHit;

        public SleepAction(Skin skin)
        {
            _skin = skin;
        }

        public void OnItemHit()
        {
            _numItemsHit++;
            if(_numItemsHit >= 3)
            {
                _skin.actionController.DoAction(new IdleAction(_skin, 1.0f, false));
            }
            else
            {
                _skin.emoteController.DiscomfortEmote();
            }
        }

        protected override IEnumerator DoAction()
        {
            yield return new WaitForSeconds(.4f);
            _skin.emoteController.StartSleep();

            yield return new WaitForSeconds(3.8f);
            _skin.emoteController.EnableSleepSnot();

            while (_skin.statsController.Stamina < 100)
            {
                yield return null;
            }
            _skin.emoteController.StopSleep(false);
            yield return new WaitForSeconds(4.0f);
        }

        protected override void CancelAction()
        {
            _skin.emoteController.StopSleep(true);
            _skin.emoteController.DiscomfortEmote();
        }
    }
}
