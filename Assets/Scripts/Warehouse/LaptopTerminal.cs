using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LaptopTerminal : MonoBehaviour
{
    private const string S_WAIT_STRING = "WAIT:";
    private const string TANGO_ID_KEY = "TANGO_ID";
    private const string ALIVE_TIME_KEY = "ALIVE_TIME";
    private const string NUM_APPLES_KEY = "NUM_APPLES";
    private const string DMG_TAKEN_KEY = "DMG_TAKEN";

    private const float S_FLASH_USERNAME_TIME = 4.0f;
    private const float S_CHAR_TIME = .0005f;
    private const float S_LINE_TIME = .05f;

    private const float S_M_SPACE = .5f;

    private const float S_TANGO_ASCII_SIZE = .5f;

    [SerializeField] private string _username;
    [SerializeField] private TextMeshProUGUI _terminalText;
    [SerializeField] private ScrollRect _terminalScroll;

    [TextArea]
    [SerializeField] private string _tangoAsciiArt;

    [TextArea]
    [SerializeField] private string _spawnText;

    [TextArea]
    [SerializeField] private string _statsText1;
    [TextArea]
    [SerializeField] private string _statsText2;

    private string _terminalContent;
    private bool _isDoingCommand;

    private bool _isShowingUsername;
    private float _flashUsernameTimer;

    private void Start()
    {
        _terminalContent = "";
        _terminalText.text = "";
    }

    public void DoCommand(string cmd)
    {
        StartCoroutine(DoCommandRoutine(cmd));
    }

    public IEnumerator DoCommandRoutine(string cmd)
    {
        _isDoingCommand = true;

        cmd = ReplaceTerminalTags(cmd);

        string usernameText = (_terminalContent == "") ? "" : "\n";
        usernameText += _username;
        _terminalContent += usernameText;

        var lines = cmd.Split('\n');
        foreach (var line in lines)
        {
            if (line.Contains(S_WAIT_STRING))
            {
                var waitString = line.Replace(S_WAIT_STRING, "");
                yield return new WaitForSeconds(float.Parse(waitString));
                continue;
            }
            int numChars = (int)(Time.deltaTime / S_CHAR_TIME);
            int numFrameChars = 0;
            for (int i = 0; i < line.Length; i++)
            {
                _terminalContent = _terminalContent + line[i];
                if (++numFrameChars > numChars)
                {
                    numChars = (int)(Time.deltaTime / S_CHAR_TIME);
                    numFrameChars = 0;
                    yield return new WaitForSeconds(S_CHAR_TIME);
                }
            }

            _terminalContent = _terminalContent + '\n';
            yield return new WaitForSeconds(S_LINE_TIME);
        }

        _isDoingCommand = false;
    }

    public IEnumerator PrintStats()
    {
        string txt = "";
        txt += _statsText1;
        txt += "\n";

        txt += GetMSpaceEndTag() + GetMSpaceStartTag(S_M_SPACE * S_TANGO_ASCII_SIZE)
            + GetSizeStartTag(S_TANGO_ASCII_SIZE) + _tangoAsciiArt + GetSizeEndTag()
            + GetMSpaceEndTag() + GetMSpaceStartTag();

        txt += "\n";

        txt += _statsText2;

        return DoCommandRoutine(txt);
    }

    public IEnumerator PrintSpawn()
    {
        return DoCommandRoutine(_spawnText);
    }

    public IEnumerator ClearRoutine()
    {
        while(_terminalContent != "")
        {
            int i = _terminalContent.IndexOf("\n");
            if (i == -1)
            {
                _terminalContent = "";
            }
            else
            {
                _terminalContent = _terminalContent.Substring(i + 1);
            }
            yield return new WaitForSeconds(S_LINE_TIME);
        }
    }

    void Update()
    {
        if(!_isDoingCommand)
        {
            _flashUsernameTimer -= Time.deltaTime;
            if(_flashUsernameTimer < 0)
            {
                if(_isShowingUsername)
                {
                    _flashUsernameTimer = .01f * S_FLASH_USERNAME_TIME;
                }
                else
                {
                    _flashUsernameTimer = S_FLASH_USERNAME_TIME * (.3f * UnityEngine.Random.value + 1);
                }
                _isShowingUsername = !_isShowingUsername;
            }

            string usernamePrefix = (_terminalContent == "") ? "" : "\n";
            _terminalText.text = GetMSpaceStartTag() + _terminalContent + usernamePrefix + ((_isShowingUsername) ? _username: "") + GetMSpaceEndTag();
        }
        else
        {
            _terminalText.text = GetMSpaceStartTag() + _terminalContent + GetMSpaceEndTag();
        }

        _terminalScroll.verticalNormalizedPosition = 0;
    }

    private string ReplaceTerminalTags(string text)
    {
        if (HighscoreController.Instance.Scores.Count > 0)
        {
            var lastScore = HighscoreController.Instance.Scores[HighscoreController.Instance.Scores.Count - 1];
            text = text.Replace(TANGO_ID_KEY, lastScore.TangoId.ToString());
            text = text.Replace(ALIVE_TIME_KEY, lastScore.TimeAlive.ToString());
            text = text.Replace(NUM_APPLES_KEY, lastScore.NumApplesEaten.ToString());
            text = text.Replace(DMG_TAKEN_KEY, lastScore.DamageTaken.ToString());

        }

        return text;
    }

    private string GetMSpaceStartTag(float mspace = S_M_SPACE)
    {
        return "<mspace=" + mspace + "em>";
    }

    private string GetMSpaceEndTag()
    {
        return "</mspace>";
    }

    private string GetSizeStartTag(float sizePercent)
    {
        return "<size=" + ((int)(sizePercent * 100)) + "%>";
    }

    private string GetSizeEndTag()
    {
        return "</size>";
    }
}
