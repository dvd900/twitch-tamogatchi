using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter(Collision collision)
    {
        iTween.PunchScale(this.gameObject, iTween.Hash("scale", new Vector3(2f, 2f, 2f), "time", 3.0f));
        Debug.Log("Collide");
    }
}
