
using UnityEngine;

public class DroneController : MonoBehaviour
{
    [SerializeField] private int _numItemsWarning;
    [SerializeField] private int _numItemsDestroy;
    [SerializeField] private float _scanRange;

    private int _lastScannedIndex;

    private void Update()
    {
        if(Planner.Instance.WorldData.AllItems.Count > 0)
        {
            _lastScannedIndex = (_lastScannedIndex + 1) % Planner.Instance.WorldData.AllItems.Count;
            var targetItem = Planner.Instance.WorldData.AllItems[_lastScannedIndex];

            var colliders = Physics.OverlapSphere(targetItem.transform.position, _scanRange, VBLayerMask.ItemLayerMask);
        }
    }
}