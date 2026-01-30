using System;
using System.Collections;
using Script.IAmGonnaTrySomething.Gameplay;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

public class EvaluationManager : MonoBehaviour
{
    [SerializeField] private DialoguePanel dialoguePanel;
    [SerializeField] private Transform catharsys;
    [SerializeField] private SpriteRenderer catharsysFace;
    [SerializeField] private ScreenTransition screenTransition;
    [SerializeField] private WheelManager wheelManager;

    private void Start()
    {
        dialoguePanel.gameObject.SetActive(false);
    }

    public void EvaluateSequence(SequenceForScene sequence)
    {
        screenTransition.TransitionDisappear();
        StartCoroutine(CatharsysEntrance(sequence));
        
    }

    IEnumerator CatharsysEntrance(SequenceForScene sequence)
    {
        Vector2 goalPosition = new Vector2(0, 0);
        while (Vector2.Distance(catharsys.position, goalPosition) > 0.1f)
        {
            catharsys.position = Vector2.MoveTowards(catharsys.position, goalPosition, Time.deltaTime * 5f);
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        Expression(sequence.CatharsysReaction);
        dialoguePanel.gameObject.SetActive(true);
        dialoguePanel.SetUp(sequence.reactionText, "Catharsys");
        yield return new WaitForSeconds(4f);
        StartCoroutine(CatharysEvaluation(sequence));
    }

    IEnumerator CatharysEvaluation(SequenceForScene sequence)
    {
        Vector2 goalPosition = new Vector2(0, 10);
        while (Vector2.Distance(catharsys.position, goalPosition) > 0.1f)
        {
            catharsys.position = Vector2.MoveTowards(catharsys.position, goalPosition, Time.deltaTime * 5f);
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        wheelManager.AjustWheel(sequence.wheelScore);
    }

    private void Expression(Sprite reaction)
    {
        catharsysFace.sprite = reaction;
    }
}

public enum WheelScore
{
    Boredom,
    Bad,
}
