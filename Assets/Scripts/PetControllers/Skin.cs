using UnityEngine;
using System.Collections;

public class Skin : MonoBehaviour {

    public ItemController itemController;
    public MovementController movementController;
    public ActionController actionController;
    public EmoteController emoteController;

    public Animator animator;

    public DynamicBone spineBone;
    public DynamicBone lArmBone;
    public DynamicBone rArmBone;

    public Transform feetTransform;
    public Transform lHandTransform;
    public Transform rHandTransform;

}
