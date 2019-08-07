using System;
using UnityEngine;

public class Consumable : Item {

    [SerializeField] private GameObject[] _consumptionStates;

    private int _biteInd;

    public void DoEat() {
        Debug.Log("DOEAT");
        if (gameObject == null) {
            Debug.LogError("GO null");
            Debug.LogError("current action: " + _holder.actionController.currentAction);
        }

        if (++_biteInd >= _consumptionStates.Length) {
            Destroy(gameObject);
            return;
        }

        _consumptionStates[_biteInd - 1].SetActive(false);
        _consumptionStates[_biteInd].SetActive(true);
    }
}

