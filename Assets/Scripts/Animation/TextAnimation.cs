using System;
using System.Collections;
using TMPro;
using UnityEngine;

public static class TextAnimation
{
    private const string WAIT_STRING = "WAIT:";

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
            if(line.Contains(WAIT_STRING))
            {
                var waitString = line.Replace(WAIT_STRING, "");
                yield return new WaitForSeconds(float.Parse(waitString));
                continue;
            }
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
