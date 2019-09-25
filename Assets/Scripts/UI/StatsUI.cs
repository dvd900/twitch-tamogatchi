using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

namespace UI { 
    public class StatsUI : MonoBehaviour {

        [SerializeField] private StatsController _stats;

        [SerializeField] private RectTransform _heartIcon;
        [SerializeField] private Image _energyIcon;
        [SerializeField] private Image _hungerIcon;

        private void Update() {
            UpdateHealth(_stats.health / 100.0f);
            UpdateEnergy(_stats.stamina / 100.0f);
            UpdateHunger(_stats.hunger / 100.0f);
        }

        private void UpdateHealth(float p) {
            _heartIcon.localScale = new Vector3(p, p, p);
        }

        private void UpdateEnergy(float p) {
            _energyIcon.fillAmount = p;
        }

        private void UpdateHunger(float p) {
            _hungerIcon.fillAmount = p;
        }
    }
}
