using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public enum DialogueType
{
    GREETING,
    ANGRY,
    HAPPY,
    SAD,
    BORED
}

[RequireComponent(typeof(TextMeshProUGUI))]
public class DialogueManager : MonoBehaviour
{
    [SerializeField] private DialogueList[] dialogueLists;
    private static DialogueManager instance;
    private TextMeshProUGUI textComponent;

    private string[] currentDialogue;
    private int currentIndex = 0;
    private Coroutine typingCoroutine;
    private float currentTextSpeed = 0.05f;
    private bool dialogueFinished = false; 
    
    private void Awake()
    {
        instance = this;
        textComponent = GetComponent<TextMeshProUGUI>();
        textComponent.text = string.Empty;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (currentDialogue == null) return;

        if (Keyboard.current.spaceKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
                textComponent.text = currentDialogue[currentIndex];
                typingCoroutine = null;
            }
            else if (!dialogueFinished)
            {
                NextLine();
            }
            else
            {
                textComponent.text = string.Empty;
                currentDialogue = null;
                dialogueFinished = false;
            }
        }
    }

    public static void StartDialogue(DialogueType type, float textSpeed = 0.05f)
    {
        if (!instance.gameObject.activeInHierarchy)
            instance.gameObject.SetActive(true);  // <- active le GameObject si nécessaire

        instance.currentDialogue = instance.dialogueLists[(int)type].Sentences;
        instance.currentIndex = 0;
        instance.currentTextSpeed = textSpeed;

        if (instance.currentDialogue.Length > 0)
        {
            if (instance.typingCoroutine != null)
                instance.StopCoroutine(instance.typingCoroutine);

            instance.typingCoroutine = instance.StartCoroutine(instance.TypeLine(instance.currentDialogue[0]));
        }
    }


    private void NextLine()
    {
        currentIndex++;

        if (currentIndex < currentDialogue.Length)
        {
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);

            typingCoroutine = StartCoroutine(TypeLine(currentDialogue[currentIndex]));
        }
        else
        {
            dialogueFinished = true;
        }
    }

    private IEnumerator TypeLine(string line)
    {
        textComponent.text = string.Empty;
        typingCoroutine = null;

        foreach (char c in line.ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(currentTextSpeed);
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        string[] names = Enum.GetNames(typeof(DialogueType));
        Array.Resize(ref dialogueLists, names.Length);
        for (int i = 0; i < dialogueLists.Length; i++)
            dialogueLists[i].name = names[i];
    }
#endif
}

[Serializable]
public struct DialogueList
{
    public string[] Sentences => sentences;
    [SerializeField] public string name;
    [SerializeField] private string[] sentences;
}
