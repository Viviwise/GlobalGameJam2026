using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
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
    public bool dialogueFinished = false;
    
    [System.Serializable]
    public class CustomDialogue
    {
        [TextArea(2, 5)]
        public string[] lines;
    }

    
    public static bool IsDialogueFinished
    {
        get
        {
            return instance != null && instance.dialogueFinished;
        }
    }
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
            }
        }
    }

    public static void StartDialogue(DialogueType type, float textSpeed = 0.05f)
    {
        if (!instance.gameObject.activeInHierarchy)
            instance.gameObject.SetActive(true);

        instance.dialogueFinished = false;

        instance.currentDialogue = instance.dialogueLists[(int)type].Sentences;
        instance.currentIndex = 0;
        instance.currentTextSpeed = textSpeed;

        if (instance.currentDialogue.Length > 0)
        {
            if (instance.typingCoroutine != null)
                instance.StopCoroutine(instance.typingCoroutine);

            instance.typingCoroutine = instance.StartCoroutine(
                instance.TypeLine(instance.currentDialogue[0])
            );
        }
    }
    
    public static void StartCustomDialogue(string[] lines, float textSpeed = 0.05f)
    {
        if (instance == null) return;

        if (!instance.gameObject.activeInHierarchy)
            instance.gameObject.SetActive(true);

        instance.dialogueFinished = false;
        instance.currentDialogue = lines;
        instance.currentIndex = 0;
        instance.currentTextSpeed = textSpeed;

        if (lines.Length > 0)
        {
            if (instance.typingCoroutine != null)
                instance.StopCoroutine(instance.typingCoroutine);

            instance.typingCoroutine = instance.StartCoroutine(
                instance.TypeLine(lines[0])
            );
        }
    }

    public void StartHappyDialogue()
    {
        StartDialogue(DialogueType.HAPPY);
    }

    public void StartAngryDialogue()
    {
        StartDialogue(DialogueType.ANGRY);
    }
    
    public void StartSadDialogue()
    {
        StartDialogue(DialogueType.SAD);
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
