using UnityEngine;

public static class VBLayerMask
{
    public static int ItemLayerMask = 1 << LayerMask.NameToLayer("Item");
    public static int GroundLayerMask = 1 << LayerMask.NameToLayer("Ground");
    public static int SweeTangoLayerMask = 1 << LayerMask.NameToLayer("SweeTango");

    public static string GroundTag = "ground";
    public static string ItemTag = "item";
    public static string SweeTangoTag = "sweetango";
}
