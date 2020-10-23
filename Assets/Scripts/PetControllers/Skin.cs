using UnityEngine;
using System.Collections;

public class Skin : MonoBehaviour, AISkin
{
    public static Skin CurrentTango;
    
    public ItemController itemController;
    public MovementController movementController;
    public ActionController actionController;
    public EmoteController emoteController;
    public EffectController effectController;
    public TangoSFXController sfxController;
    public StatsController statsController;
    public SpeechController speechController;
    public HeadController headController;
    public CosmeticController cosmeticController;
    public TangoStats stats;

    public Animator animator;

    public SkinnedMeshRenderer bodyRend;
    public SkinnedMeshRenderer lEyeRend;
    public SkinnedMeshRenderer rEyeRend;
    public SkinnedMeshRenderer mouthRend;

    public SphereCollider headCollider;

    public Transform rootBone;

    public DynamicBone spineBone;
    public DynamicBone lArmBone;
    public DynamicBone rArmBone;

    public Transform feetTransform;
    public Transform lHandTransform;
    public Transform rHandTransform;
    public Transform headTransform;

    public TangoWorldData WorldData;
    
    private void Awake()
    {
        CurrentTango = this;
        WorldData = new TangoWorldData(this);
    }

    private void Update()
    {
        WorldData.UpdateData();
    }
}
