using System.Collections;
using UnityEngine;

[System.Serializable]
public class CustomDialogue
{
    [TextArea(2, 5)]
    public string[] lines;
}

public class SceneSequenceController : MonoBehaviour
{
    [Header("Béatrice")]
    public Animator beatriceAnimator;
    public string beatriceOrationTrigger;
    public string beatriceIdleTrigger;
    public CustomDialogue beatriceDialogue1;
    public CustomDialogue beatriceDialogue2;

    [Header("William")]
    public Animator williamAnimator;
    public string williamOrationTrigger;
    public string williamIdleTrigger;
    public string williamMoveTrigger;
    public CustomDialogue williamDialogue;

    // Lancer toute la scène
    public void PlayScene()
    {
        StartCoroutine(SceneCoroutine());
    }

    private IEnumerator SceneCoroutine()
    {
        //  Béatrice oration + dialogue
        beatriceAnimator.SetTrigger(beatriceOrationTrigger);
        DialogueManager.StartCustomDialogue(beatriceDialogue1.lines);
        yield return new WaitUntil(() => DialogueManager.IsDialogueFinished);

        //  Béatrice idle
        beatriceAnimator.SetTrigger(beatriceIdleTrigger);
        yield return new WaitForSeconds(0.2f);

        //  William oration + dialogue
        williamAnimator.SetTrigger(williamOrationTrigger);
        DialogueManager.StartCustomDialogue(williamDialogue.lines);
        yield return new WaitUntil(() => DialogueManager.IsDialogueFinished);

        //  William idle
        williamAnimator.SetTrigger(williamIdleTrigger);
        yield return new WaitForSeconds(0.2f);

        //  Béatrice reparle
        DialogueManager.StartCustomDialogue(beatriceDialogue2.lines);
        yield return new WaitUntil(() => DialogueManager.IsDialogueFinished);

        //  William se déplace et sort
        williamAnimator.SetTrigger(williamMoveTrigger);
    }
    
}
