using UnityEngine;

[CreateAssetMenu(fileName = "ShowFrameEvent", menuName = "Test/Events/ShowFrame")]
public class ShowFrameEvent : EventForScene
{
    private void OnEnable()
    {
        eventType = ScenarioEventTypes.FrameMove;
    }
}