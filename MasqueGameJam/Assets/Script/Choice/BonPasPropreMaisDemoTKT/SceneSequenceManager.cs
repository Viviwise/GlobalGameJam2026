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

    private Dictionary<CharacterID, CharacterActor> actors =
        new Dictionary<CharacterID, CharacterActor>();

    void Start()
    {
        if (firstScene == null)
            return;

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

        actors.Clear();

        foreach (var prefab in scene.characters)
        {
            GameObject go = Instantiate(prefab, characterContainer);

            CharacterActor actor = go.GetComponent<CharacterActor>();
            if (actor == null)
            {
                Debug.LogError($"Prefab {prefab.name} n'a pas de CharacterActor !");
                continue;
            }

            actors.Add(actor.characterID, actor);
        }

        // --- Choices ---
        ShowChoices(scene.choices);
    }

    void ShowChoices(Choice[] choices)
    {
        if (choicePanel == null || choiceButtonPrefab == null)
            return;

        foreach (Transform t in choicePanel)
            Destroy(t.gameObject);

        if (choices == null || choices.Length == 0)
            return;

        foreach (var choice in choices)
        {
            GameObject buttonGO = Instantiate(choiceButtonPrefab, choicePanel);
            Button button = buttonGO.GetComponent<Button>();
            TMP_Text text = buttonGO.GetComponentInChildren<TMP_Text>();

            if (text == null)
                continue;

            text.text = choice.label;
            button.onClick.AddListener(() => OnChoiceClicked(choice));
        }
    }

    void OnChoiceClicked(Choice choice)
    {
        foreach (Transform t in choicePanel)
        {
            Button btn = t.GetComponent<Button>();
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
                runningCoroutines.Add(StartCoroutine(ExecuteAction(action)));

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
                if (!actors.TryGetValue(action.character, out var actor))
                {
                    Debug.LogError($"Actor {action.character} introuvable !");
                    yield break;
                }

                if (actor.animator == null || action.animationClip == null)
                    yield break;

                actor.animator.Play(action.animationClip.name, 0, 0f);
                break;
            }

            case ActionType.Sound:
            {
                if (SoundManager.instance != null)
                    SoundManager.PlaySound(action.soundType);

                if (action.blocking)
                {
                    float wait = action.duration > 0 ? action.duration : 1f;
                    yield return new WaitForSeconds(wait);
                }
                break;
            }

            case ActionType.Dialogue:
            {
                if (!actors.TryGetValue(action.character, out var actor))
                    yield break;

                float duration = action.duration > 0 ? action.duration : 2f;

                if (dialogueManager != null)
                {
                    dialogueManager.ShowDialogue(
                        actor.displayName,
                        action.dialogueText,
                        duration
                    );
                }
                else
                {
                    Debug.Log($"{actor.displayName} : {action.dialogueText}");
                }

                yield return new WaitForSeconds(duration);
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
    }

    void ShowNextSceneButton(SceneSO nextScene)
    {
        if (choiceButtonPrefab == null || choicePanel == null)
            return;

        GameObject buttonGO = Instantiate(choiceButtonPrefab, choicePanel);
        TMP_Text text = buttonGO.GetComponentInChildren<TMP_Text>();
        text.text = "Suivant";

        Button button = buttonGO.GetComponent<Button>();
        button.onClick.AddListener(() => LoadScene(nextScene));
    }
}
