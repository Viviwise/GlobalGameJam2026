using System;
using Script;
using UnityEngine;


public class DialogueP1S1 : MonoBehaviour, IInteractable
{
    public void OnHoverEnter()
    {
        SoundManager.PlaySound(SoundType.FLIP);
        CursorManager.Instance.OnHoverEnter();
    }

    public void OnHoverExit()
    {
        CursorManager.Instance.OnHoverExit();
    }

    public void OnClick()
    {
        DialogueManager.StartDialogue(DialogueType.HAPPY); 
        Destroy(gameObject);
    }
}
