using System.Collections.Generic;

public class EventManager
{
    private static EventManager _instance;

    private Dictionary<EventType, List<IEventListener>> _listeners;

    private EventManager() 
    {
        _listeners = new Dictionary<EventType, List<IEventListener>>();
    }

    public static EventManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = new EventManager();
        }
        return _instance;
    }

    private void Subscribe(EventType eventType, IEventListener listener)
    {
        if (!_listeners.ContainsKey(eventType))
        {
            _listeners[eventType] = new List<IEventListener>();
        }

        _listeners[eventType].Add(listener);
    }

    void UnSubscribe(EventType eventType, IEventListener listener)
    {
        if (_listeners.ContainsKey(eventType))
        {
            _listeners[eventType].Remove(listener);
        }
    }

    void Notify(EventType eventType, MandoState state)
    {
        foreach (var listener in _listeners[eventType])
        {
            listener.Update(state);
        }
    }
}
