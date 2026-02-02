using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class PhasesManager : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private ScenarioManager scenarioManager;
    [SerializeField] private EvaluationManager evaluationManager;
    [FormerlySerializedAs("screenTransition")] [SerializeField] private ScreenTransitionCurtain screenTransitionCurtain;
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
        screenTransitionCurtain.TransitionDisappear();
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
        screenTransitionCurtain.TransitionAppear();
        yield return new WaitForSeconds(screenTransitionCurtain.transitionTime);
        cam.transform.position = new Vector3(0, 0 ,-10);
        screenTransitionCurtain.TransitionDisappear();
        scenarioManager.KillActor();
        yield return new WaitForSeconds(5f);
        screenTransitionCurtain.TransitionAppear();
        yield return new WaitForSeconds(screenTransitionCurtain.transitionTime);
        cam.transform.position = new Vector3(50, 0 ,-10);
        screenTransitionCurtain.TransitionDisappear();
    }
    IEnumerator BackToStage()
    {
        screenTransitionCurtain.TransitionAppear();
        yield return new WaitForSeconds(screenTransitionCurtain.transitionTime);
        cam.transform.position = new Vector3(0, 0 ,-10);
        scenarioManager.NextScene();
    }
}
