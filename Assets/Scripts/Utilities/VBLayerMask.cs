using UnityEngine;

public static class VBLayerMask
{
    public static string GroundTag = "ground";
    public static string ItemTag = "item";
    public static string SweeTangoTag = "sweetango";

    public static int ItemLayer = LayerMask.NameToLayer("Item");
    public static int GroundLayer = LayerMask.NameToLayer("Ground");
    public static int SweeTangoLayer = LayerMask.NameToLayer("SweeTango");
    public static int BombableLayer = LayerMask.NameToLayer("Bombable");

    public static int ItemLayerMask = 1 << ItemLayer;
    public static int GroundLayerMask = 1 << GroundLayer;
    public static int SweeTangoLayerMask = 1 << SweeTangoLayer;
    public static int BombableLayerMask = 1 << BombableLayer;

    public static int SweeTangoItemMask = SweeTangoLayerMask | ItemLayerMask;
    public static int SweeTangoItemBombableMask = SweeTangoItemMask | BombableLayerMask;

}
