
using System.Collections;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    [SerializeField] private int _numItemsWarning;
    [SerializeField] private int _numItemsDestroy;
    [SerializeField] private float _scanRange;
    [SerializeField] private float _moveTime;
    [SerializeField] private float _rotateTime;
    [SerializeField] private float _idleTime;
    [SerializeField] private ActionController _actionController;
    [SerializeField] private Animator _animator;

    public bool IsMoving { get { return LeanTween.isTweening(_moveTween); } }

    private int _lastScannedIndex;
    private int _moveTween = -1;
    private Item _scanTarget;

    private void Start()
    {
        LeanTween.cancel(_moveTween);
    }

    private void Update()
    {
        _animator.SetBool("moving", IsMoving);

        if (Planner.Instance.WorldData.AllItems.Count > 0)
        {
            _lastScannedIndex = (_lastScannedIndex + 1) % Planner.Instance.WorldData.AllItems.Count;
            var targetItem = Planner.Instance.WorldData.AllItems[_lastScannedIndex];
            if (!targetItem.isHeld && CheckWarning(targetItem.transform.position))
            {
                _scanTarget = targetItem;
            }

        }

        if (_actionController.CurrentAction == null)
        {
            if(_actionController.LastAction is DroneIdleAction)
            {
                if(_scanTarget != null)
                {
                    _actionController.DoAction(new DroneScanAction(this, _scanTarget, _idleTime));
                }
                else
                {
                    Vector3 moveTarget = CoordsUtils.RandomWorldPointOnScreen();
                    moveTarget.y = transform.position.y;
                    _actionController.DoAction(new FlyToAction(this, moveTarget));
                }
            }
            else
            {
                _actionController.DoAction(new DroneIdleAction(_idleTime));
            }
        }
    }

    public void FlyToDest(Vector3 dest)
    {
        dest.y = transform.position.y;
        LeanTween.cancel(_moveTween);
        _moveTween = LeanTween.move(gameObject, dest, _moveTime).setEaseInOutQuad().id;

        var d = dest - transform.position;
        var lookTarget = Quaternion.LookRotation(d);
        LeanTween.rotate(gameObject, lookTarget.eulerAngles, _rotateTime).setEaseInOutQuad();
    }

    public void HoverOver(Vector3 dest)
    {
        dest.y = transform.position.y;
        transform.position = dest;
    }

    public void DoScan(Item item)
    {
        StartCoroutine(ScanRoutine(item));
    }

    public void ZapItems(Vector3 position)
    {

        _animator.SetTrigger("zap");
        var items = Physics.OverlapSphere(position, _scanRange, VBLayerMask.ItemLayerMask);

        Debug.Log("zapping items: #" + items.Length);
        for (int i = 0; i < items.Length; i++)
        {
            GameObject.Destroy(items[i].gameObject);
        }
    }

    private IEnumerator ScanRoutine(Item item)
    {
        _animator.SetTrigger("scan");

        float timer = 3.0f;
        while(timer > 0)
        {
            timer -= Time.deltaTime;
            DebugDraw.DrawSphere(item.transform.position, _scanRange, Color.red);

        }

        if(item == null)
        {
            yield break;
        }
        if(CheckDestroy(item.transform.position))
        {
            ZapItems(item.transform.position);

            DebugDraw.DrawSphere(item.transform.position, _scanRange, Color.green);
        }
    }

    public void ResetDest()
    {
        LeanTween.cancel(_moveTween);
    }

    public bool CheckWarning(Vector3 target)
    {
        return TooManyItemsInRange(target, _numItemsWarning);
    }

    public bool CheckDestroy(Vector3 target)
    {
        return TooManyItemsInRange(target, _numItemsDestroy);
    }

    private bool TooManyItemsInRange(Vector3 target, int numItems)
    {
        var colliders = Physics.OverlapSphere(target, _scanRange, VBLayerMask.ItemLayerMask);
        return colliders.Length > numItems;
    }
}