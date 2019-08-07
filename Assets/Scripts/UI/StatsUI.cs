using UnityEngine;
using System.Collections;
using TMPro;

namespace UI { 
    public class StatsUI : MonoBehaviour {

        public StatsController stats;

        public TMP_Text staminaField;
        public TMP_Text healthField;
        public TMP_Text happinessField;
        public TMP_Text hungerField;

        private void Update() {
            staminaField.text = stats.stamina.ToString("N0");
            healthField.text = stats.health.ToString("N0");
            happinessField.text = stats.happiness.ToString("N0");
            hungerField.text = stats.hunger.ToString("N0");
        }
    }
}
