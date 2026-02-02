using System;
using System.Collections;
using UnityEngine;

public class PhasesManager : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private ScenarioManager scenarioManager;
    [SerializeField] private EvaluationManager evaluationManager;
    [SerializeField] private ScreenTransition screenTransition;
    private void Start()
    {
        cam = Camera.main;
        scenarioManager.SceneFinishedAction += SceneEnded;
        evaluationManager.EvaluationPhaseDone += EvaluationEnded;
        evaluationManager.KillActor += KillActor;
    }

    private void SceneEnded(SequenceForScene sequencePlayed)
    {
        cam.transform.position = new Vector3(50, 0 ,-10);
        screenTransition.TransitionDisappear();
        evaluationManager.EvaluateSequence(sequencePlayed);
    }

    private void EvaluationEnded()
    {
        StartCoroutine(BackToStage());
    }

    private void KillActor()
    {
        StartCoroutine(KillActorSequence());
    }

    IEnumerator KillActorSequence()
    {
        screenTransition.TransitionAppear();
        yield return new WaitForSeconds(screenTransition.transitionTime);
        cam.transform.position = new Vector3(0, 0 ,-10);
        screenTransition.TransitionDisappear();
        scenarioManager.KillActor();
        yield return new WaitForSeconds(5f);
        screenTransition.TransitionAppear();
        yield return new WaitForSeconds(screenTransition.transitionTime);
        cam.transform.position = new Vector3(50, 0 ,-10);
        screenTransition.TransitionDisappear();
    }
    IEnumerator BackToStage()
    {
        screenTransition.TransitionAppear();
        yield return new WaitForSeconds(screenTransition.transitionTime);
        cam.transform.position = new Vector3(0, 0 ,-10);
        scenarioManager.NextScene();
    }
}
