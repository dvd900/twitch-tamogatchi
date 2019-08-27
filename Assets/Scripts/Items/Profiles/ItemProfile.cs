
using ItemSystem;

[System.Serializable]
public class ItemProfile : ItemBase {

    public float _value;

    public override void UpdateUniqueProperties(ItemBase itemToChangeTo) {
        base.UpdateUniqueProperties(itemToChangeTo);
        _value = ((ItemProfile)itemToChangeTo)._value;
    }
}
