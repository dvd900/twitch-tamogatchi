using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoombaLight : MonoBehaviour
{
    float rotateValue = 90;
    double fillValue = 0.75;

    // Start is called before the first frame update
    void Start()
    {
        TweenThat();
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.rotation = Quaternion.Euler(0, rotateValue, 0);
        gameObject.GetComponent<Renderer>().sharedMaterial.SetFloat("_Rotation",rotateValue);
        gameObject.GetComponent<Renderer>().sharedMaterial.SetFloat("_Fillpercentage", (float)fillValue);
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
            "time", 1.5f,
            "onupdatetarget", gameObject,
            "onupdate", "tweenOnUpdateCallBack",
            "easetype", iTween.EaseType.easeInOutQuad
            )
        );
    }
    void tweenOnUpdateCallBack(int newValue)
    {
        rotateValue = newValue;

    }

}

