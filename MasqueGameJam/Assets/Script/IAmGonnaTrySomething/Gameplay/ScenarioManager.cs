using System;
using System.Collections;
using System.Linq;
using Script.IAmGonnaTrySomething.Data.EventsScripts;
using Script.IAmGonnaTrySomething.Gameplay;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public class ScenarioManager : MonoBehaviour
{
    [SerializeField] private DramaObject[] allObjects;
    [SerializeField] private SceneData firstScene;
    private GameObject[] allObjectsRefs;
    private SceneData currentScene;
    private Vector3 objectsOffset;

    [SerializeField] private Light2D light;
    [SerializeField] private ChoiceButton[] buttonsForChoices;
    [SerializeField] private DialoguePanel dialoguePanel;
    [SerializeField] private GameObject smallWheel;
    [SerializeField] private ScreenTransition screenTransition;

    public event Action<SequenceForScene> SceneFinishedAction;
    void Start()
    {
        objectsOffset = new Vector3(0, -1.5f, 0);
        light.gameObject.SetActive(false);
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
        StartCoroutine(ShowWheel());
        for (int i = 0; i < currentScene.sequences.Length; i++)
        {
            buttonsForChoices[i].gameObject.SetActive(true);
            buttonsForChoices[i].SetUp(this, currentScene.sequences[i]);
        }
    }
    
    public void CallSequence(SequenceForScene sequence)
    {
        StartCoroutine(HideWheel());
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
                    yield return new WaitForSeconds((currentDialogueEvent.content.Length*0.01f)+3);
                    dialoguePanel.gameObject.SetActive(false);
                    break;
                }
                case ScenarioEventTypes.Sound:
                {
                    SoundEvent currentSoundEvent = (SoundEvent)sequence.events[currentEventIndex];
                    SoundManager.PlaySound(currentSoundEvent.soundType, currentSoundEvent.volume);
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
                case ScenarioEventTypes.FlipPose:
                {
                    FlipPoseEvent currentFlipPoseEvent = (FlipPoseEvent)sequence.events[currentEventIndex];
                    SpriteRenderer objectToFlip = FindTarget(currentFlipPoseEvent.target).gameObject.GetComponent<SpriteRenderer>();
                    if (objectToFlip == null)
                    {
                        break;
                    }
                    if (objectToFlip.flipX)
                    {
                        objectToFlip.flipX = false;
                    }
                    else
                    {
                        objectToFlip.flipX = true;
                    }
                    break;
                }
                case ScenarioEventTypes.Disappear:
                {
                    DisappearEvent currentDisappearEvent = (DisappearEvent)sequence.events[currentEventIndex];
                    SpriteRenderer objectToDisappear = FindTarget(currentDisappearEvent.target).gameObject.GetComponent<SpriteRenderer>();
                    if (objectToDisappear == null)
                    {
                        break;
                    }

                    objectToDisappear.sprite = null;
                    break;
                }
                case ScenarioEventTypes.Light:
                {
                    LightEvent currentLightEvent = (LightEvent)sequence.events[currentEventIndex];
                    GameObject objectToLight = FindTarget(currentLightEvent.target).gameObject;
                    GameObject lightOperator = FindTarget(ObjectsID.LightOperator).gameObject;
                    if (objectToLight == null || lightOperator == null)
                    {
                        break;
                    }
                    light.gameObject.SetActive(true);
                    light.gameObject.transform.position = lightOperator.transform.position;
                    Vector2 direction = objectToLight.transform.position - light.transform.position;
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    Debug.Log(angle);
                    light.transform.rotation = Quaternion.Euler(0f, 0f, (angle-110));
                    light.color = currentLightEvent.lightColor;
                    if (currentLightEvent.general)
                    {
                        light.falloffIntensity = 0.5f;
                    }
                    else
                    {
                        light.falloffIntensity = 1f;
                    }
                    break;
                }
                case ScenarioEventTypes.Unlight:
                {
                    light.gameObject.SetActive(false);
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
        else 
        {
                currentScene = sequence.nextScene; 
                screenTransition.TransitionAppear(); 
                yield return new WaitForSeconds(screenTransition.transitionTime); 
                light.gameObject.SetActive(false);
                Debug.Log(light.gameObject.activeInHierarchy);
                SceneFinishedAction?.Invoke(sequence);
        }
    }

    IEnumerator ShowWheel()
    {
        Vector2 targetPosition = new Vector2(0, -3.5f);
        float speed = 1.5f;
        while (smallWheel.transform.position.y < targetPosition.y)
        {
            smallWheel.transform.position = Vector3.MoveTowards(smallWheel.transform.position, targetPosition, Time.deltaTime *speed );
            yield return null;
        }
    }

    IEnumerator HideWheel()
    {
        Vector2 targetPosition = new Vector2(0, -8f);
        float speed = 1.5f;
        while (smallWheel.transform.position.y > targetPosition.y)
        {
            smallWheel.transform.position = Vector3.MoveTowards(smallWheel.transform.position, targetPosition, Time.deltaTime *speed );
            yield return null;
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

    public void NextScene()
    {
        screenTransition.TransitionDisappear();
        if (currentScene.setUpSequence != null)
        {
            CallSequence(currentScene.setUpSequence);
        }
        else
        {
            DisplayChoices();
        }
    }
}
