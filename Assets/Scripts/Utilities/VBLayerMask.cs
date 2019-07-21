using UnityEngine;

public static class VBLayerMask
{
    public static int Item = 1 << LayerMask.NameToLayer("Item");
    public static int Ground = 1 << LayerMask.NameToLayer("Ground");
}
