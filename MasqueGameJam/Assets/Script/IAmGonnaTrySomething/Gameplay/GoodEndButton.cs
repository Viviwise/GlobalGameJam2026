using UnityEngine;
using UnityEngine.UI;

namespace Script.IAmGonnaTrySomething.Gameplay
{
    public class GoodEndButton : MonoBehaviour
    {
        [SerializeField] private ScenarioManager scenarioManager;
        [SerializeField] private GameObject goodEndButton;

        private void Awake()
        {
            goodEndButton.SetActive(false);
        }

        private void OnEnable()
        {
            if (scenarioManager != null)
                scenarioManager.OnGoodEndReached += ShowButton;
        }

        private void OnDisable()
        {
            if (scenarioManager != null)
                scenarioManager.OnGoodEndReached -= ShowButton;
        }

        private void ShowButton()
        {
            goodEndButton.SetActive(true);
        }

        public void HideButton()
        {
            goodEndButton.SetActive(false);
        }
    }
}