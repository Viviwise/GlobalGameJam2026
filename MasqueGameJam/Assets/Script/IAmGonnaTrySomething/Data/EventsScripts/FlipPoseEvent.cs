using UnityEngine;

namespace Script.IAmGonnaTrySomething.Data.EventsScripts
{
    [CreateAssetMenu(fileName = "FlipPoseEvent", menuName = "Test/Events/FlipPoseEvent")]
    public class FlipPoseEvent : EventForScene
    {
        public ObjectsID target;
    }
}