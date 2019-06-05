using UnityEngine;
using System.Collections;

public class Spin : MonoBehaviour
{
    public float speed = 10f;


    void Update()
    {
        transform.Rotate(Vector3.back, speed * Time.deltaTime);
    }
}