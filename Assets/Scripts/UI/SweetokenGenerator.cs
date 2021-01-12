using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SweetokenGenerator : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public float tokensCount;
    private int minuteCount;
    private int hourCount;
    void Update()
    {
        UpdateTimerUI();
    }
    //call this on update
    public void UpdateTimerUI()
    {
        //set timer UI

        //timerText.text = hourCount + "h:" + minuteCount + "m:" + (int)tokensCount + "s";
        //if (tokensCount >= 60)
        //{
        //    minuteCount++;
        //    tokensCount = 0;
        //}
        //else if (minuteCount >= 60)
        //{
        //    hourCount++;
        //    minuteCount = 0;
        //}
        if(tokensCount < 100)
        {
            tokensCount += Time.deltaTime;
        }
        timerText.text = tokensCount.ToString("F0");
    }

}
