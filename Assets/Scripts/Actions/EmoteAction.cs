
using System;
using System.Linq;
using UnityEngine;

public class EmoteAction : AIAction
{
    private static int LAST_EMOTE;

    private EmoteType _type;
    private bool _hasDoneEmote;

    public EmoteAction(Skin skin) : base(skin)
    {
        LAST_EMOTE = (LAST_EMOTE + 1) % ((int)Enum.GetValues(typeof(EmoteType)).Cast<EmoteType>().Max() + 1);
        int emoteInd = LAST_EMOTE;
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

    public override void Interrupt()
    {
        _skin.emoteController.OnEmoteEnd();
    }

    public override bool IsFinished()
    {
        return !_skin.movementController.IsTurning && !_skin.emoteController.IsDoingEmote;
    }

    public override float Score(AIWorldData data)
    {
        return .2f;
    }

    public override void StartAction()
    {
        _skin.movementController.StopWalking();
        _skin.movementController.FaceCamera();
    }

    public override void UpdateAction()
    {
        if(!_hasDoneEmote && !_skin.movementController.IsTurning)
        {
            switch (_type)
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
            _hasDoneEmote = true;
        }
    }
}

