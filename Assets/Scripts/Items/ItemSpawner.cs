using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public static ItemSpawner Instance;

    [SerializeField] private Canvas _myCanvas;
    [SerializeField] private GameObject[] _icons;
    [SerializeField] private int _itemCost;
    [SerializeField] private SweetokenGenerator _playerTokens;

    [SerializeField] private GameObject[] _items;
    [SerializeField] private GameObject[] _hazards;

    [SerializeField] private GameObject _dust;
	[SerializeField] private GameObject _spawnParticles;
	[SerializeField] private GameObject _nametagPrefab;
    [SerializeField] private AudioClip _itemHitGroundClip;
    [SerializeField] private AudioClip _itemSpawnClip;
    [SerializeField] private AudioSource _itemSFXSource;

    [SerializeField] private float _hazardSpawnTime;

    private GameObject _currentHazard;
    private float _timerHazardSpawn;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        MessengerServer.singleton.SetHandler(NetMsgInds.SpawnMessage, OnSpawnMessage);
        _timerHazardSpawn = _hazardSpawnTime;
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
        SpawnItem(_items[spawnMsg.itemId], CoordsUtils.ViewToWorldPos(new Vector3(spawnMsg.x, spawnMsg.y, 0)), spawnMsg.username);
    }

    void Update() {
        if(Input.GetButtonDown("Spawn")) {
            SpawnItemAtRandomPos("local player");
        }

        //if(_currentHazard == null)
        //{
            _timerHazardSpawn -= Time.deltaTime;
            if(_timerHazardSpawn < 0)
            {
                int hazardInd = UnityEngine.Random.Range(0, _hazards.Length);
                _currentHazard = SpawnItem(_hazards[hazardInd], CoordsUtils.RandomWorldPointOnScreen(), null);
                _timerHazardSpawn = _hazardSpawnTime;
            }
        //}
    }

    private void SpawnItemAtRandomPos(string username) {
        SpawnRandomItem(CoordsUtils.RandomWorldPointOnScreen(), username);
    }

    public void SpawnRandomItem(Vector3 worldPos, string username) {
        SpawnItem(_items[UnityEngine.Random.Range(0, _items.Length)], worldPos, username);
        //SpawnItem(_items.Length-1, worldPos);
    }
    public void SpawnIconItem(Vector3 worldPos, string username, int itemArrayNum)
    {
        if (_playerTokens.tokensCount >= _itemCost)
        {
            SpawnItem(_items[itemArrayNum], worldPos, username);
            _playerTokens.tokensCount -= _itemCost;
        }
        else
        {
            Debug.Log("You dont have enough SweeTokens!");
        }
        //SpawnItem(_items.Length-1, worldPos);
    }

    public Bomb SpawnBomb(Vector3 worldPos, string username)
    {
        return SpawnItem(_items[1], worldPos, username).GetComponent<Bomb>();
    }

    public GameObject SpawnItem(GameObject prefab, Vector3 worldPos, string username) {

        Item itemPrefab = prefab.GetComponent<Item>();

        Vector3 itemPos = new Vector3(worldPos.x, worldPos.y, worldPos.z);

        if(!string.IsNullOrEmpty(username))
        {
            var itemLabel = GameObject.Instantiate(_nametagPrefab, itemPos, Quaternion.identity);
            itemLabel.GetComponent<LabelController>().Init(username);
        }

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

        iTween.RotateBy(newItem.gameObject, iTween.Hash("amount", itemPrefab.transform.up,
            "time", 1f, "easetype", iTween.EaseType.easeOutSine));

		_itemSFXSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
        _itemSFXSource.PlayOneShot(_itemSpawnClip,0.2f);

        if(Skin.CurrentTango != null
            && !Skin.CurrentTango.IsSleeping
            && !Skin.CurrentTango.IsDying
            && !Skin.CurrentTango.emoteController.IsDoingEmote)
        {
            Skin.CurrentTango.headController.GlanceAtTarget(newItem.transform, true);
        }

        return newItem;
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

    public void HotbarSpawn(int itemNum)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_myCanvas.transform as RectTransform, Input.mousePosition, _myCanvas.worldCamera, out pos);
        Instantiate(_icons[itemNum], pos, Quaternion.identity);
    }
    public void GetCost(int itemCost)
    {
        _itemCost = itemCost;
    }
}
