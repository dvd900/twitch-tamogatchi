using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cosmetic : Item
{
    protected override bool IsPickupabble { get { return true; } }

    public CosmeticLocation Location { get { return ((CosmeticProfile)_profile)._cosmeticLocation; } }


    protected override void OnPickup()
    {
    }

    protected override void OnDrop()
    {
    }
}
