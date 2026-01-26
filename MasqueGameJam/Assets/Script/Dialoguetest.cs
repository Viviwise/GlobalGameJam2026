using Script;
using UnityEngine;

public class Dialoguetest : MonoBehaviour, IInteractable
{
    public void OnHoverEnter()
    {
    }

    public void OnHoverExit()
    {
    }

    public void OnClick()
    {
        DialogueManager.StartDialogue(DialogueType.ANGRY);
    }
}
