using UnityEngine;
using System.Collections;

public class Skin : MonoBehaviour 
{
    public PickupController pickupController;
    public MovementController movementController;
    public Animator animator;

    public DynamicBone spineBone;
    public DynamicBone lArmBone;
    public DynamicBone rArmBone;

    public Transform feetTransform;
    public Transform lHandTransform;
    public Transform rHandTransform;
}
