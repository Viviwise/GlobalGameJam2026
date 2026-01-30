using System;
using System.Collections;
using System.Linq;
using Script.IAmGonnaTrySomething.Gameplay;
using UnityEngine;
using UnityEngine.Events;

public class ScenarioManager : MonoBehaviour
{
    [SerializeField] private DramaObject[] allObjects;
    [SerializeField] private SceneData firstScene;
    private GameObject[] allObjectsRefs;
    private SceneData currentScene;
    private Vector3 objectsOffset;

    [SerializeField] private ChoiceButton[] buttonsForChoices;
    [SerializeField] private DialoguePanel dialoguePanel;
    [SerializeField] private ScreenTransition screenTransition;

    public Action<SequenceForScene> SceneFinishedAction;
    void Start()
    {
        objectsOffset = new Vector3(0, -1.5f, 0);
        currentScene = firstScene;
        allObjectsRefs = new GameObject[allObjects.Length];
        for (int i = 0; i < allObjects.Length; i++)
        {
            allObjectsRefs[i] = allObjects[i].gameObject;
            allObjectsRefs[i].transform.position = new Vector3(100, 100, 0);
        }
        dialoguePanel.gameObject.SetActive(false);
        for (int i = 0; i < buttonsForChoices.Length; i++)
        {
            buttonsForChoices[i].gameObject.SetActive(false);
        }

        if (currentScene.setUpSequence != null)
        {
            CallSequence(currentScene.setUpSequence);
        }
    }

    public void DisplayChoices()
    {
        for (int i = 0; i < currentScene.sequences.Length; i++)
        {
            buttonsForChoices[i].gameObject.SetActive(true);
            buttonsForChoices[i].SetUp(this, currentScene.sequences[i]);
        }
    }
    
    public void CallSequence(SequenceForScene sequence)
    {
        for (int i = 0; i < buttonsForChoices.Length; i++)
        {
            buttonsForChoices[i].gameObject.SetActive(false);
        }
        StartCoroutine(PlaySequence(sequence));
    }

    IEnumerator PlaySequence(SequenceForScene sequence)
    {
        int currentEventIndex = 0;
        while (currentEventIndex < sequence.events.Length)
        {
            EventForScene currentEvent = sequence.events[currentEventIndex];
            switch (sequence.events[currentEventIndex].eventType)
            {
                case ScenarioEventTypes.Movement:
                {
                    MovementEvent currentMovementEvent = (MovementEvent)sequence.events[currentEventIndex];
                    Transform objectToMove = FindTarget(currentMovementEvent.target).transform;
                    if (objectToMove == null)
                    {
                        break;
                    }
                    Vector3 target = currentMovementEvent.position+objectsOffset;
                    while (Vector3.Distance(objectToMove.position, target) > 0.1f)
                    {
                        objectToMove.position = Vector3.MoveTowards(objectToMove.position, target, Time.deltaTime * currentMovementEvent.speed);
                        yield return null;
                    }
                    break;
                }
                case ScenarioEventTypes.Teleportation:
                {
                    TeleportationEvent currentTeleportationEvent = (TeleportationEvent)sequence.events[currentEventIndex];
                    Transform objectToMove = FindTarget(currentTeleportationEvent.target).transform;
                    if (objectToMove == null)
                    {
                        break;
                    }
                    Vector3 target = currentTeleportationEvent.position+objectsOffset;
                    objectToMove.position = target;
                    break;
                }
                case ScenarioEventTypes.Dialogue:
                {
                    dialoguePanel.gameObject.SetActive(true);
                    DialogueEvent currentDialogueEvent = (DialogueEvent)sequence.events[currentEventIndex];
                    dialoguePanel.SetUp(currentDialogueEvent.content, currentDialogueEvent.character.ToString());
                    yield return new WaitForSeconds(currentDialogueEvent.displayTime);
                    dialoguePanel.gameObject.SetActive(false);
                    break;
                }
                case ScenarioEventTypes.Sound:
                {
                    break;
                }
                case ScenarioEventTypes.ChangePose:
                {
                    ChangePoseEvent currentChangePoseEvent = (ChangePoseEvent)sequence.events[currentEventIndex];
                    SpriteRenderer characterSprite = FindTarget(currentChangePoseEvent.id).gameObject.GetComponent<SpriteRenderer>();
                    if (characterSprite == null)
                    {
                        break;
                    }
                    characterSprite.sprite = currentChangePoseEvent.newPose;
                    break;
                }
                case ScenarioEventTypes.Delay:
                {
                    DelayEvent currentDelayEvent = (DelayEvent)sequence.events[currentEventIndex];
                    yield return new WaitForSeconds(currentDelayEvent.duration);
                    break;
                }
                    
            }
            currentEventIndex++;
            yield return null;
        }

        if (sequence.isSetUp)
        {
            DisplayChoices();
        }
        else if (!sequence.isCleanUp)
        {
            if (sequence.cleanUpSequence != null)
            {
                CallSequence(sequence.cleanUpSequence);
            }
            else
            {
                currentScene = sequence.nextScene; 
                screenTransition.TransitionAppear(); 
                yield return new WaitForSeconds(screenTransition.transitionTime); 
                screenTransition.TransitionDisappear(); 
                DisplayChoices();
            }
        }
        else
        {
            currentScene = sequence.nextScene; 
            screenTransition.TransitionAppear(); 
            yield return new WaitForSeconds(screenTransition.transitionTime); 
            CallSequence(currentScene.setUpSequence);
            screenTransition.TransitionDisappear(); 
            DisplayChoices();
        }
    }

    private GameObject FindTarget(ObjectsID id)
    {
        foreach (GameObject obj in allObjectsRefs)
        {
            if (obj.GetComponent<DramaObject>().id == id)
            {
                if (currentScene.actors.Contains(id))
                {
                    return obj;
                }
            }
        }
        return null;
    }
    
}
