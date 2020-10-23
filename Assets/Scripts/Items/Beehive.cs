using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beehive : MonoBehaviour
{
    [SerializeField] private GameObject _beePrefab;
    [SerializeField] private float _spawnTime;
    [SerializeField] private int _spawnNumber;

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while(true)
        {
            Vector3 spawnCenter = new Vector3(transform.position.x, transform.position.y + 4, transform.position.z - 4);
            for (int i = 0; i < _spawnNumber; i++)
            {
                Vector3 offset = 6 * Random.onUnitSphere;
                offset.y = Mathf.Abs(offset.y);
                var bee = GameObject.Instantiate(_beePrefab, spawnCenter + offset, transform.rotation);
                bee.GetComponent<BeeController>().Init(this);
            }

            LeanTween.scaleX(gameObject, (transform.localScale * 0.9f).x, 1f).setEase(LeanTweenType.punch);
            LeanTween.scaleY(gameObject, (transform.localScale * 0.9f).y, 1f).setEase(LeanTweenType.punch);
            LeanTween.scaleZ(gameObject, (transform.localScale * 1.075f).z, 0.75f).setEase(LeanTweenType.punch);

            yield return new WaitForSeconds(_spawnTime);
        }
    }
}
