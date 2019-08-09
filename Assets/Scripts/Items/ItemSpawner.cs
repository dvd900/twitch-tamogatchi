using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public static ItemSpawner singleton;

    [SerializeField] private GameObject[] _items;
    [SerializeField] private GameObject _dust;

    private void Awake() {
        singleton = this;
    }

    void Start() {

        MessengerServer.singleton.SetHandler(NetMsgInds.SpawnMessage, OnSpawnMessage);
        MessengerServer.singleton.SetHandler(NetMsgInds.ClickMessage, OnClickMessage);
    }

    private void OnClickMessage(NetMsg msg) {
        ClickMessage click = (ClickMessage)msg;
        Vector3 screenPos = new Vector3(click.x * Screen.width, click.y * Screen.height, 0);
        SpawnRandomItem(CoordsUtils.ScreenToWorldPos(screenPos));

    }

    private void OnSpawnMessage(NetMsg msg) {
        SpawnMessage spawnMsg = (SpawnMessage)msg;
        SpawnItem(spawnMsg.itemInd, CoordsUtils.ScreenToWorldPos(new Vector3(spawnMsg.x, spawnMsg.y, 0)));
    }

    void Update() {
        if(Input.GetButtonDown("Spawn")) {
            SpawnItemAtRandomPos();
        }
    }

    private void SpawnItemAtRandomPos() {
        SpawnRandomItem(CoordsUtils.RandomWorldPointOnScreen());
    }

    public void SpawnRandomItem(Vector3 worldPos) {
        SpawnItem(UnityEngine.Random.Range(0, _items.Length), worldPos);
    }

    public void SpawnItem(int spawnInd, Vector3 worldPos) {
        if(spawnInd < 0 || spawnInd >= _items.Length) {
            throw new IndexOutOfRangeException("Item ind out of range");
        }

        GameObject clone = _items[spawnInd];

        clone = Instantiate(clone, new Vector3(worldPos.x, worldPos.y + 30, worldPos.z),
           Quaternion.Euler(new Vector3(0, UnityEngine.Random.Range(0, 360), 0)));

        Vector3 originalScale = clone.transform.localScale;
        clone.transform.localScale = (1 / 2.5f) * originalScale;

        iTween.ScaleTo(clone.gameObject, iTween.Hash("scale", originalScale,
            "time", 1.0f, "easetype", iTween.EaseType.easeOutElastic));

        iTween.RotateBy(clone.gameObject, iTween.Hash("amount", new Vector3(0, 1, 0),
            "time", 1f, "easetype", iTween.EaseType.easeOutSine));
    }

    public GameObject MakeDust() { 

        GameObject clone = Instantiate(_dust, new Vector3(transform.position.x, 
            transform.position.y - 1.5f, transform.position.z),
            Quaternion.Euler(new Vector3(90, 0, 0)), null) as GameObject;

        clone.transform.localScale = 2.0f * Vector3.one;

        return clone;
    }
}
