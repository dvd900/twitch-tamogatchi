using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public static ItemSpawner singleton;

    [SerializeField] private GameObject[] _items;
    [SerializeField] private GameObject _dust;
    [SerializeField] private AudioClip _itemHitGroundClip;
    [SerializeField] private AudioClip _itemSpawnClip;
    [SerializeField] private AudioSource _itemSFXSource;

    private void Awake() {
        singleton = this;
    }

    void Start() {

        MessengerServer.singleton.SetHandler(NetMsgInds.SpawnMessage, OnSpawnMessage);
    }

    private void OnClickMessage(NetMsg msg) {
        ClickMessage click = (ClickMessage)msg;
        Vector3 viewPos = new Vector3(click.x, click.y, 0);
        SpawnRandomItem(CoordsUtils.ViewToWorldPos(viewPos));

    }

    private void OnSpawnMessage(string msg) {
        Debug.Log(msg + "msg");
        SpawnMessage spawnMsg = JsonUtility.FromJson<SpawnMessage>(msg);
        Debug.Log(spawnMsg + "spwn");
        SpawnItem(spawnMsg.itemId, CoordsUtils.ViewToWorldPos(new Vector3(spawnMsg.x, spawnMsg.y, 0)));
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
        //SpawnItem(_items.Length-1, worldPos);
    }

    public void SpawnItem(int spawnInd, Vector3 worldPos) {
        if(spawnInd < 0 || spawnInd >= _items.Length) {
            throw new IndexOutOfRangeException("Item ind out of range");
        }

        Item itemPrefab = _items[spawnInd].GetComponent<Item>();

        Vector3 itemPos = new Vector3(worldPos.x, worldPos.y, worldPos.z);
        if(itemPrefab.dropsIn) {
            itemPos.y = itemPos.y + 40;
        } else {
            itemPos.y = itemPrefab.transform.position.y;
        }

        Quaternion itemRot = Quaternion.AngleAxis(360 * UnityEngine.Random.value, Vector3.up) *
            itemPrefab.transform.rotation;

        GameObject clone = Instantiate(itemPrefab.gameObject, itemPos, itemRot);

        Vector3 originalScale = clone.transform.localScale;
        clone.transform.localScale = (1 / 2.5f) * originalScale;

        iTween.ScaleTo(clone.gameObject, iTween.Hash("scale", originalScale,
            "time", 1.0f, "easetype", iTween.EaseType.easeOutElastic));

        iTween.RotateBy(clone.gameObject, iTween.Hash("amount", _items[spawnInd].transform.up,
            "time", 1f, "easetype", iTween.EaseType.easeOutSine));

        _itemSFXSource.PlayOneShot(_itemSpawnClip);
    }

    public GameObject MakeDust() { 

        GameObject clone = Instantiate(_dust, new Vector3(transform.position.x, 
            transform.position.y - 1.5f, transform.position.z),
            Quaternion.Euler(new Vector3(90, 0, 0)), null) as GameObject;

        clone.transform.localScale = 2.0f * Vector3.one;

        _itemSFXSource.PlayOneShot(_itemHitGroundClip);

        return clone;
    }
}
