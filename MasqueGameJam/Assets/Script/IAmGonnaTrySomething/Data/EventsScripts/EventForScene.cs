using UnityEngine;
public class EventForScene : ScriptableObject
{
    public ScenarioEventTypes eventType;
}

public enum ScenarioEventTypes
{
    Movement,
    Teleportation,
    Dialogue,
    Sound,
    ChangePose,
    Delay,
    
}
