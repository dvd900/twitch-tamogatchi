
using System.Collections;
using TMPro;
using UnityEngine;

public class LabelController : MonoBehaviour
{
    [SerializeField] private TextMeshPro _label;
    /// <summary>
    /// Make negative to never disappear
    /// </summary>
    [SerializeField] private float _ttl;

    public void Init(string label)
    {
        _label.text = label;
    }

    private void Start()
    {
        if(_ttl > 0)
        {
            StartCoroutine(WaitAndDestroy(_ttl));
        }
    }

    private IEnumerator WaitAndDestroy(float time)
    {
        yield return new WaitForSeconds(time);
        GameObject.Destroy(gameObject);
    }
}