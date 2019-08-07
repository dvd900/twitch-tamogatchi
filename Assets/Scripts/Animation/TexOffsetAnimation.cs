
using UnityEngine;

public class TexOffsetAnimation : FrameAnimation {

    private Renderer _rend;
    private int _matIndex;
    private string _texName;
    private float _offset;

    public TexOffsetAnimation(Renderer rend, int matIndex, string texName, float frameLength, 
        int numFrames, float loopDelay) : base(frameLength, numFrames, loopDelay) {

        _rend = rend;
        _matIndex = matIndex;
        _texName = texName;

        _offset = 1.0f / numFrames;
    }

    protected override void UpdateFrame() {
        Vector2 offset = new Vector2(_offset * _frameNum, 0);
        _rend.materials[_matIndex].SetTextureOffset(_texName, offset);
    }
}
