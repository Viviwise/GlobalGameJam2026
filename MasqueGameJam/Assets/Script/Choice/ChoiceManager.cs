using Script.Choice;
using UnityEngine;

public class ChoiceManager : MonoBehaviour
{
    public static ChoiceManager Instance;

    private GameObject currentChoicesParent;
    private GameObject currentObject;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void ShowChoices(GameObject obj, GameObject choicePrefab1, Transform spawnPoint)
    {
        // Cacher les choix précédents
        HideCurrentChoices();

        currentObject = obj;

        currentChoicesParent = new GameObject("CurrentChoices");
        choicePrefab1 = Instantiate(choicePrefab1, spawnPoint.position, Quaternion.identity, currentChoicesParent.transform);
    }

    public void HideCurrentChoices()
    {
        if (currentChoicesParent != null)
        {
            Destroy(currentChoicesParent);
            currentChoicesParent = null;
            currentObject = null;
        }
    }

    public void ConfirmChoice()
    {
        HideCurrentChoices();
        InteractionBlocker.BlockAllInteractions = true;
    }
}