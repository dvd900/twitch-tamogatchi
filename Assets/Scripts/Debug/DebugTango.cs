
using AIActions;
using System.Collections;
using UnityEngine;

public class DebugTango : MonoBehaviour
{
    [SerializeField] private float _timeToGreetUser;

    private void Start()
    {
        StartCoroutine(GreetRoutine());
    }

    IEnumerator GreetRoutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(_timeToGreetUser);
            if(Skin.CurrentTango != null)
            {
                Skin.CurrentTango.actionController.DoAction(new GreetAction(Skin.CurrentTango, GetNewUsername()));
            }
        }

    }

    private string GetNewUsername()
    {
        string[] names = { "peetle", "bigWiz", "david", "hyurk", "bloodenthusiast", "ysciv", "newguy" };
        return names[Random.Range(0, names.Length)];
    }
}
