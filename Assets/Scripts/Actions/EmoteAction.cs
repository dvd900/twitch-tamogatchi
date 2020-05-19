
using System;
using System.Linq;
using UnityEngine;

public class EmoteAction : AIAction
{
    private static int LAST_EMOTE;

    private EmoteType _type;

    public EmoteAction(Skin skin) : base(skin)
    {
        LAST_EMOTE = (LAST_EMOTE + 1) % ((int)Enum.GetValues(typeof(EmoteType)).Cast<EmoteType>().Max() + 1);
        int emoteInd = LAST_EMOTE;
        Debug.Log("Maxemote: " + (int)Enum.GetValues(typeof(EmoteType)).Cast<EmoteType>().Max());
        _type = (EmoteType)emoteInd;
        Debug.Log("Made action with emote: " + emoteInd);
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
        Debug.Log("startign emote: " + _type);
        switch(_type)
        {
            case EmoteType.Cheer:
                _skin.emoteController.Cheer();
                break;
            case EmoteType.Wave:
                _skin.emoteController.Wave();
                break;
            case EmoteType.Dance:
                _skin.emoteController.Dance();
                break;
        }
    }

    public override void UpdateAction()
    {
    }
}

