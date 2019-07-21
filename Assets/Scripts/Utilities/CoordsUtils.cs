using System;
using UnityEngine;

public static class CoordsUtils {
    public static Vector3 RandomScreenPoint() {
        float x = UnityEngine.Random.value;
        float y = UnityEngine.Random.value;

        Vector3 mousePos = new Vector3(x * Screen.width, y * Screen.height, 0f);
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
    
