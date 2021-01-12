using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SweetokenGenerator : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public float accrueRate = 1;
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
        if(tokensCount < 100)
        {
            tokensCount += Time.deltaTime * accrueRate;
        }
        timerText.text = tokensCount.ToString("F0");
    }

}
