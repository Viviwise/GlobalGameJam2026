namespace Script.Choice
{
    using UnityEngine;

    public class Choice : MonoBehaviour, IInteractable
    {
        [SerializeField] private GameEvent actionEvent; // ou DialogueManager etc.

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
            if (InteractionBlocker.BlockAllInteractions) return;

            actionEvent?.Raise();
            ChoiceManager.Instance.ConfirmChoice();        }
    }

}