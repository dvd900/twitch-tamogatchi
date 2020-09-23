using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public static ItemSpawner singleton;

    [SerializeField] private GameObject[] _items;
    [SerializeField] private GameObject _dust;
	[SerializeField] private GameObject _spawnParticles;
	[SerializeField] private GameObject _nametagPrefab;
    [SerializeField] private AudioClip _itemHitGroundClip;
    [SerializeField] private AudioClip _itemSpawnClip;
    [SerializeField] private AudioSource _itemSFXSource;

    private void Awake()
    {
        singleton = this;
    }

    void Start()
    {
        MessengerServer.singleton.SetHandler(NetMsgInds.SpawnMessage, OnSpawnMessage);
    }

    private void OnClickMessage(NetMsg msg) {
        ClickMessage click = (ClickMessage)msg;
        Vector3 viewPos = new Vector3(click.x, click.y, 0);
        SpawnRandomItem(CoordsUtils.ViewToWorldPos(viewPos), "click message");

    }

    private void OnSpawnMessage(string msg) {
        Debug.Log(msg + "msg");
        SpawnMessage spawnMsg = JsonUtility.FromJson<SpawnMessage>(msg);
        Debug.Log(spawnMsg + " spwn " + "username: " + spawnMsg.username);
        SpawnItem(spawnMsg.itemId, CoordsUtils.ViewToWorldPos(new Vector3(spawnMsg.x, spawnMsg.y, 0)), spawnMsg.username);
    }

    void Update() {
        if(Input.GetButtonDown("Spawn")) {
            SpawnItemAtRandomPos("local player");
        }
    }

    private void SpawnItemAtRandomPos(string username) {
        SpawnRandomItem(CoordsUtils.RandomWorldPointOnScreen(), username);
    }

    public void SpawnRandomItem(Vector3 worldPos, string username) {
        SpawnItem(UnityEngine.Random.Range(0, _items.Length), worldPos, username);
        //SpawnItem(_items.Length-1, worldPos);
    }

    public void SpawnItem(int spawnInd, Vector3 worldPos, string username) {
        if(spawnInd < 0 || spawnInd >= _items.Length) {
            throw new IndexOutOfRangeException("Item ind out of range");
        }

        Item itemPrefab = _items[spawnInd].GetComponent<Item>();

        Vector3 itemPos = new Vector3(worldPos.x, worldPos.y, worldPos.z);

        var itemLabel = GameObject.Instantiate(_nametagPrefab, itemPos, Quaternion.identity);
        itemLabel.GetComponent<LabelController>().Init(username);

        if(itemPrefab.dropsIn) {
            itemPos.y = itemPos.y + 40;
        } else {
            itemPos.y = itemPrefab.transform.position.y;
        }

        Quaternion itemRot = Quaternion.AngleAxis(360 * UnityEngine.Random.value, Vector3.up) *
            itemPrefab.transform.rotation;

        GameObject newItem = Instantiate(itemPrefab.gameObject, itemPos, itemRot);
		GameObject particleEffect = Instantiate(_spawnParticles, new Vector3(itemPos.x,itemPos.y+3,itemPos.z-9),
	        Quaternion.Euler(new Vector3(0, 0, UnityEngine.Random.Range(0,360))), null) as GameObject;
		Destroy(particleEffect, 1f);

        Vector3 originalScale = newItem.transform.localScale;
        newItem.transform.localScale = (1 / 2.5f) * originalScale;

        iTween.ScaleTo(newItem.gameObject, iTween.Hash("scale", originalScale,
            "time", 1.0f, "easetype", iTween.EaseType.easeOutElastic));

        iTween.RotateBy(newItem.gameObject, iTween.Hash("amount", _items[spawnInd].transform.up,
            "time", 1f, "easetype", iTween.EaseType.easeOutSine));

		_itemSFXSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
        _itemSFXSource.PlayOneShot(_itemSpawnClip,0.2f);

        if(Skin.CurrentTango != null)
        {
            Skin.CurrentTango.headController.GlanceAtTarget(newItem.transform, true);
        }
    }

    public GameObject MakeDust() { 

        GameObject clone = Instantiate(_dust, new Vector3(transform.position.x, 
            transform.position.y - 1f, transform.position.z),
            Quaternion.Euler(new Vector3(90, 0, 0)), null) as GameObject;

        clone.transform.localScale = 2.0f * Vector3.one;

        _itemSFXSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
		_itemSFXSource.PlayOneShot(_itemHitGroundClip,0.5f);

        return clone;
    }
}
