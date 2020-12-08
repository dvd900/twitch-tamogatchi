using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beehive : MonoBehaviour, IBombable
{
    [SerializeField] private GameObject _beePrefab;
    [SerializeField] private GameObject _destroyedParticles;
    [SerializeField] private float _spawnTime;
    [SerializeField] private int _spawnNumber;
    [SerializeField] private int _numBombsToDestroy;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private bool _isDebugHive;

    private int _numBees;
    private int _numBombHits;
    private Color _startColor;
    private float _startStage;

    void Start()
    {
        _startColor = _renderer.material.GetColor("emission");
        _startStage = 0;

        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while(true)
        {
            if(_isDebugHive)
            {
                SpawnBee();
                DoSpawnAnimation();
            }
            else if(_numBees < _spawnNumber)
            {
                int numToSpawn = _spawnNumber - _numBees;

                for (int i = 0; i < numToSpawn; i++)
                {
                    SpawnBee();
                }

                DoSpawnAnimation();

                _numBees += numToSpawn;
            }

            yield return new WaitForSeconds(_spawnTime);
        }
    }

    private void DoSpawnAnimation()
    {
        LeanTween.scaleX(gameObject, (transform.localScale * 0.97f).x, 1f).setEase(LeanTweenType.punch);
        LeanTween.scaleY(gameObject, (transform.localScale * 0.97f).y, 1f).setEase(LeanTweenType.punch);
    
    }

    private void SpawnBee()
    {
        Vector3 spawnCenter = new Vector3(transform.position.x, transform.position.y + 4, transform.position.z - 4);
        Vector3 offset = 6 * Random.onUnitSphere;
        offset.y = Mathf.Abs(offset.y);
        var bee = GameObject.Instantiate(_beePrefab, spawnCenter + offset, transform.rotation);
        bee.GetComponent<BeeController>().Init(this, _isDebugHive);
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
            Vector3 spawnCenter = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            LeanTween.scale(gameObject, new Vector3(0,0,0), 0.1f).setEase(LeanTweenType.easeInBack);
            StartCoroutine(WaitAndDestroy(0.1f));
            

        }
        else
        {
            Color col = new Color(145,55,0);
            _renderer.material.SetFloat("_Stage3", Mathf.Lerp(_startStage, 1, (_numBombHits / ((float)_numBombsToDestroy))));

            LeanTween.scaleX(gameObject, (transform.localScale * 0.91f).x, 1f).setEase(LeanTweenType.punch);
            LeanTween.scaleY(gameObject, (transform.localScale * 0.91f).y, 1f).setEase(LeanTweenType.punch);
            LeanTween.scaleZ(gameObject, (transform.localScale * 1.075f).z, 0.75f).setEase(LeanTweenType.punch);
            //_renderer.material.color = Color.Lerp(_startColor, Color.black, (_numBombHits / ((float)_numBombsToDestroy)));
            //45E950
        }
    }
    private IEnumerator WaitAndDestroy(float waitTime)
    {
        Vector3 spawnCenter = new Vector3(transform.position.x, transform.position.y+10, transform.position.z);
        yield return new WaitForSeconds(waitTime);

        var destroyedSplash = Instantiate(_destroyedParticles, spawnCenter, Quaternion.identity);
        Destroy(gameObject);
    }
}
