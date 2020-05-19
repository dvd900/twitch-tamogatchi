using UnityEngine;
using System.Collections;

public class Skin : MonoBehaviour {

    public ItemController itemController;
    public MovementController movementController;
    public ActionController actionController;
    public EmoteController emoteController;
    public TangoSFXController sfxController;
    public StatsController statsController;
    public SpeechController speechController;
    public IKController ikController;

    public Animator animator;

    public SkinnedMeshRenderer bodyRend;
    public SkinnedMeshRenderer lEyeRend;
    public SkinnedMeshRenderer rEyeRend;
    public SkinnedMeshRenderer mouthRend;

    public Transform rootBone;

    public DynamicBone spineBone;
    public DynamicBone lArmBone;
    public DynamicBone rArmBone;

    public Transform feetTransform;
    public Transform lHandTransform;
    public Transform rHandTransform;

}
