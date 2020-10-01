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
                DoUnequip(_headCosmetic);
                DoEquip(cosmetic, _headTransform);
                break;
            case CosmeticLocation.Face:
                DoUnequip(_faceCosmetic);
                DoEquip(cosmetic, _faceTransform);
                break;
        }
    }

    private void DoUnequip(Cosmetic item)
    {
        if(item == null)
        {
            return;
        }

        item.transform.parent = null;
        item.DisableHolding();
    }

    private void DoEquip(Cosmetic item, Transform parent)
    {
        item.transform.parent = parent;
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;
    }
}
