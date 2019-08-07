using UnityEngine;
using System.Collections;

public class LevelRefs : MonoBehaviour {
    public static LevelRefs singleton;

    public Camera worldCam;
    public Camera screenCam;

    private void Awake() {
        singleton = this;
    }
}
