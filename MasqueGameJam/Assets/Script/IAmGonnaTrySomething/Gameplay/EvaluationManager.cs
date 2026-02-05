using System;
using System.Collections;
using Script.IAmGonnaTrySomething.Gameplay;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vector3 = System.Numerics.Vector3;

public class EvaluationManager : MonoBehaviour
{
    [SerializeField] private DialoguePanel dialoguePanel;
    [SerializeField] private Transform catharsys;
    [SerializeField] private SpriteRenderer catharsysFace;
    [SerializeField] public WheelManager wheelManager ;

    [SerializeField] private Sprite baseFace, maxMadFace, maxBoredFace;
    [SerializeField] private string killSentenceBoredom, killSentenceBad, calmedSentence;
    private SequenceForScene currentSequence;
    [SerializeField] private ScreenTransitionCurtain screenTransitionCurtain;

    public event Action EvaluationPhaseDone;
    public event Action KillActor;
    private void Start()
    {
        dialoguePanel.gameObject.SetActive(false);
        wheelManager.EndPhaseSurviving += Entrance;
        wheelManager.EndPhaseDying += KillCharacter;
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
        catharsysFace.sprite = baseFace;
        Vector2 goalPosition = new Vector2(50,0);
        while (Vector2.Distance(catharsys.position, goalPosition) > 0.1f)
        {
            catharsys.position = Vector2.MoveTowards(catharsys.position, goalPosition, Time.deltaTime * 5f);
            yield return null;
        }
        yield return new WaitForSeconds(2f);
        SoundManager.PlaySound(currentSequence.catharsysReactionSound);
        catharsysFace.sprite = currentSequence.CatharsysReaction;
        dialoguePanel.gameObject.SetActive(true);
        dialoguePanel.SetUp(currentSequence.reactionText, "Catharsys", 0.02f);
        yield return new WaitForSeconds(4f);
        dialoguePanel.gameObject.SetActive(false);
        EvaluationPhaseDone?.Invoke();
    }

    IEnumerator CatharysEvaluation()
    {
        catharsys.position = new Vector2(50, -10);
        yield return new WaitForSeconds(1f);
        wheelManager.AjustWheel(currentSequence.wheelScore);
    }

    private void KillCharacter(int score)
    {
        StartCoroutine(CatharsysDeathEntrance(score));
    }

    IEnumerator CatharsysDeathEntrance(int score)
    {
        Vector2 goalPosition = new Vector2(50,0);
        catharsysFace.sprite = currentSequence.CatharsysReaction;

        while (Vector2.Distance(catharsys.position, goalPosition) > 0.1f)
        {
            catharsys.position = Vector2.MoveTowards(catharsys.position, goalPosition, Time.deltaTime * 8f);
            yield return null;
        }
        yield return new WaitForSeconds(2f);

        dialoguePanel.gameObject.SetActive(true);
        if (score >= 3)
        {
            SoundManager.PlaySound(SoundType.CatharsysMad);
            dialoguePanel.SetUp(killSentenceBoredom, "Catharsys", 0.02f);
        }
        else if (score <= -3)
        {
            SoundManager.PlaySound(SoundType.CatharsysBored);
            dialoguePanel.SetUp(killSentenceBad, "Catharsys", 0.02f);
        }
        yield return new WaitForSeconds(2f);
        dialoguePanel.gameObject.SetActive(false);

        KillActor?.Invoke();
        
        yield return new WaitForSeconds(10f);
        dialoguePanel.gameObject.SetActive(true);
        dialoguePanel.SetUp(calmedSentence, "Catharsys", 0.02f);
        wheelManager.ResetCursorsForKillActor();

        yield return new WaitForSeconds(3f);
        dialoguePanel.gameObject.SetActive(false);

        if (wheelManager.lives == 0)
        {
            screenTransitionCurtain.FermetureRideaux();
            screenTransitionCurtain.TransitionAppear();
            yield return new WaitForSeconds(3f);
            SceneManager.LoadScene("BadEnd");
        }

        EvaluationPhaseDone?.Invoke();
    }
}

public enum WheelScore
{
    Boredom,
    Bad,
    Good,
}
