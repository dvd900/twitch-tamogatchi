
using UnityEngine;

public class TexOffsetAnimation : FrameAnimation {

    private Renderer _rend;
    private int _matIndex;
    private string _texName;
    private float _offsetX;
    private float _offsetY;
    private int _framesX;
    private int _framesY;

    public TexOffsetAnimation(Renderer rend, int matIndex, string texName, float frameLength, 
        int framesX, int framesY, float loopDelay) : base(frameLength, framesX * framesY, loopDelay) {

        _rend = rend;
        _matIndex = matIndex;
        _texName = texName;
        _framesX = framesX;
        _framesY = framesY;

        _offsetX = 1.0f / framesX;
        _offsetY = 1.0f / framesY;
    }

    protected override void UpdateFrame() {
        int x = _frameNum % _framesX;
        int y = _frameNum / _framesX;
        Vector2 offset = new Vector2(_offsetX * x, 1.0f - _offsetY * (y + 1));
        _rend.materials[_matIndex].SetTextureOffset(_texName, offset);
    }
}
