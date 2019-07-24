using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Planner : MonoBehaviour {

    [SerializeField] private Skin _pet;
    [SerializeField] private float _planDelay;

    private List<AIAction> _actions;

    public AIWorldData worldData { get { return _worldData; } }
    private AIWorldData _worldData;

    private float _planTimer;
    private AIAction _lastAction;

    void Start() {
        _actions = new List<AIAction>();

        _actions.Add(new PickupAction(_pet));
        _actions.Add(new WalkToAction(_pet));
        _actions.Add(new EatAction(_pet));
        _actions.Add(new IdleAction(_pet));

        _worldData = new AIWorldData(_pet);
    }

    void Update() {
        _planTimer -= Time.deltaTime;
        if(_planTimer < 0) {
            _planTimer = _planDelay;
            Plan();
        }
    }

    private void Plan() {
        if (_pet.actionController.currentAction != null) {
            return;
        }

        _worldData.UpdateData();

        float maxScore = 0;
        AIAction bestAction = null;
        foreach(AIAction action in _actions) {
            float score = action.Score(_worldData);
            if(score > maxScore) {
                maxScore = score;
                bestAction = action;
            }
        }

        AIAction newAction = bestAction.Generate();
        _pet.actionController.DoAction(newAction);
        _lastAction = newAction;
    }
}
