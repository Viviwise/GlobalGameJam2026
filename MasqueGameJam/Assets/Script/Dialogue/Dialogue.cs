using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textCompenment;
    public string[] sentences;
    public float textSpeed;

    private int index;

    void Start()
    {
        textCompenment.text = string.Empty;
        StartDialogue();
    }
    void Update()
    {
        if ((Keyboard.current.spaceKey.wasPressedThisFrame) ||
            (Mouse.current.leftButton.wasPressedThisFrame))
        {
            if (textCompenment.text == sentences[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textCompenment.text = sentences[index];
            }
        }
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(typeLine());
    }

    IEnumerator typeLine()
    {
        foreach (char c  in  sentences[index].ToCharArray())
        {
            textCompenment.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }
    void NextLine()
    {
        if (index < sentences.Length -1)
        {
            index++;
            textCompenment.text = string.Empty;
            StartCoroutine(typeLine());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

}
