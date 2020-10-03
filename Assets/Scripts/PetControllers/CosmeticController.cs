using UnityEngine;
using System.Collections;

public enum CosmeticLocation
{
    Head,
    Face
}

public class CosmeticController : MonoBehaviour
{
    [SerializeField] private Transform _headTransform;
    [SerializeField] private Transform _faceTransform;

    private Skin _skin;

    private Cosmetic _headCosmetic;
    private Cosmetic _faceCosmetic;

    private void Awake()
    {
        _skin = GetComponent<Skin>();
    }

    public void EquipItem(Cosmetic cosmetic)
    {
        switch(cosmetic.Location)
        {
            case CosmeticLocation.Head:
                DoUnequip(ref _headCosmetic);
                DoEquip(cosmetic, _headTransform, ref _headCosmetic);
                break;
            case CosmeticLocation.Face:
                DoUnequip(ref _faceCosmetic);
                DoEquip(cosmetic, _faceTransform, ref _faceCosmetic);
                break;
        }
    }

    private void DoUnequip(ref Cosmetic slot)
    {
        if(slot == null)
        {
            return;
        }

        slot.transform.parent = null;
        slot.DisableHolding();

        Vector3 launchVec = _skin.transform.forward;
        launchVec.y = 1.0f;

        slot.Rigidbody.AddForce(1700 * slot.Rigidbody.mass * launchVec);

        slot = null;
    }

    private void DoEquip(Cosmetic item, Transform parent, ref Cosmetic slot)
    {
        item.transform.SetParent(parent, true);
        item.transform.localPosition = Vector3.zero;
        item.transform.rotation = Quaternion.identity;
        slot = item;

    }
}
