using Script;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionManager : MonoBehaviour
{
    private IInteractable currentHover;

    void Update()
    {
        Vector2 mouseScreen = Mouse.current.position.ReadValue();
        Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);

        RaycastHit2D hit = Physics2D.Raycast(mouseWorld, Vector2.zero);

        IInteractable newHover =
            hit.collider ? hit.collider.GetComponent<IInteractable>() : null;

        if (newHover != currentHover)
        {
            if (currentHover != null)
                currentHover.OnHoverExit();

            if (newHover != null)
                newHover.OnHoverEnter();

            currentHover = newHover;
        }

        if (currentHover != null &&
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            currentHover.OnClick();
        } 
    } 
}