using System;
using System.Collections.Generic;

public static class EventBus
{
    private static readonly Dictionary<string, Action> Events = new Dictionary<string, Action>();

    public static void Subscribe(string id, Action handler)
    {
        if (string.IsNullOrEmpty(id) || handler == null)
        {
            return;
        }
        if (Events.TryGetValue(id, out var existing))
        {
            Events[id] = existing + handler;
        }
        else
        {
            Events.Add(id, handler);
        }
    }

    public static void Unsubscribe(string id, Action handler)
    {
        if (string.IsNullOrEmpty(id) || handler == null)
        {
            return;
        }
        if (!Events.TryGetValue(id, out var existing))
        {
            return;
        }
        existing -= handler;
        if (existing == null)
        {
            Events.Remove(id);
        }
        else
        {
            Events[id] = existing;
        }
    }

    public static void Publish(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return;
        }
        if (Events.TryGetValue(id, out var handlers))
        {
            handlers?.Invoke();
        }
    }
}
