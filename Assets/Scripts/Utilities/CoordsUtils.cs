using System;
using UnityEngine;

public static class CoordsUtils {

    // Percentage of the screen not used so pet doesn't go off screen
    private const float BUFFER_PERCENTAGE = .1f;

    public static Vector3 RandomScreenPoint() {
        float x = UnityEngine.Random.value;
        float y = UnityEngine.Random.value;

        float bufferWidth = BUFFER_PERCENTAGE * Screen.width;
        float bufferHeight = BUFFER_PERCENTAGE * Screen.height;

        Vector3 mousePos = new Vector3(bufferWidth + x * (Screen.width - 2.0f * bufferWidth),
            bufferHeight + y * (Screen.height - 2.0f * bufferHeight), 0f);

        return mousePos;
    }

    public static Vector3 RandomWorldPointOnScreen() {
        Vector3 screenPoint = RandomScreenPoint();
        return ScreenToWorldPos(screenPoint);
    }

    public static Vector3 ScreenToWorldPos(Vector3 screenPos) {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000f, VBLayerMask.Ground)) {
            return hit.point;
        } else {
            throw new Exception("Ground does not cover screen??");
        }
    }
}
    
