using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Script.IAmGonnaTrySomething.Gameplay
{
    public class DialoguePanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textArea, nameArea;
        
        public void SetUp(string dialogue, string speaker, float speed)
        {
            nameArea.text = speaker;
            StartCoroutine(DisplayTextDynamic(dialogue, speed));
            
        }

        IEnumerator DisplayTextDynamic(string text, float letterSpeed)
        {
            string currentText = "";
            for (int i = 0; i < text.Length; i++)
            {
                currentText += text[i];
                textArea.text = currentText;
                yield return new WaitForSeconds(letterSpeed);
            }
        }
    }
}