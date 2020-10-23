using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using AIActions;

public class Planner : MonoBehaviour {

    public static Planner Instance;

    [SerializeField] private Skin _pet;
    [SerializeField] private float _planDelay;

    private List<GeneratedAction> _actions;

    private float _planTimer;
    private GeneratedAction _lastAction;

    // DEBUG
    private List<string> _DEBUG_LastScores;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if(_pet != null)
        {
            SetPet(_pet);
        }
    }

    void Update() {
        if(_pet == null)
        {
            return;
        }

        _planTimer -= Time.deltaTime;
        if(_planTimer < 0) {
            _planTimer = _planDelay;
            Plan();
        }
    }

    public void SetPet(Skin pet)
    {
        _pet = pet;
        _actions = new List<GeneratedAction>();
        _DEBUG_LastScores = new List<string>();

        _actions.Add(new PickupAction(_pet));
        _actions.Add(new WalkToAction(_pet));
        _actions.Add(new EatAction(_pet));
        _actions.Add(new IdleAction(_pet));
        _actions.Add(new EmoteAction(_pet));

        _lastAction = _actions[0];
    }

    private void Plan() {
        if (_pet.actionController.CurrentAction != null) {
            return;
        }

        _DEBUG_LastScores.Clear();

        float maxScore = 0;
        GeneratedAction bestAction = null;
        foreach(GeneratedAction action in _actions) {
            float score = action.Score(_pet);

            string scoreString = action.ToString() + ": " + score;

            if(action.GetType() == _lastAction.GetType()) {
                score *= .5f;

                scoreString += " * .5f";
            }

            float randomAdjustment = _pet.stats.ActionRandomness * (UnityEngine.Random.value - .5f);
            score += randomAdjustment;

            scoreString += " + " + randomAdjustment.ToString("F1");
            scoreString = score.ToString("F1") + ": " + scoreString;

            if (score > maxScore) {
                maxScore = score;
                bestAction = action;
            }

            _DEBUG_LastScores.Add(scoreString);
        }

        GeneratedAction newAction = bestAction.Generate(_pet);
        _pet.actionController.DoAction((AIAction) newAction);
        _lastAction = newAction;
    }
}
