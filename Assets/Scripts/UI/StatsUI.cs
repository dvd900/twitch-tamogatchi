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
        private float _lastStamina;
        private float _lastHunger;

        private void Awake()
        {
            Instance = this;
        }

        public void SetStatsController(StatsController stats)
        {
            _stats = stats;
        }

        private void Update() {
            if(_lastHealth != _stats.Health)
            {
                UpdateHealth(_stats.Health / 100.0f);
                _lastHealth = _stats.Health;
            }
            if(_lastStamina != _stats.Stamina)
            {
                UpdateImageStat(_energyIcon, _stats.Stamina, ref _lastStamina);
            }
            if(_lastHunger != _stats.Hunger)
            {
                UpdateImageStat(_hungerIcon, _stats.Hunger, ref _lastHunger);
            }
            //UpdateEnergy(_stats.Stamina / 100.0f);
            //UpdateHunger(_stats.Hunger / 100.0f);
        }

        private void UpdateHealth(float p) {

            LeanTween.scale(_heartIcon.gameObject, new Vector3(p, p, p), .5f).setEaseInOutElastic();
        }

        private void UpdateImageStat(Image image, float newValue, ref float oldValue)
        {
            image.fillAmount = newValue / 100.0f;

            // if its just a small change, just set it and return 
            if (Mathf.Abs(newValue - oldValue) > 2)
            {
                Vector3 punchTarget = (newValue > oldValue) ? 1.2f * Vector3.one : .8f * Vector3.one;
                LeanTween.scale(image.gameObject, punchTarget, .5f).setEasePunch();
            }

            oldValue = newValue;

            //LeanTween.value(image.gameObject, (float val) => { image.fillAmount = val / 100.0f; }, oldValue, newValue, .5f).setEaseInOutQuad();


        }

        private void UpdateEnergy(float p) {
            _energyIcon.fillAmount = p;
        }

        private void UpdateHunger(float p) {
            _hungerIcon.fillAmount = p;
        }
    }
}
