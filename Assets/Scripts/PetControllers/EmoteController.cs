using UnityEngine;
using System.Collections;

public class EmoteController : MonoBehaviour {

    [SerializeField] private EyeTrackBlink _eyes;

    public void DiscomfortEmote() {
        iTween.PunchScale(_eyes.gameObject, iTween.Hash("amount", 
            new Vector3(0f, 0.2f, 0.2f), "time", 4.0f));

        _eyes.DoDiscomfortEyes(1.0f);
    }
}
