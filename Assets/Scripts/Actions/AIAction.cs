
public abstract class AIAction {

    protected AIAction(Skin skin) {
        this.skin = skin;
    }

    protected Skin skin;

    public abstract void StartAction();
    public abstract void UpdateAction();
    public abstract bool IsFinished();
    public abstract void Interrupt();
}
