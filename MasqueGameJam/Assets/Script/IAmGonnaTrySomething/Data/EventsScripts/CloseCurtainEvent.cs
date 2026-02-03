using UnityEngine;
[CreateAssetMenu(fileName = "CurtainEvent", menuName = "Test/Events/CloseCurtainEvent")]
public class CLoseCurtainEvent : EventForScene
{
    private void OnEnable()
    {
        eventType = ScenarioEventTypes.CloseCurtain;
    }
    
}