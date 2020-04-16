using System;
using UnityEngine;

public static class CoordsUtils {

    // Percentage of the screen not used so pet doesn't go off screen
    private const float BUFFER_PERCENTAGE = .1f;
    public const float EPSILON = .3f;

    public static Vector3 RandomViewPoint() {
        float x = UnityEngine.Random.value;
        float y = UnityEngine.Random.value;

        x = Mathf.Clamp(x, BUFFER_PERCENTAGE, 1.0f - BUFFER_PERCENTAGE);
        y = Mathf.Clamp(y, BUFFER_PERCENTAGE, 1.0f - BUFFER_PERCENTAGE);
        Vector3 viewPos = new Vector3(x, y, 0f);

        return viewPos;
    }

    public static Vector3 RandomWorldPointOnScreen() {
        Vector3 viewPos = RandomViewPoint();
        return ViewToWorldPos(viewPos);
    }

    public static Vector3 ViewToWorldPos(Vector3 viewPos) {
        Ray ray = LevelRefs.singleton.worldCam.ViewportPointToRay(viewPos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000f, VBLayerMask.Ground)) {
            return hit.point;
        } else {
            return Vector3.zero;
            //throw new Exception("Ground does not cover screen??");
        }
    }
}
    
