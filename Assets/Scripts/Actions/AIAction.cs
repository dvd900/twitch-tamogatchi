
using UnityEngine;

public abstract class AIAction {

    protected AIAction(Skin skin) {
        _skin = skin;
    }

    protected Skin _skin;
    public GameObject _debugMarker;

    public void FinishAction() {
        if(_debugMarker != null) {
            GameObject.Destroy(_debugMarker);
        }
    }

    public abstract void StartAction();
    public abstract void UpdateAction();
    public abstract bool IsFinished();
    public abstract void Interrupt();

    public abstract float Score(AIWorldData data);
    public abstract AIAction Generate();
}
