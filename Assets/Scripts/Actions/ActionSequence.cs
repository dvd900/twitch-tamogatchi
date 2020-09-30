using System;
public class ActionSequence : AIAction
{
    private AIAction[] _actions;

    private int _actionInd;

    public ActionSequence(params AIAction[] actions)
    {
        _actions = actions;
        _actionInd = -1;
    }

    public override void Interrupt()
    {
        if(_actionInd != -1)
        {
            _actions[_actionInd].Interrupt();
        }
    }

    public override bool IsFinished()
    {
        return _actionInd == _actions.Length - 1 && _actions[_actionInd].IsFinished();
    }

    public override void StartAction()
    {
        _actionInd = 0;
        _actions[_actionInd].StartAction();
    }

    public override void UpdateAction()
    {
        if(_actions[_actionInd].IsFinished())
        {
            if (_actionInd < _actions.Length - 1)
            {
                _actionInd++;
                _actions[_actionInd].StartAction();
            }
            return;
        }
        _actions[_actionInd].UpdateAction();
    }
}
