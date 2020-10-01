using System;
using ItemSystem;

[System.Serializable]
public class CosmeticProfile : ItemProfile
{

    public CosmeticLocation _cosmeticLocation;

    public override void UpdateUniqueProperties(ItemBase itemToChangeTo)
    {
        base.UpdateUniqueProperties(itemToChangeTo);

        CosmeticProfile item = (CosmeticProfile)itemToChangeTo;

        _cosmeticLocation = item._cosmeticLocation;
    }
}
