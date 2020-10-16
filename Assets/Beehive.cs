using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beehive : MonoBehaviour
{
    public GameObject bee;
    public float Timer = 3;
    private float resetTime;
    // Start is called before the first frame update
    void Start()
    {
        resetTime = Timer;
    }

    // Update is called once per frame
    void Update()
    {
        Timer -= Time.deltaTime;
        if (Timer <= 0f)
        {
            GameObject beeClone = Instantiate(bee, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y+4, gameObject.transform.position.z-4), transform.rotation) as GameObject;
            LeanTween.scaleX(gameObject, (gameObject.transform.localScale * 0.9f).x, 1f).setEase(LeanTweenType.punch);
            LeanTween.scaleY(gameObject, (gameObject.transform.localScale * 0.9f).y, 1f).setEase(LeanTweenType.punch);
            LeanTween.scaleZ(gameObject, (gameObject.transform.localScale * 1.075f).z, 0.75f).setEase(LeanTweenType.punch);

            Timer = resetTime;
        }
    }
}
