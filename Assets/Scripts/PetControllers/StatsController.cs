using UnityEngine;
using System.Collections;
using AIActions;

/// <summary>
/// Stats go from 0 to 100
/// </summary>
public class StatsController : MonoBehaviour {

    /// <summary>
    /// Rate of stamina drain
    /// </summary>
    [SerializeField] private float _staminaDrain;
    /// <summary>
    /// Rate of hunger drain
    /// </summary>
    [SerializeField] private float _hungerDrain;
    /// <summary>
    /// Rate of health decrease when hungry
    /// </summary>
    [SerializeField] private float _hungryHealthDrain;

    public bool IsHungry { get { return _hunger <= 0; } }
    public bool IsTired { get { return _stamina <= 0; } }

    public float Stamina { get { return _stamina; } }
    private float _stamina;
    public float Health { get { return _health; } }
    private float _health;
    public float Hunger { get { return _hunger; } }
    private float _hunger;
    public float Happiness { get { return _happiness; } }
    private float _happiness;

    private Skin _skin;

    private void Start() {
        _stamina = 100;
        _health = 100;
        _happiness = 100;
        _hunger = 100;
        _skin = GetComponent<Skin>();
    }

    public void AddStamina(float x) {
        AddStat(x, ref _stamina);
    }

    public void AddHealth(float x) {
        AddStat(x, ref _health);
        if(_health <= 0 && !_skin.actionController.IsDying)
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
        AddHunger(-_hungerDrain * Time.deltaTime);

        if(IsHungry) {
            AddHealth(-_hungryHealthDrain * Time.deltaTime);
        }
    }
}
