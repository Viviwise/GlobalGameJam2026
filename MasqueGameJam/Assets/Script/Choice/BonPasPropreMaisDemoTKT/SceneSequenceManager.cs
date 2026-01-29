using System.Collections;
using System.Collections.Generic;
using Script.Choice.BonPasPropreMaisDemoTKT;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class SceneSequenceManager : MonoBehaviour
{
    [Header("UI References")]
    public SpriteRenderer backgroundImage;         
    public Transform choicePanel;         
    public GameObject choiceButtonPrefab; 

    [Header("Scene Setup")]
    public Transform characterContainer;  
    public SceneSO firstScene;
    
    [Header("Dialogue")]
    public DialogueManager dialogueManager;


    private SceneSO currentScene;

    void Start()
    {
        if (firstScene == null)
        {
            return;
        }

        LoadScene(firstScene);
    }
    
    public void LoadScene(SceneSO scene)
    {
        currentScene = scene;

        // --- Background ---
        if (backgroundImage != null && scene.background != null)
            backgroundImage.sprite = scene.background;

        // --- Characters ---
        foreach (Transform t in characterContainer)
            Destroy(t.gameObject);

        foreach (var character in scene.characters)
            Instantiate(character, characterContainer);

        // --- Choices ---
        ShowChoices(scene.choices);
    }
    
    void ShowChoices(Choice[] choices)
    {
        if (choicePanel == null || choiceButtonPrefab == null)
        {
            return;
        }

        foreach (Transform t in choicePanel)
            Destroy(t.gameObject);

        if (choices == null || choices.Length == 0)
        {
            return;
        }

        foreach (var choice in choices)
        {
            var buttonGO = Instantiate(choiceButtonPrefab, choicePanel);
            var button = buttonGO.GetComponent<Button>();
            var text = buttonGO.GetComponentInChildren<TMP_Text>();

            if (text == null)
            {
                continue;
            }

            text.text = choice.label;

            button.onClick.AddListener(() => OnChoiceClicked(choice));
        }
    }
    
    void OnChoiceClicked(Choice choice)
    {
        foreach (Transform t in choicePanel)
        {
            var btn = t.GetComponent<Button>();
            if (btn != null) btn.interactable = false;
        }

        StartCoroutine(PlaySequence(choice.sequence, choice.targetScene));
    }

    IEnumerator PlaySequence(Sequence sequence, SceneSO nextScene)
    {
        if (sequence == null || sequence.groups == null || sequence.groups.Length == 0)
        {
            ShowNextSceneButton(nextScene);
            yield break;
        }


        foreach (var group in sequence.groups)
        {
            if (group.actions == null || group.actions.Length == 0)
                continue;

            List<Coroutine> runningCoroutines = new List<Coroutine>();

            foreach (var action in group.actions)
            {
                runningCoroutines.Add(StartCoroutine(ExecuteAction(action)));
            }

            foreach (var c in runningCoroutines)
                yield return c;
        }

        ShowNextSceneButton(nextScene);
    }

    IEnumerator ExecuteAction(SequenceAction action)
    {
        if (action == null)
            yield break;

        switch (action.type)
        {
            case ActionType.PlayAnimation:
            {
                if (characterContainer == null)
                {
                    yield break;
                }

                Transform actor = characterContainer.Find(action.character);
                if (actor == null)
                {
                    yield break;
                }

                Animator animator = actor.GetComponent<Animator>();
                if (animator == null)
                {
                    yield break;
                }

                if (action.animationClip == null)
                {
                    yield break;
                }
                
                animator.Play(action.animationClip.name, 0, 0f);

                yield return null;

                break;
            }
            
            case ActionType.Sound:
            {
                if (SoundManager.instance != null)
                {
                    SoundManager.PlaySound(action.soundType);
                }
                if (action.blocking)
                {
                    float wait = action.duration > 0 ? action.duration : 1f;
                    yield return new WaitForSeconds(wait);
                }
                break;
            }



            case ActionType.Dialogue:
            {
                if (dialogueManager != null)
                {
                    float duration = action.duration > 0 ? action.duration : 2f;
                    dialogueManager.ShowDialogue(action.character, action.dialogueText, duration);
                    yield return new WaitForSeconds(duration);
                }
                else
                {
                    Debug.Log($"{action.character}: {action.dialogueText}");
                    yield return new WaitForSeconds(action.duration > 0 ? action.duration : 2f);
                }
                break;
            }

            case ActionType.Wait:
                {
                    float duration = action.duration > 0 ? action.duration : 1f;
                    yield return new WaitForSeconds(duration);
                    break;
                }
            case ActionType.Spawn:
                {
                    if (action.prefabToSpawn != null)
                        Instantiate(action.prefabToSpawn, characterContainer);
                    break;
                }
        }

        yield return null;
    }
    
    void ShowNextSceneButton(SceneSO nextScene)
    {
        if (choiceButtonPrefab == null || choicePanel == null)
        {
            return;
        }

        var buttonGO = Instantiate(choiceButtonPrefab, choicePanel);
        var text = buttonGO.GetComponentInChildren<TMP_Text>();
        text.text = "Suivant";

        var button = buttonGO.GetComponent<Button>();
        button.onClick.AddListener(() => LoadScene(nextScene));
    }
}
