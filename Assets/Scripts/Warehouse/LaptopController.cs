using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LaptopController : MonoBehaviour
{
    private const float S_CHAR_TIME = .0005f;
    private const float S_LINE_TIME = .05f;

    [SerializeField] private RectTransform _terminal;
    [SerializeField] private RectTransform _game;

    [SerializeField] private TextMeshProUGUI _terminalText;

    [TextArea]
    [SerializeField] private string _spawnText;

    public void EnterGame()
    {
        StartCoroutine(EnterGameRoutine());
    }

    private IEnumerator EnterGameRoutine()
    {
        yield return TextAnimation.PrintTextRoutine(_spawnText, _terminalText, S_CHAR_TIME, S_LINE_TIME);


        yield return new WaitForSeconds(2.0f);
        LevelRefs.Instance.Spawner.Spawn();

        LeanTween.scale(_terminal, Vector3.zero, 1.5f).setEaseInOutElastic();
        LeanTween.move(_game, Vector3.zero, 2.0f);
        yield return new WaitForSeconds(2.2f);
        LeanTween.scale(_game, 1.27f * Vector3.one, 1.2f).setEaseInOutElastic();

        yield return new WaitForSeconds(1.2f);
    }
}
