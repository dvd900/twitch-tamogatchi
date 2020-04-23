using UnityEngine;
using System.Collections;

public class FaceController : MonoBehaviour {

    [SerializeField] private float _timeBetweenBlinks;
    [SerializeField] private float _blinkTime;
    [SerializeField] private float _chewFrameLength;

    [SerializeField] private Material _lEyeOuchMat;
    [SerializeField] private Material _rEyeOuchMat;

    [SerializeField] private Material _lEyeClosedMat;
    [SerializeField] private Material _rEyeClosedMat;

    [SerializeField] private Material _eyeHappyClosedMat;

    [SerializeField] private Transform _headBone;

    [SerializeField] private float _lookTexOffset;

    private Skin _skin;

    private FrameAnimation _lEyeBlinkAnim;
    private FrameAnimation _rEyeBlinkAnim;
    private bool _blinkEnabled;

    private FrameAnimation _chewAnim;
    private bool _chewing;

    private Material _lEyeNormalMat;
    private Material _rEyeNormalMat;

    private Transform _lookTarget;
    private bool _isLooking;

    public bool IsChewing { get { return _chewing; } }

    private void Start() {
        _skin = GetComponent<Skin>();

        _lEyeNormalMat = _skin.lEyeRend.materials[0];
        _rEyeNormalMat = _skin.rEyeRend.materials[0];

        _lEyeBlinkAnim = new TexOffsetAnimation(_skin.lEyeRend, 0,
            VBShaderUtils.P_MAIN_TEX, _blinkTime, 4, 2, _timeBetweenBlinks);

        _rEyeBlinkAnim = new TexOffsetAnimation(_skin.rEyeRend, 0,
            VBShaderUtils.P_MAIN_TEX, _blinkTime, 4, 2, _timeBetweenBlinks);

        _chewAnim = new TexOffsetAnimation(_skin.mouthRend, 2, 
            VBShaderUtils.P_DETAIL_TEX, _chewFrameLength, 2, 1, 0);

        DoNormalEyes();
    }

    private void Update() {

        if (_blinkEnabled) {
            _lEyeBlinkAnim.UpdateAnim();
            _rEyeBlinkAnim.UpdateAnim();
        }

        if(_chewing) {
            _chewAnim.UpdateAnim();
        }
    }

    //look in late update so the animator doesn't override head rotation
    private void LateUpdate() {
        if (_isLooking) {
            DoLooking();
        }
    }

    private void DoLooking() {
        return;
        Vector3 d = transform.position - _lookTarget.position;
        d.Normalize();
        Vector3 dLocal = transform.InverseTransformVector(d);

        Vector2 lOffset = _lookTexOffset * new Vector2(-dLocal.x, -dLocal.z);
        Vector2 rOffset = lOffset;
        rOffset.x = 1.0f - rOffset.x;

        _skin.lEyeRend.materials[0].SetTextureOffset(VBShaderUtils.P_DETAIL_TEX, lOffset);
        _skin.rEyeRend.materials[0].SetTextureOffset(VBShaderUtils.P_DETAIL_TEX, rOffset);

        //_headBone.LookAt(_lookAtPoint);
        //_headBone.rotation = Quaternion.FromToRotation(-transform.right, d);
        //_headBone.LookAt(_lookAtPoint);
        //_headBone.RotateAround(Vector3.up, 90);
        //_headBone.localRotation = Quaternion.Lerp(Quaternion.identity, _headBone.localRotation, .5f);
    }

    public void DoNormalEyes() {
        SetEyeMats(_lEyeNormalMat, _rEyeNormalMat);

        StartBlink();
    }

    public void DoDiscomfortEyes() {
        StopBlink();

        SetEyeMats(_lEyeOuchMat, _rEyeOuchMat);
    }

    public void DoClosedEyes() {
        StopBlink();

        SetEyeMats(_lEyeClosedMat, _rEyeClosedMat);
    }

    public void DoHappyClosedEyes() {
        StopBlink();

        SetEyeMats(_eyeHappyClosedMat, _eyeHappyClosedMat);
    }

    public void SetLookAt(Transform target) {
        _isLooking = true;
        _lookTarget = target;
        //_skin.ikController.SetLookAt(target);
        //_lookAtPoint.y = _headBone.position.y;
    }

    public void StopLookingAt() {
        _isLooking = false;
        _lookTarget = null;
        //_skin.ikController.ClearLookAt();
    }

    public void DoChewMouth() {
        Debug.Log("CHEW mouth");
        _chewing = true;
    }

    public void DoNormalMouth() {
        Debug.Log("normal mouth");
        _chewAnim.ResetAnim();
        _chewing = false;
    }

    private void StartBlink() {
        _lEyeBlinkAnim.ResetAnim();
        _rEyeBlinkAnim.ResetAnim();
        _blinkEnabled = true;
    }

    private void StopBlink() {
        _lEyeBlinkAnim.ResetAnim();
        _rEyeBlinkAnim.ResetAnim();
        _blinkEnabled = false;
    }

    private void SetEyeMats(Material lMat, Material rMat) {
        _skin.lEyeRend.material = lMat;
        _skin.rEyeRend.material = rMat;

        //Material[] mats = _skin.renderer.materials;
        //mats[0] = lMat;
        //mats[1] = rMat;
        //_skin.renderer.materials = mats;
    }
}
