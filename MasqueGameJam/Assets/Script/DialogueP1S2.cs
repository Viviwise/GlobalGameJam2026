using Script;
using Unity.VisualScripting;
using UnityEngine;


public class DialogueP1S2 : MonoBehaviour, IInteractable
{
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void OnHoverEnter()
    {
        CursorManager.Instance.OnHoverEnter();
    }

    public void OnHoverExit()
    {
        CursorManager.Instance.OnHoverExit();

    }

    public void OnClick()
    {
        DialogueManager.StartDialogue(DialogueType.ANGRY);
        Destroy(gameObject);

    }
}
