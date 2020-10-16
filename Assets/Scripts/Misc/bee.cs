using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bee : MonoBehaviour
{
    public float speed;
    private GameObject SweeT;
    // Start is called before the first frame update
    void Awake()
    {
        SweeT = GameObject.FindGameObjectWithTag("sweetango");
        gameObject.transform.LookAt(2 * transform.position - SweeT.transform.position);
    }
    void Start()
    {
        
        gameObject.transform.localScale = new Vector3(0, 0, 0);
        LeanTween.scale(gameObject, new Vector3(1,1,1), 1f).setEase(LeanTweenType.easeOutBack);
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(SweeT.transform.position.x, SweeT.transform.position.y+8, SweeT.transform.position.z), step);
        gameObject.transform.LookAt(2 * transform.position - SweeT.transform.position);
    }
}
