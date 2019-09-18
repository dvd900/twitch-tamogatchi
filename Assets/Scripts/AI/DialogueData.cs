using System;
using UnityEngine;

namespace GameData {
    [CreateAssetMenu(fileName = "DialogueData", menuName = "ScriptableObjects/DialogueData", order = 1)]
    public class DialogueData : ScriptableObject {
        public string[] IdleDialogues;
    }
}
