using UnityEngine;
[CreateAssetMenu(fileName = "CurtainEvent", menuName = "Test/Events/OpenCurtainEvent")]
public class OpenCurtainEvent : EventForScene
{
    private void OnEnable()
    {
        eventType = ScenarioEventTypes.OpenCurtain;
    }
    
}
