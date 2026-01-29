using UnityEngine;
[CreateAssetMenu(fileName = "MovementEvent", menuName = "Test/Events/MovementEvent")]
public class MovementEvent : EventForScene
{
    public ObjectsID target;
    public float speed;
    public Vector3 position;
}
