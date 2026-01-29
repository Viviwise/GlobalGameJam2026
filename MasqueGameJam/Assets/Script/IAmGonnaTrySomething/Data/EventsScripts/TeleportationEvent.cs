using UnityEngine;
[CreateAssetMenu(fileName = "TeleportationEvent", menuName = "Test/Events/TelportationEvent")]
public class TeleportationEvent : EventForScene
{
    public ObjectsID target;
    public Vector3 position;
}
