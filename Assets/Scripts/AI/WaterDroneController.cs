using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDroneController : MonoBehaviour
{
    [SerializeField] private float _hoverHeight;
    [SerializeField] private float _entryHeight;
    [SerializeField] private float _entryTime;
    [SerializeField] private SphereCollider _douseCollider;

    private void Start()
    {
        var startPos = transform.position;
        startPos.y = _entryHeight;
        transform.position = startPos;

        StartCoroutine(DouseBombs());
    }

    private IEnumerator DouseBombs()
    {
        Vector3 target = transform.position;
        target.y = _hoverHeight;
        var moveTween = LeanTween.move(gameObject, target, _entryTime).setEaseInOutQuad();

        while(LeanTween.isTweening(moveTween.id))
        {
            yield return null;
        }

        Ray ray = new Ray(transform.position, Vector3.down);
        var hitItems = Physics.SphereCastAll(ray, _douseCollider.radius * _douseCollider.transform.lossyScale.x, 1000, VBLayerMask.ItemLayerMask);
        foreach(var hit in hitItems)
        {
            var bomb = hit.collider.gameObject.GetComponent<Bomb>();
            if(bomb != null)
            {
                bomb.Douse();
            }
        }
        

        yield return new WaitForSeconds(4.0f);

        target.y = _entryHeight;
        moveTween = LeanTween.move(gameObject, target, _entryTime).setEaseInOutQuad();

        while(LeanTween.isTweening(moveTween.id))
        {
            yield return null;
        }

        GameObject.Destroy(gameObject);
    }
}
