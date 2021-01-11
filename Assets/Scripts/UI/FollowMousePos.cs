using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMousePos : MonoBehaviour
{
    public int itemNum;
    public Canvas myCanvas;

    // Start is called before the first frame update
    void Start()
    {
        myCanvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();
        gameObject.transform.parent = myCanvas.transform.Find("Panel").transform;
        gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.3f);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 viewMousePos = new Vector2(mousePos.x / Screen.width, mousePos.y / Screen.height);
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(myCanvas.transform as RectTransform, Input.mousePosition, myCanvas.worldCamera, out pos);
        transform.position = myCanvas.transform.TransformPoint(new Vector2(pos.x,pos.y+30));
        if (Input.GetMouseButtonDown(0))
        {
            DoClick(viewMousePos);
        }
    }
    private void DoClick(Vector2 viewPos)
    {
        Vector3 worldPos = CoordsUtils.ViewToWorldPos(viewPos);
        //_itemSpawner.SpawnItem(3, worldPos);
        ItemSpawner.Instance.SpawnIconItem(worldPos, "local player",itemNum);
        Destroy(this.gameObject);
    }
}
