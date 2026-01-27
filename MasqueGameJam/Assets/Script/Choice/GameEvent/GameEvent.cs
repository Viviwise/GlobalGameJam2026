using System.Collections.Generic;
using Script.Choice;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/GameEvent")]
public class GameEvent : ScriptableObject
{
    private readonly List<GameEventListener> listeners = new();

    public void RegisterListener(GameEventListener listener)
    {
        if (listener == null) return;
        if (!listeners.Contains(listener))
            listeners.Add(listener);
    }

    public void UnregisterListener(GameEventListener listener)
    {
        if (listener == null) return;
        listeners.Remove(listener);
    }

    public void Raise()
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            if (listeners[i] != null)
                listeners[i].OnEventRaised();
        }
    }

    private void OnDisable()
    {
        listeners.Clear();
    }
}