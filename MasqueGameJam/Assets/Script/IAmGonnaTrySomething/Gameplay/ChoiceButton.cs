using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class ChoiceButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textBox;
    [SerializeField] public Button choiceButton;

    public void SetUp(ScenarioManager manager, SequenceForScene sequence)
    {
        choiceButton.onClick.RemoveAllListeners();
        textBox.text = sequence.sequenceName;
        choiceButton.onClick.AddListener(() => manager.CallSequence(sequence));
    }
}
