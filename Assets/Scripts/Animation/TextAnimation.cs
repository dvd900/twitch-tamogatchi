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

    public static void PrintText(string text, TextMeshProUGUI target, float charTime, float lineTime)
    {
        new CoroutineTask(PrintTextRoutine(text, target, charTime, lineTime));
    }

    public static IEnumerator PrintTextRoutine(string text, TextMeshProUGUI target, float charTime, float lineTime)
    {
        target.text = "";
        var lines = text.Split('\n');
        foreach(var line in lines)
        {
            int numChars = (int)(Time.deltaTime / charTime);
            int numFrameChars = 0;
            for(int i = 0; i < line.Length; i++)
            {
                target.text = target.text + line[i];
                if (++numFrameChars > numChars)
                {
                    numChars = (int)(Time.deltaTime / charTime);
                    numFrameChars = 0;
                    yield return new WaitForSeconds(charTime);
                }
            }

            target.text = target.text + '\n';
            yield return new WaitForSeconds(lineTime);
        }
    }
}
