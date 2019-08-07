using System;
using UnityEngine;

public class Consumable : Item {


    [SerializeField] private float _healthMod;
    [SerializeField] private float _staminaMod;
    [SerializeField] private float _hungerMod;
    [SerializeField] private float _happinessMod;

    [SerializeField] private GameObject[] _consumptionStates;

    private int _biteInd;

    public void DoEat() {
        if (gameObject == null) {
            Debug.LogError("GO null");
            Debug.LogError("current action: " + _holder.actionController.currentAction);
        }

        _holder.statsController.AddHealth(_healthMod);
        _holder.statsController.AddStamina(_staminaMod);
        _holder.statsController.AddHunger(_hungerMod);
        _holder.statsController.AddHappiness(_happinessMod);

        if (++_biteInd >= _consumptionStates.Length) {
            Destroy(gameObject);
            return;
        }

        _consumptionStates[_biteInd - 1].SetActive(false);
        _consumptionStates[_biteInd].SetActive(true);
    }
}

