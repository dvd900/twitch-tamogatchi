using UnityEngine;

public class GhostController : MonoBehaviour
{
    [SerializeField] private float _floatWait;
    [SerializeField] private float _floatDuration;
    [SerializeField] private float _floatSpeed;

    private void Update()
    {
        if(_floatWait > 0)
        {
            _floatWait -= Time.deltaTime;
            return;
        }

        if(_floatDuration > 0)
        {
            _floatDuration -= Time.deltaTime;
            transform.position += _floatSpeed * Vector3.up;
            return;
        }

        Destroy(gameObject);
    }
}