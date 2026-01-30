using UnityEngine;
using UnityEngine.Serialization;

namespace Script.IAmGonnaTrySomething.Data.EventsScripts
{
    [CreateAssetMenu(fileName = "DisappearEvent", menuName = "Test/Events/DisappearEvent")]
    public class DisappearEvent : EventForScene
    {
        public ObjectsID target;
    }
}