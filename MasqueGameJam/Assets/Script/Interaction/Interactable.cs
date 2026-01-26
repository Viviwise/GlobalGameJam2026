using Script;
using UnityEngine;

public class Interactable : MonoBehaviour, IInteractable
{
    public virtual void OnHoverEnter()
    {
        CursorManager.Instance.OnHoverEnter();
    }

    public virtual void OnHoverExit()
    {
        CursorManager.Instance.OnHoverExit();
    }

    public virtual void OnClick()
    {
        Debug.Log("Clicked on " + name);
    }
}