using System;
using System.Collections;
using TMPro;
using UnityEngine;

public static class TextAnimation
{
    public static void PrintText(string text, TextMeshPro target, float charTime)
    {
        new CoroutineTask(PrintTextRoutine(text, target, charTime));
    }

    private static IEnumerator PrintTextRoutine(string text, TextMeshPro target, float charTime)
    {
        target.text = "";
        for(int i = 0; i < text.Length; i++)
        {
            char nextChar = text[i];
            target.text = target.text + nextChar;
            if (nextChar == '\n')
            {
                yield return new WaitForSeconds(10 * charTime);
            }
            else
            {
                yield return new WaitForSeconds(charTime);
            }
        }
    }
}
