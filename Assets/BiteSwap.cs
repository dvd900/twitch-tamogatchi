using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiteSwap : MonoBehaviour
{
public GameObject[] apples;
    int i = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            iTween.PunchScale(gameObject, iTween.Hash("amount", new Vector3(0.4f, 0.4f, 0.4f), "time", 1.0f));
            if (i == 3)
            {
                i = 0;
                    }
            apples[i].SetActive(false);
            i++;
            apples[i].SetActive(true);
        }
    }
}
