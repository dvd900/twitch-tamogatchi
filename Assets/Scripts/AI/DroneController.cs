
using System.Collections;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    [SerializeField] private int _numItemsDestroy;
    [SerializeField] private float _scanRange;
    [SerializeField] private float _moveTime;
    [SerializeField] private float _rotateTime;
    [SerializeField] private float _idleTime;
    [SerializeField] private ActionController _actionController;
    [SerializeField] private Animator _animator;

    public bool IsMoving { get { return LeanTween.isTweening(_moveTween); } }

    private int _lastCheckedIndex;
    private int _moveTween = -1;
    private Item _scanTarget;

    private void Start()
    {
        LeanTween.cancel(_moveTween);
    }

    private void Update()
    {
        _animator.SetBool("moving", IsMoving);

        if(_actionController.CurrentAction == null && _actionController.LastAction is DroneLeaveScreenAction)
        {
            _lastCheckedIndex = (_lastCheckedIndex + 1) % AIWorldData.Instance.AllItems.Count;
            var targetItem = AIWorldData.Instance.AllItems[_lastCheckedIndex];
            if (!targetItem.isHeld && TooManyItemsInRange(targetItem.transform.position, _numItemsDestroy))
            {
                var moveTo = new FlyToAction(this, _scanTarget.transform.position);
                var scan = new DroneScanAction(this, _scanTarget, _idleTime);
                var sequence = new ActionSequence(moveTo, scan);
                _actionController.DoAction(sequence);
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
        transform.position = Vector3.Lerp(transform.position, dest, .02f);
    }

    public void DoScan(Item item)
    {
        Debug.Log("scanning");
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

    public void ResetDest()
    {
        LeanTween.cancel(_moveTween);
    }

    private bool TooManyItemsInRange(Vector3 target, int numItems)
    {
        var colliders = Physics.OverlapSphere(target, _scanRange, VBLayerMask.ItemLayerMask);
        return colliders.Length > numItems;
    }
}