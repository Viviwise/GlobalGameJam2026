using System;
using System.Collections;
using System.Linq;
using Script.IAmGonnaTrySomething.Data.EventsScripts;
using Script.IAmGonnaTrySomething.Gameplay;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;
using Random = System.Random;

[DefaultExecutionOrder(1)]
public class ScenarioManager : MonoBehaviour
{
    [SerializeField] private DramaObject[] allObjects;
    [SerializeField] private SceneData firstScene;
    private GameObject[] allObjectsRefs;
    private SceneData currentScene;
    private Vector3 objectsOffset;

    [SerializeField] private Light2D lightObject;
    [SerializeField] private ParticleSystem explosionEffect;
    [SerializeField] private ChoiceButton[] buttonsForChoices;
    [SerializeField] private DialoguePanel dialoguePanel;
    [SerializeField] private GameObject smallWheel;
    [FormerlySerializedAs("screenTransition")] [SerializeField] private ScreenTransitionCurtain screenTransitionCurtain;
    
    [SerializeField] private OpenCurtain openCurtain;
    [SerializeField] private FrameMove frameMove;


    public event Action<SequenceForScene> SceneFinishedAction;
    void Start()
    {
        currentScene = firstScene;
        if (currentScene.setUpSequence != null)
        {
            CallSequence(currentScene.setUpSequence);
        }
    }

    public void SetUpAll()
    {
        objectsOffset = new Vector3(0, -1.5f, 0);
        lightObject.gameObject.SetActive(false);
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
                    int dialogueIndex = 0;
                    float textSpeed = 0.01f;
                    while (dialogueIndex < currentDialogueEvent.contents.Length)
                    {
                        SoundManager.PlaySound(currentDialogueEvent.sound,0.6f);
                        dialoguePanel.SetUp(currentDialogueEvent.contents[dialogueIndex], FindTarget(currentDialogueEvent.character).GetComponent<DramaObject>().actorName, textSpeed);
                        yield return new WaitForSeconds((currentDialogueEvent.contents[dialogueIndex].Length*textSpeed)+3);
                        dialogueIndex++;
                    }
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

                    if (characterSprite.gameObject.GetComponent<DramaObject>().dead)
                    {
                        characterSprite.sprite = currentChangePoseEvent.newPoseDead;
                    }
                    else
                    {
                        characterSprite.sprite = currentChangePoseEvent.newPose;
                    }
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
                    lightObject.gameObject.SetActive(true);
                    lightObject.gameObject.transform.position = lightOperator.transform.position;
                    Vector2 direction = objectToLight.transform.position - lightObject.transform.position;
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    Debug.Log(angle);
                    lightObject.transform.rotation = Quaternion.Euler(0f, 0f, (angle-110));
                    lightObject.color = currentLightEvent.lightColor;
                    if (currentLightEvent.general)
                    {
                        lightObject.falloffIntensity = 0.5f;
                    }
                    else
                    {
                        lightObject.falloffIntensity = 1f;
                    }
                    break;
                }
                case ScenarioEventTypes.Unlight:
                {
                    lightObject.gameObject.SetActive(false);
                    break;
                }

                case ScenarioEventTypes.OpenCurtain:
                {
                    openCurtain.Ouvrir();
                    break;
                }

                case ScenarioEventTypes.CloseCurtain:
                {
                    openCurtain.Fermer();
                    break;
                }
                
                case ScenarioEventTypes.SisypheAnim:
                {
                    SisypheAnimEvent animEvent = (SisypheAnimEvent)sequence.events[currentEventIndex];

                    GameObject target = FindTarget(animEvent.target);
                    if (target == null) break;

                    Animator animator = target.GetComponent<Animator>();
                    if (animator == null) break;
                    animator.Play(animEvent.animatorStateName, 0, 0f);

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
                screenTransitionCurtain.TransitionAppear(); 
                yield return new WaitForSeconds(screenTransitionCurtain.transitionTime); 
                lightObject.gameObject.SetActive(false);
                Debug.Log(lightObject.gameObject.activeInHierarchy);
                SceneFinishedAction?.Invoke(sequence);
        }
    }

    IEnumerator ShowWheel()
    {
        Vector2 targetPosition = new Vector2(0, -3.5f);
        float speed = 3f;
        while (smallWheel.transform.position.y < targetPosition.y)
        {
            smallWheel.transform.position = Vector3.MoveTowards(smallWheel.transform.position, targetPosition, Time.deltaTime *speed );
            yield return null;
        }
    }

    IEnumerator HideWheel()
    {
        Vector2 targetPosition = new Vector2(0, -8f);
        float speed = 3f;
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

    public void KillActor()
    {
        bool targetFound = false;
        DramaObject target = null;
        int tries = 0;
        while (!targetFound && tries < 20)
        {
            int randomSelection = UnityEngine.Random.Range(0, allObjects.Length);
            DramaObject targetObject = allObjects[randomSelection];
            if (targetObject.killable && targetObject.gameObject.transform.position.x>=-4 && targetObject.gameObject.transform.position.x <=4)
            {
                targetFound = true;
                target = targetObject;
                tries++;
            }
        }
        target.Explode(explosionEffect);

    }

    public void NextScene()
    {
        screenTransitionCurtain.TransitionDisappear();
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
