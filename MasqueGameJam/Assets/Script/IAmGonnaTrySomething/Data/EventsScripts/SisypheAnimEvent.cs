using UnityEngine;

    [CreateAssetMenu(fileName = "MissSisypheEvent", menuName = "Test/Events/SisypheAnimEvent")]

public class SisypheAnimEvent : EventForScene
{
    public ObjectsID target;
    public string animatorStateName;
}
