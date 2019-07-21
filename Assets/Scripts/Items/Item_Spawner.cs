using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Spawner : MonoBehaviour
{
    public GameObject item;
    // Start is called before the first frame update
    void Start()
    {

        MessengerServer.singleton.SetHandler(NetMsgInds.ClickMessage, OnClickMessage);
    }

    private void OnClickMessage(NetMsg msg) {
        ClickMessage click = (ClickMessage)msg;
        Vector3 screenPos = new Vector3(click.x * Screen.width, click.y * Screen.height, 0);
        SpawnApple(CoordsUtils.ScreenToWorldPos(screenPos));

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Spawn")) {


            SpawnApple(CoordsUtils.RandomWorldPointOnScreen());
        }
    }

    private void SpawnApple(Vector3 worldPos) {
        GameObject clone;
        clone = Instantiate(item, new Vector3(worldPos.x, worldPos.y + 10, worldPos.z), Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)));
        iTween.ScaleTo(clone.gameObject, iTween.Hash("scale", new Vector3(2.5f, 2.5f, 2.5f), "time", 1.0f, "easetype", iTween.EaseType.easeOutElastic));
        iTween.RotateBy(clone.gameObject, iTween.Hash("amount", new Vector3(0, 1, 0), "time", 1f, "easetype", iTween.EaseType.easeOutSine));

    }
}
