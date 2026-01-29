using System.Collections;
using UnityEngine;
using TMPro;

namespace Script.Choice.BonPasPropreMaisDemoTKT
{
    public class DialogueManager : MonoBehaviour
    {
        public TMP_Text dialogueText;   // Le texte affiché
        public GameObject panel;        // Le panel complet

        private void Awake()
        {
            panel.SetActive(false);      // caché par défaut
        }

        public void ShowDialogue(string character, string text, float duration = 2f)
        {
            StopAllCoroutines();
            panel.SetActive(true);
            StartCoroutine(DisplayDialogue(character, text, duration));
        }

        private IEnumerator DisplayDialogue(string character, string text, float duration)
        {
            dialogueText.text = $"{character}: {text}";
            yield return new WaitForSeconds(duration);
            panel.SetActive(false);
        }
    }

}