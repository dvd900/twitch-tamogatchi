﻿using System.Collections;
using System.Collections.Generic;
using AIActions;
using TMPro;
using UnityEngine;

public class LaptopController : MonoBehaviour
{
    private const float S_CHAR_TIME = .0005f;
    private const float S_LINE_TIME = .05f;

    private const string TANGO_ID_KEY = "TANGO_ID";
    private const string ALIVE_TIME_KEY = "ALIVE_TIME";
    private const string NUM_APPLES_KEY = "NUM_APPLES";
    private const string DMG_TAKEN_KEY = "DMG_TAKEN";

    [SerializeField] private RectTransform _terminal;
    [SerializeField] private RectTransform _game;

    [SerializeField] private TextMeshProUGUI _terminalText;

    [TextArea]
    [SerializeField] private string _spawnText;

    [TextArea]
    [SerializeField] private string _deathText;

    private int _tangoId;

    public void EnterGame()
    {
        StartCoroutine(EnterGameRoutine());
    }

    public void ExitGame()
    {
        StartCoroutine(ExitGameRoutine());
    }

    private IEnumerator EnterGameRoutine()
    {
        _tangoId = Random.Range(1000, 9999);
        yield return TextAnimation.PrintTextRoutine(GetTermText(_spawnText), _terminalText, S_CHAR_TIME, S_LINE_TIME);


        yield return new WaitForSeconds(2.0f);

        LevelRefs.Instance.Spawner.Spawn();

        LeanTween.scale(_terminal, Vector3.zero, 1.5f).setEaseInOutElastic();
        LeanTween.move(_game, Vector3.zero, 2.0f);
        yield return new WaitForSeconds(2.2f);
        LeanTween.scale(_game, 1.27f * Vector3.one, 1.2f).setEaseInOutElastic();

        yield return new WaitForSeconds(1.2f);

        yield return KillTangoRoutine();

        AppController.Instance.ChangeActiveScene(SceneName.Tango);
    }

    private IEnumerator ExitGameRoutine()
    {
        yield return new WaitForSeconds(2.0f);
        LeanTween.scale(_game, Vector3.zero, 1.5f).setEaseInOutElastic();

        yield return new WaitForSeconds(1.2f);

        _terminalText.text = "root@vb $";
        LeanTween.scale(_terminal, Vector3.one, 1.2f).setEaseInOutElastic();
        yield return new WaitForSeconds(2.0f);

        yield return TextAnimation.PrintTextRoutine(GetTermText(_deathText), _terminalText, S_CHAR_TIME, S_LINE_TIME);
    }

    private string GetTermText(string text)
    {
        text = text.Replace(TANGO_ID_KEY, _tangoId.ToString());

        if(HighscoreController.Instance.Scores.Count > 0)
        {
            var lastScore = HighscoreController.Instance.Scores[HighscoreController.Instance.Scores.Count - 1];
            text = text.Replace(ALIVE_TIME_KEY, lastScore.TimeAlive.ToString());
            text = text.Replace(NUM_APPLES_KEY, lastScore.NumApplesEaten.ToString());
            text = text.Replace(DMG_TAKEN_KEY, lastScore.DamageTaken.ToString());

        }

        return text;
    }

    public void KillTango()
    {
        StartCoroutine(KillTangoRoutine());
    }

    private IEnumerator KillTangoRoutine()
    {
        Skin.CurrentTango.actionController.DoAction(new SleepAction(Skin.CurrentTango, 40));

        int numBombs = 15;
        for (int i = 0; i < numBombs; i++)
        {
            Vector3 offset = 15 * Vector3.forward;
            Quaternion rot = Quaternion.AngleAxis(360 * (((float)i) / numBombs), Vector3.up);
            offset = rot * offset;
            ItemSpawner.Instance.SpawnBomb(Skin.CurrentTango.transform.position + offset, "pkill SWEETANGO");

            yield return null;
        }
    }
}
