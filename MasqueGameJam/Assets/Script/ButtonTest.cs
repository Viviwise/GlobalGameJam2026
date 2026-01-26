using System;
using Script;
using UnityEngine;

public class ButtonTest : MonoBehaviour, IInteractable
{
    public void OnHoverEnter()
    {
        SoundManager.PlaySound(SoundType.FLIP);

    }

    public void OnHoverExit()
    {
    }

    public void OnClick()
    {
        Debug.Log("Nik fait un truc");
        DialogueManager.StartDialogue(DialogueType.HAPPY);
    }
}
