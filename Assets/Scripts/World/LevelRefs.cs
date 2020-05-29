using UnityEngine;
using System.Collections;

public class LevelRefs : MonoBehaviour {
    public static LevelRefs Instance;

    public Camera WorldCam;
    public Camera ScreenCam;
    public TangoSpawner Spawner;

    private void Awake() {
        Instance = this;
    }
}
