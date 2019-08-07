using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] _items;

    void Start() {

        MessengerServer.singleton.SetHandler(NetMsgInds.SpawnMessage, OnSpawnMessage);
        MessengerServer.singleton.SetHandler(NetMsgInds.ClickMessage, OnClickMessage);
    }

    private void OnClickMessage(NetMsg msg) {
        ClickMessage click = (ClickMessage)msg;
        Vector3 screenPos = new Vector3(click.x * Screen.width, click.y * Screen.height, 0);
        SpawnApple(CoordsUtils.ScreenToWorldPos(screenPos));

    }

    private void OnSpawnMessage(NetMsg msg) {
        SpawnRandomApple();
    }

    void Update() {
        if(Input.GetButtonDown("Spawn")) {
            SpawnRandomApple();
        }
    }

    private void SpawnRandomApple() {
        SpawnApple(CoordsUtils.RandomWorldPointOnScreen());
    }

    public void SpawnApple(Vector3 worldPos) {
        Debug.Log("Spawning apple at: " + worldPos);

        GameObject clone = _items[UnityEngine.Random.Range(0, _items.Length)];

        clone = Instantiate(clone, new Vector3(worldPos.x, worldPos.y + 30, worldPos.z), 
            Quaternion.Euler(new Vector3(0, UnityEngine.Random.Range(0, 360), 0)));

        Vector3 originalScale = clone.transform.localScale;
        clone.transform.localScale = (1 / 2.5f) * originalScale;

        iTween.ScaleTo(clone.gameObject, iTween.Hash("scale", originalScale, 
            "time", 1.0f, "easetype", iTween.EaseType.easeOutElastic));

        iTween.RotateBy(clone.gameObject, iTween.Hash("amount", new Vector3(0, 1, 0),
            "time", 1f, "easetype", iTween.EaseType.easeOutSine));

    }
}
