
using System;
using System.Linq;
using UnityEngine;

public class EmoteAction : AIAction
{
    private EmoteType _type;

    public EmoteAction(Skin skin) : base(skin)
    {
        int emoteInd = UnityEngine.Random.Range(1, (int)Enum.GetValues(typeof(EmoteType)).Cast<EmoteType>().Max());
        _type = (EmoteType)emoteInd;
    }

    public EmoteAction(Skin skin, EmoteType type) : base(skin)
    {
        _type = type;
    }

    public override AIAction Generate()
    {
        return new EmoteAction(_skin);
    }

    public override void Interrupt() { }

    public override bool IsFinished()
    {
        return !_skin.emoteController.IsDoingEmote;
    }

    public override float Score(AIWorldData data)
    {
        return .3f;
    }

    public override void StartAction()
    {
        switch(_type)
        {
            case EmoteType.Cheer:
                _skin.emoteController.Cheer();
                break;
            case EmoteType.Wave:
                _skin.emoteController.Wave();
                break;
        }
    }

    public override void UpdateAction()
    {
    }
}

