using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoombaLight : MonoBehaviour
{
    float rotateValue = 0;

    // Start is called before the first frame update
    void Start()
    {
        TweenThat();
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<Renderer>().sharedMaterial.SetFloat("_Rotation",rotateValue);
        if (rotateValue >=360)
        {
            rotateValue = 0;
            TweenThat();
        }
    }

    void TweenThat()
    {
        iTween.ValueTo(gameObject, iTween.Hash(
            "from", rotateValue,
            "to", 360,
            "time", 3f,
            "onupdatetarget", gameObject,
            "onupdate", "tweenOnUpdateCallBack",
            "easetype", iTween.EaseType.easeInOutQuart
            )
        );
    }
    void tweenOnUpdateCallBack(int newValue)
    {
        rotateValue = newValue;
    }

}

