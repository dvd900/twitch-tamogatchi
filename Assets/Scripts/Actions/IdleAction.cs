
using UnityEngine;

public class IdleAction : AIAction
{
    private const float CHANCE_TO_SPEAK = .3f;

    public float waitTime { get { return _waitTime; } set { _waitTime = value; } }
    private float _waitTime;
    private float _timer;
    private bool _sayDialogue;

    /// <summary>
    /// Chance to say dialogue. if not, wait ~avgIdleTime, otherwise say dialogue then wait ~avgIdleTime / 2
    /// </summary>
    /// <param name="skin"></param>
    public IdleAction(Skin skin) : base(skin) {
        _waitTime = _skin.actionController.avgIdleTime
            + _skin.actionController.avgIdleTime * (Random.value - .5f);
        _sayDialogue = Random.value < CHANCE_TO_SPEAK;

        if(_sayDialogue)
        {
            _waitTime /= 2.0f;
        }
    }

    public IdleAction(Skin skin, float waitTime, bool sayDialogue) : base(skin)
    {
        _waitTime = waitTime;
        _sayDialogue = sayDialogue;
    }

    public override AIAction Generate() {
        return new IdleAction(_skin);
    }

    public override void Interrupt() { }

    public override bool IsFinished() {
        return _timer <= 0;
    }

    public override float Score(AIWorldData data) {
        return .3f;
    }

    public override void StartAction() {
        _timer = _waitTime;
        _skin.movementController.StopWalking();
        if(_sayDialogue)
        {
            _skin.movementController.FaceCamera();
        }
    }

    public override void UpdateAction() {
        if(!_sayDialogue)
        {
            _timer -= Time.deltaTime;
        }
        else if(_sayDialogue && !_skin.movementController.IsTurning)
        {
            _skin.speechController.SayRandomDialogue();
            _sayDialogue = false;
        }
    }
}

