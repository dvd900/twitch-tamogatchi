using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

namespace UI { 
    public class StatsUI : MonoBehaviour {

        public static StatsUI Instance;

        [SerializeField] private StatsController _stats;

        [SerializeField] private RectTransform _heartIcon;
        [SerializeField] private Image _energyIcon;
        [SerializeField] private Image _hungerIcon;

        private float _lastHealth;

        private void Awake()
        {
            Instance = this;
        }

        public void SetStatsController(StatsController stats)
        {
            _stats = stats;
        }

        private void Update() {
            if(_lastHealth != _stats.health)
            {
                //LeanTween.scale(avatarScale, new Vector3(1.7f, 1.7f, 1.7f), 5f).setEase(LeanTweenType.easeOutBounce);
                UpdateHealth(_stats.health / 100.0f);
                _lastHealth = _stats.health;
            }
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
