using System;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

public class ChoiceButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textBox;
    [SerializeField] private Button choiceButton;
    [SerializeField] private Image iconImage;
    [SerializeField] private GameObject goodEndButton;


    private void OnEnable()
    {
        choiceButton = gameObject.GetComponent<Button>();
    }

    public void SetUp(ScenarioManager manager, SequenceForScene sequence)
    {
        choiceButton.onClick.RemoveAllListeners();
        textBox.text = sequence.sequenceName;
        choiceButton.onClick.AddListener(() => manager.CallSequence(sequence));
        iconImage.sprite = sequence.giver;
    }
}
