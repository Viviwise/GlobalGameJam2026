using System;
using UnityEngine;

namespace Script.IAmGonnaTrySomething.Data.EventsScripts
{
    [CreateAssetMenu(fileName = "LightEvent", menuName = "Test/Events/LightEvent")]
    public class LightEvent : EventForScene
    {
        public ObjectsID target;
        public Color lightColor;
        public bool general;
    }
}