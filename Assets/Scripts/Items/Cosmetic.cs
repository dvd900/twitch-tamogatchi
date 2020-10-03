using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cosmetic : Item
{
    protected override bool IsPickupabble { get { return true; } }

    public CosmeticLocation Location { get { return ((CosmeticProfile)_profile)._cosmeticLocation; } }

    [SerializeField] private Renderer _renderer;
    [SerializeField] private Material[] _mats;

    private void Start()
    {
        if(_mats.Length > 0)
        {
            int ind = Random.Range(0, _mats.Length);
            _renderer.material = _mats[ind];
        }
    }

    protected override void OnPickup()
    {
    }

    protected override void OnDrop()
    {
    }
}
