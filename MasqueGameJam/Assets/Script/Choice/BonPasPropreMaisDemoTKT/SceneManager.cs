using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Pour TextMeshPro

public class SceneManager : MonoBehaviour
{
    [Header("UI References")]
    public SpriteRenderer backgroundImage;         // UI Image pour le background
    public Transform choicePanel;         // Panel pour afficher les boutons
    public GameObject choiceButtonPrefab; // Prefab du bouton (TMP_Text à l'intérieur)

    [Header("Scene Setup")]
    public Transform characterContainer;  // Container pour les personnages
    public SceneSO firstScene;            // Première scène à charger

    private SceneSO currentScene;

    void Start()
    {
        if (firstScene == null)
        {
            Debug.LogError("First Scene non assignée !");
            return;
        }

        LoadScene(firstScene);
    }

    /// <summary>
    /// Charge une scène ScriptableObject
    /// </summary>
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

    /// <summary>
    /// Affiche les boutons de choix dynamiquement
    /// </summary>
    void ShowChoices(Choice[] choices)
    {
        if (choicePanel == null || choiceButtonPrefab == null)
        {
            Debug.LogError("Choice Panel ou Button Prefab non assigné !");
            return;
        }

        // Clear old buttons
        foreach (Transform t in choicePanel)
            Destroy(t.gameObject);

        if (choices == null || choices.Length == 0)
        {
            Debug.LogWarning("Cette scène n'a aucun choix !");
            return;
        }

        foreach (var choice in choices)
        {
            var buttonGO = Instantiate(choiceButtonPrefab, choicePanel);
            var button = buttonGO.GetComponent<Button>();
            var text = buttonGO.GetComponentInChildren<TMP_Text>();

            if (text == null)
            {
                Debug.LogError("Le bouton prefab doit contenir un TMP_Text");
                continue;
            }

            text.text = choice.label;

            button.onClick.AddListener(() => OnChoiceClicked(choice));
        }
    }

    /// <summary>
    /// Quand le joueur clique sur un choix
    /// </summary>
    void OnChoiceClicked(Choice choice)
    {
        // Désactive tous les boutons pour éviter les clics multiples
        foreach (Transform t in choicePanel)
        {
            var btn = t.GetComponent<Button>();
            if (btn != null) btn.interactable = false;
        }

        StartCoroutine(PlaySequence(choice.sequence, choice.targetScene));
    }

    /// <summary>
    /// Exécute la séquence du choix, avec ActionGroup pour actions simultanées
    /// </summary>
    IEnumerator PlaySequence(Sequence sequence, SceneSO nextScene)
    {
        if (sequence == null || sequence.groups == null || sequence.groups.Length == 0)
        {
            Debug.LogWarning("Sequence vide !");
            ShowNextSceneButton(nextScene);
            yield break;
        }

        // Parcours chaque groupe (simultanéité à l'intérieur du groupe)
        foreach (var group in sequence.groups)
        {
            if (group.actions == null || group.actions.Length == 0)
                continue;

            List<Coroutine> runningCoroutines = new List<Coroutine>();

            foreach (var action in group.actions)
            {
                runningCoroutines.Add(StartCoroutine(ExecuteAction(action)));
            }

            // Attend que toutes les actions de ce groupe soient terminées
            foreach (var c in runningCoroutines)
                yield return c;
        }

        // Après la séquence, afficher le bouton "Suivant"
        ShowNextSceneButton(nextScene);
    }

    /// <summary>
    /// Execute une seule action (PlayAnimation, Dialogue, Wait, Spawn)
    /// </summary>
    IEnumerator ExecuteAction(SequenceAction action)
    {
        if (action == null)
            yield break;

        switch (action.type)
        {
            case ActionType.PlayAnimation:
                {
                    var actor = characterContainer.Find(action.character);
                    
                    var animator = actor.GetComponent<Animator>();
                    if(animator != null && action.animationClip != null)
                    {
                        animator.Play(action.animationClip.name); // Safe : on prend le nom du clip assigné
                    }


                    break;
                }
            case ActionType.Dialogue:
                {
                    Debug.Log($"{action.character}: {action.dialogueText}");
                    // Attend la durée si définie, sinon 1 seconde par défaut
                    float duration = action.duration > 0 ? action.duration : 1f;
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

        yield return null;
    }

    /// <summary>
    /// Affiche un bouton "Suivant" pour passer à la scène suivante
    /// </summary>
    void ShowNextSceneButton(SceneSO nextScene)
    {
        if (choiceButtonPrefab == null || choicePanel == null)
        {
            Debug.LogError("Button Prefab ou Choice Panel manquant pour Suivant !");
            return;
        }

        var buttonGO = Instantiate(choiceButtonPrefab, choicePanel);
        var text = buttonGO.GetComponentInChildren<TMP_Text>();
        text.text = "Suivant";

        var button = buttonGO.GetComponent<Button>();
        button.onClick.AddListener(() => LoadScene(nextScene));
    }
}
