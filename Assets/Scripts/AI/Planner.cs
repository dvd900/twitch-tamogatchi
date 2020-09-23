using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Planner : MonoBehaviour {

    public static Planner Instance;

    [SerializeField] private Skin _pet;
    [SerializeField] private float _planDelay;

    private List<AIAction> _actions;

    public AIWorldData WorldData { get { return _worldData; } }
    private AIWorldData _worldData;

    private float _planTimer;
    private AIAction _lastAction;

    // DEBUG
    private List<string> _DEBUG_LastScores;

    private void Awake()
    {
        Instance = this;
    }

    void Start() {
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
        _actions = new List<AIAction>();
        _DEBUG_LastScores = new List<string>();

        _actions.Add(new PickupAction(_pet));
        _actions.Add(new WalkToAction(_pet));
        _actions.Add(new EatAction(_pet));
        _actions.Add(new IdleAction(_pet));
        _actions.Add(new EmoteAction(_pet));

        _worldData = new AIWorldData(_pet);

        _lastAction = _actions[0];
    }

    private void Plan() {
        if (_pet.actionController.currentAction != null) {
            return;
        }

        _worldData.UpdateData();

        _DEBUG_LastScores.Clear();

        float maxScore = 0;
        AIAction bestAction = null;
        foreach(AIAction action in _actions) {
            float score = action.Score(_worldData);

            string scoreString = action.ToString() + ": " + score;

            if(action.GetType() == _lastAction.GetType()) {
                score *= .5f;

                scoreString += " * .5f";
            }

            float randomAdjustment = _pet.actionController.actionRandomness * (UnityEngine.Random.value - .5f);
            score += randomAdjustment;

            scoreString += " + " + randomAdjustment.ToString("F1");
            scoreString = score.ToString("F1") + ": " + scoreString;

            if (score > maxScore) {
                maxScore = score;
                bestAction = action;
            }

            _DEBUG_LastScores.Add(scoreString);
        }

        AIAction newAction = bestAction.Generate();
        _pet.actionController.DoAction(newAction);
        _lastAction = newAction;
    }
}
