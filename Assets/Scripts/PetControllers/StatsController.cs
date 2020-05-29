using UnityEngine;
using System.Collections;

/// <summary>
/// Stats go from 0 to 100
/// </summary>
public class StatsController : MonoBehaviour {

    /// <summary>
    /// Rate of stamina drain
    /// </summary>
    [SerializeField] private float _staminaDrain;
    /// <summary>
    /// Rate of hunger increase
    /// </summary>
    [SerializeField] private float _hungerInc;

    /// <summary>
    /// Rate of happiness drain when at low stamina
    /// </summary>
    [SerializeField] private float _staminaHappinessDrain;
    /// <summary>
    /// Rate of happiness drain when hungry
    /// </summary>
    [SerializeField] private float _hungerHappinessDrain;
    /// <summary>
    /// Rate of happiness drain when hp is low
    /// </summary>
    [SerializeField] private float _healthHappinessDrain;

    /// <summary>
    /// The point above which sweetango becomes rly hungie
    /// </summary>
    [SerializeField] private float _hungerThreshold;
    /// <summary>
    /// The point below which sweetango becomes tired
    /// </summary>
    [SerializeField] private float _staminaThreshold;
    /// <summary>
    /// The point below which sweetango becomes sick
    /// </summary>
    [SerializeField] private float _healthThreshold;

    public float stamina { get { return _stamina; } }
    private float _stamina;
    public float health { get { return _health; } }
    private float _health;
    public float hunger { get { return _hunger; } }
    private float _hunger;
    public float happiness { get { return _happiness; } }
    private float _happiness;

    private Skin _skin;

    private void Start() {
        _stamina = 100;
        _health = 100;
        _happiness = 100;
        _hunger = 0;
        _skin = GetComponent<Skin>();
    }

    public void AddStamina(float x) {
        AddStat(x, ref _stamina);
    }

    public void AddHealth(float x) {
        AddStat(x, ref _health);
        if(_health <= 0)
        {
            _skin.actionController.DoAction(new DeathAction(_skin));    
        }
    }

    public void AddHunger(float x) {
        AddStat(x, ref _hunger);
    }

    public void AddHappiness(float x) {
        AddStat(x, ref _happiness);
    }

    public bool IsHungry() {
        return _hunger > _hungerThreshold;
    }

    public bool IsTired() {
        return _stamina < _staminaThreshold;
    }

    public bool IsHPLow() {
        return _health < _healthThreshold;
    }

    private void AddStat(float x, ref float stat) {
        stat += x;
        if(stat < 0) {
            stat = 0;
        } else if(stat > 100) {
            stat = 100;
        }
    }

    private void Update() {
        AddStamina(-_staminaDrain * Time.deltaTime);
        AddHunger(_hungerInc * Time.deltaTime);

        float happinessMod = 0;

        if(IsTired()) {
            happinessMod += _staminaHappinessDrain;
        }

        if(IsHungry()) {
            happinessMod += _hungerHappinessDrain;
        }

        if(IsHPLow()) {
            happinessMod += _healthHappinessDrain;
        }

        AddHappiness(-happinessMod * Time.deltaTime);
    }
}
