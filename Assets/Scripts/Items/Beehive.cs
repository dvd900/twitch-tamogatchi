using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beehive : MonoBehaviour, IBombable
{
    [SerializeField] private GameObject _beePrefab;
    [SerializeField] private float _spawnTime;
    [SerializeField] private int _spawnNumber;
    [SerializeField] private int _numBombsToDestroy;
    [SerializeField] private Renderer _renderer;

    private int _numBees;
    private int _numBombHits;
    private Color _startColor;

    void Start()
    {
        _startColor = _renderer.material.color;

        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while(true)
        {
            if(_numBees < _spawnNumber)
            {
                int numToSpawn = _spawnNumber - _numBees;

                Vector3 spawnCenter = new Vector3(transform.position.x, transform.position.y + 4, transform.position.z - 4);
                for (int i = 0; i < numToSpawn; i++)
                {
                    Vector3 offset = 6 * Random.onUnitSphere;
                    offset.y = Mathf.Abs(offset.y);
                    var bee = GameObject.Instantiate(_beePrefab, spawnCenter + offset, transform.rotation);
                    bee.GetComponent<BeeController>().Init(this);
                }

                LeanTween.scaleX(gameObject, (transform.localScale * 0.9f).x, 1f).setEase(LeanTweenType.punch);
                LeanTween.scaleY(gameObject, (transform.localScale * 0.9f).y, 1f).setEase(LeanTweenType.punch);
                LeanTween.scaleZ(gameObject, (transform.localScale * 1.075f).z, 0.75f).setEase(LeanTweenType.punch);

                _numBees += numToSpawn;
            }

            yield return new WaitForSeconds(_spawnTime);
        }
    }

    public void OnBeeDestroy()
    {
        _numBees--;
    }

    void IBombable.Bomb(bool closeHit, Vector3 direction)
    {
        _numBombHits++;
        if(_numBombHits == _numBombsToDestroy)
        {
            Destroy(gameObject);
        }
        else
        {
            _renderer.material.color = Color.Lerp(_startColor, Color.black, (_numBombHits / ((float)_numBombsToDestroy)));
        }
    }
}
