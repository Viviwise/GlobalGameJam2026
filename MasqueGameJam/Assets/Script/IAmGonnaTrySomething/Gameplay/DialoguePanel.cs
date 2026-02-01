using System;
using TMPro;
using UnityEngine;

namespace Script.IAmGonnaTrySomething.Gameplay
{
    public class DialoguePanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textArea, nameArea;
        
        public int SetUp(string dialogue, string speaker)
        {
            nameArea.text = speaker;
            int maxChars = 24;
            if (dialogue.Length > maxChars)
            {
                textArea.text = dialogue[Range.EndAt(maxChars -1)];
                return maxChars+1;
            }
            else
            {
                textArea.text = dialogue;
                return dialogue.Length;
            }
            textArea.text = dialogue;
            
        }
    }
}