using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveUpper : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void LateUpdate()
    {
        gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x + 0.0002f, gameObject.transform.localPosition.y + 0.0002f, gameObject.transform.localPosition.z + 0.0002f);
    }
}
