using System;
using UnityEngine;

public class PhasesManager : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private ScenarioManager scenarioManager;
    [SerializeField] private EvaluationManager evaluationManager;
    private void Start()
    {
        cam = Camera.main;
        scenarioManager.SceneFinishedAction += SceneEnded;
    }

    private void SceneEnded(SequenceForScene sequencePlayed)
    {
        cam.transform.position = new Vector3(50, 0 ,0);
        evaluationManager.EvaluateSequence(sequencePlayed);
    }
}
