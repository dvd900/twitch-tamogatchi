

using System.Collections;
using UnityEngine;

namespace AIActions
{
    public class GreetAction : CoroutineAction
    {
        private Skin _skin;
        private string _newUsername;

        public GreetAction(Skin skin, string newUsername)
        {
            _skin = skin;
            _newUsername = newUsername;
        }

        protected override IEnumerator DoAction()
        {
            _skin.movementController.StopWalking();
            _skin.movementController.FaceCamera();
            _skin.emoteController.Wave();
            _skin.speechController.PrepareSpeechClip("Hello " + _newUsername);
            yield return new WaitForSeconds(.5f);
            _skin.speechController.PlayPreparedClip();
            yield return new WaitForSeconds(2.0f);
        }
    }
}