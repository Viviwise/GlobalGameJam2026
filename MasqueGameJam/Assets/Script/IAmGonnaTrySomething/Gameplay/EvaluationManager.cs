using System;
using System.Collections;
using Script.IAmGonnaTrySomething.Gameplay;
using Unity.VisualScripting;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

public class EvaluationManager : MonoBehaviour
{
    [SerializeField] private DialoguePanel dialoguePanel;
    [SerializeField] private Transform catharsys;
    [SerializeField] private SpriteRenderer catharsysFace;
    [SerializeField] public WheelManager wheelManager ;

    private SequenceForScene currentSequence;

    public event Action EvalutaionPhaseDone;
    private void Start()
    {
        dialoguePanel.gameObject.SetActive(false);
        wheelManager.EndPhaseSurviving += Entrance;
    }

    public void EvaluateSequence(SequenceForScene sequence)
    {
        currentSequence = sequence;
        StartCoroutine(CatharysEvaluation());
    }

    private void Entrance()
    {
        StartCoroutine(CatharsysEntrance());
    }
    IEnumerator CatharsysEntrance()
    {
        Vector2 goalPosition = new Vector2(50,0);
        while (Vector2.Distance(catharsys.position, goalPosition) > 0.1f)
        {
            catharsys.position = Vector2.MoveTowards(catharsys.position, goalPosition, Time.deltaTime * 5f);
            yield return null;
        }
        yield return new WaitForSeconds(2f);
        catharsysFace.sprite = currentSequence.CatharsysReaction;
        dialoguePanel.gameObject.SetActive(true);
        dialoguePanel.SetUp(currentSequence.reactionText, "Catharsys");
        yield return new WaitForSeconds(4f);
        dialoguePanel.gameObject.SetActive(false);
        EvalutaionPhaseDone?.Invoke();
    }

    IEnumerator CatharysEvaluation()
    {
        catharsys.position = new Vector2(50, -10);
        yield return new WaitForSeconds(1f);
        wheelManager.AjustWheel(currentSequence.wheelScore);
    }

}

public enum WheelScore
{
    Boredom,
    Bad,
    Good,
}
