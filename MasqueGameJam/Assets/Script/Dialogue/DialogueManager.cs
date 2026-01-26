using System;
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

[RequireComponent(typeof(TextMeshProUGUI)), ExecuteInEditMode]
public class DialogueManager : MonoBehaviour
{
    [SerializeField] private DialogueList[] dialogueLists;
    private static DialogueManager instance;
    private TextMeshProUGUI textComponent;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
        textComponent.text = string.Empty;
    }

    public static void PlayDialogue(DialogueType type, float textSpeed = 0.05f)
    {
        string[] lines = instance.dialogueLists[(int)type].Sentences;
        string randomLine = lines[UnityEngine.Random.Range(0, lines.Length)];
        instance.StopAllCoroutines();
        instance.StartCoroutine(instance.TypeLine(randomLine, textSpeed));
    }

    private IEnumerator TypeLine(string line, float textSpeed)
    {
        textComponent.text = string.Empty;
        foreach (char c in line.ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

#if UNITY_EDITOR
    private void OnEnable()
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