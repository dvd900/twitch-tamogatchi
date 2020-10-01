
using System.Collections;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    [SerializeField] private int _numItemsDestroy;
    [SerializeField] private float _scanRange;
    [SerializeField] private float _moveTime;
    [SerializeField] private float _rotateTime;
    [SerializeField] private float _idleTime;
    [SerializeField] private float _scanTime;
    [SerializeField] private float _scanCooldown;
    [SerializeField] private float _chanceToLeaveScreen;
    [SerializeField] private ActionController _actionController;
    [SerializeField] private Animator _animator;

    public bool IsMoving { get { return LeanTween.isTweening(_moveTween); } }

    private int _lastCheckedIndex;
    private int _moveTween = -1;

    private Transform _hoverTarget;
    private Vector3 _zapTarget;

    private void Start()
    {
        LeanTween.cancel(_moveTween);
    }

    private void Update()
    {
        _animator.SetBool("moving", IsMoving);

        if(_hoverTarget != null)
        {
            var dest = _hoverTarget.position;
            dest.y = transform.position.y;
            transform.position = Vector3.Lerp(transform.position, dest, .02f);
        }

        if (_actionController.CurrentAction == null)
        {
            if(_actionController.LastAction is DroneLeaveScreenAction)
            {
                if(AIWorldData.Instance.AllItems.Count > 0)
                {
                    _lastCheckedIndex = (_lastCheckedIndex + 1) % AIWorldData.Instance.AllItems.Count;
                    var targetItem = AIWorldData.Instance.AllItems[_lastCheckedIndex];
                    if (!targetItem.isHeld && TooManyItemsInRange(targetItem.transform.position, _numItemsDestroy))
                    {
                        var moveTo = new FlyToAction(this, targetItem.transform.position);
                        var scan = new DroneScanAction(this, targetItem, _idleTime);
                        _actionController.DoActionSequence(moveTo, scan);
                    }
                }
            }
            else
            {
                if (Random.value < _chanceToLeaveScreen)
                {
                    _actionController.DoAction(new DroneLeaveScreenAction(this, _scanCooldown));
                }
                else
                {
                    Vector3 moveTarget = CoordsUtils.RandomWorldPointOnScreen();
                    moveTarget.y = transform.position.y;
                    var flyAction = new FlyToAction(this, moveTarget);
                    var idleAction = new DroneIdleAction(_idleTime);
                    _actionController.DoActionSequence(flyAction, idleAction);
                }
            }
        }
    }

    public void FlyToDest(Vector3 dest)
    {
        LeanTween.cancel(_moveTween);

        dest.y = transform.position.y;
        var d = dest - transform.position;

        float scaledMoveTime = _moveTime * (d.magnitude / 200.0f);
        _moveTween = LeanTween.move(gameObject, dest, scaledMoveTime).setEaseInOutQuad().id;

        var lookTarget = Quaternion.LookRotation(d);
        LeanTween.rotate(gameObject, lookTarget.eulerAngles, _rotateTime).setEaseInOutQuad();
    }

    public void HoverOver(Transform target)
    {
        _hoverTarget = target;
        if(target != null)
        {
            LeanTween.rotate(gameObject, new Vector3(0, 180, 0), _rotateTime).setEaseInOutQuad();
        }
    }

    public void DoScan(Item item)
    {
        Debug.Log("scanning: " + item.gameObject);
        DebugDraw.DrawSphere(item.transform.position, _scanRange, Color.red, 1.0f);
        if (TooManyItemsInRange(item.transform.position, _numItemsDestroy))
        {
            _zapTarget = item.transform.position;
            _animator.SetTrigger("zap");
        }
    }

    public void DoZap()
    {
        _zapTarget.x = transform.position.x;
        _zapTarget.z = transform.position.z;
        var items = Physics.OverlapSphere(_zapTarget, _scanRange, VBLayerMask.ItemLayerMask);

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
        return colliders.Length >= numItems;
    }
}