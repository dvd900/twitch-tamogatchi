
using UnityEngine;

public class IdleAction : AIAction {

    public float waitTime { get { return _waitTime; } set { _waitTime = value; } }
    private float _waitTime;
    private float _timer;
    private bool _sayDialogue;

    public IdleAction(Skin skin) : base(skin) {
        _waitTime = _skin.actionController.avgIdleTime
            + _skin.actionController.avgIdleTime * (Random.value - .5f);
        _sayDialogue = true;
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

        if(_sayDialogue)
        {
            _skin.speechController.SayRandomDialogue();
        }
    }

    public override void UpdateAction() {
        _timer -= Time.deltaTime;
    }
}

