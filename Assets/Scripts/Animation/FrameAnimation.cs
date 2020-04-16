
using UnityEngine;

public abstract class FrameAnimation {

    public int CurrFrame { get { return _frameNum; } }

    private float _frameLength;
    private float _loopDelay;
    protected int _numFrames;

    private float _frameTimer;
    private float _loopTimer;
    protected int _frameNum;

    protected FrameAnimation(float frameLength, int numFrames, float loopDelay) {
        _frameLength = frameLength;
        _loopDelay = loopDelay;
        _numFrames = numFrames;

        _frameTimer = _frameLength;
    }

    protected abstract void UpdateFrame();

    public void ResetAnim() {
        _frameNum = 0;
        _frameTimer = _frameLength;
        _loopTimer = 0;

        UpdateFrame();
    }

    public void UpdateAnim() {
        if(_loopTimer > 0) {
            _loopTimer -= Time.deltaTime;
            return;
        }

        _frameTimer -= Time.deltaTime;

        if(_frameTimer < 0) {
            _frameNum++;
            _frameTimer = _frameLength;

            if(_frameNum >= _numFrames) {
                _frameNum = 0;
                _loopTimer = _loopDelay;
            }

            UpdateFrame();
        }
    }
}
