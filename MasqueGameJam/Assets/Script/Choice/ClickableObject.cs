using Script;
using Script.Choice;
using UnityEngine;

public class ClickableObject : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject choicePrefab1;
    [SerializeField] private Transform choiceSpawnPoint;

    public void OnHoverEnter()
    {
        CursorManager.Instance.OnHoverEnter();
        if (InteractionBlocker.BlockAllInteractions) return;

        ChoiceManager.Instance.ShowChoices(gameObject, choicePrefab1, choiceSpawnPoint);    }

    public void OnHoverExit()
    { 
        CursorManager.Instance.OnHoverExit();
    }

    public void OnClick()
    {
        if (InteractionBlocker.BlockAllInteractions) return;
        ChoiceManager.Instance.ShowChoices(gameObject, choicePrefab1, choiceSpawnPoint);    }
}