using TMPro;
using UnityEngine;

namespace Script.IAmGonnaTrySomething.Gameplay
{
    public class DialoguePanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textArea, nameArea;
        
        public void SetUp(string dialogue, string speaker)
        {
            textArea.text = dialogue;
            nameArea.text = speaker;
        }
    }
}