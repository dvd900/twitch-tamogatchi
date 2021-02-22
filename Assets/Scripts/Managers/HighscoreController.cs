using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highscore
{
    public float TimeAlive;
    public int NumApplesEaten;
    public float DamageTaken;
    public int TangoId;
}

public class HighscoreController
{
    public static HighscoreController Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new HighscoreController();
            }

            return _instance;
        }
    }

    private static HighscoreController _instance;

    public List<Highscore> Scores { get { return _scores; } }
    private List<Highscore> _scores;

    public Highscore LastScore { get { return _scores[_scores.Count - 1]; } }

    private Highscore _currentScore;
    private float _birthTime;

    private HighscoreController()
    {
        _scores = new List<Highscore>();

        var dummyScore = new Highscore();
        dummyScore.TangoId = 1234;
        dummyScore.TimeAlive = -3000.0f;
        dummyScore.NumApplesEaten = -100;
        dummyScore.DamageTaken = -1000.0f;

        _scores.Add(dummyScore);
    }

    public void OnTangoSpawn()
    {
        _currentScore = new Highscore();
        _currentScore.TangoId = Random.Range(1000, 9999);
        _birthTime = Time.time;
    }

    public void OnAppleEaten()
    {
        _currentScore.NumApplesEaten++;
    }

    public void OnDamageTaken(float damage)
    {
        _currentScore.DamageTaken += damage;
    }

    public void OnTangoDie()
    {
        _currentScore.TimeAlive = Time.time - _birthTime;
        _scores.Add(_currentScore);
    }
}
