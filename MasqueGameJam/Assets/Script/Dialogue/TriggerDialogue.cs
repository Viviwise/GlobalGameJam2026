using UnityEngine;

public class TriggerDialogue : MonoBehaviour
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
        
    }
}
