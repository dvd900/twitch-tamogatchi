using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TangoSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _sweeTango;
    [SerializeField] private GameObject _spawnParticles;

    [SerializeField] private Transform _spawnTarget;
    [SerializeField] private Animator _animator;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _fallOnGrassClip;

    private void Start()
    {
        Spawn();
    }

    public void Spawn()
    {
        _animator.SetTrigger("spawn");
        _audioSource.Play();
    }

    private IEnumerator DoSpawnParticles()
    {
        GameObject particles = GameObject.Instantiate(_spawnParticles, _spawnTarget.position, Quaternion.identity);
        yield return new WaitForSeconds(5.0f);
        GameObject.Destroy(particles);
    }

    public void AE_DoSpawn()
    {
        StartCoroutine(DoSpawnParticles());

        NavMeshHit hit;
        if (NavMesh.SamplePosition(_spawnTarget.position, out hit, 100.0f, NavMesh.AllAreas))
        { 
            GameObject sweeT = GameObject.Instantiate(_sweeTango, hit.position, Quaternion.AngleAxis(180, Vector3.up));
            var skin = sweeT.GetComponent<Skin>();
            if(PlayerInput.Instance != null)
            {
                PlayerInput.Instance._skin = skin;
            }
            if(Planner.Instance != null)
            {
                Planner.Instance.SetPet(skin);
            }
            skin.emoteController.SpawnCheer();
            skin.sfxController.PlayLifeClip();
        }
        else
        {
            Debug.LogError("Could not find point on navmesh to spawn!!");
        }
    }

    public void AE_EggHitGround()
    {
        _audioSource.PlayOneShot(_fallOnGrassClip);
    }
}
