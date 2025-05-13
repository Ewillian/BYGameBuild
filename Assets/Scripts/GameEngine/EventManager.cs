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

    public void Subscribe(EventType eventType, IEventListener listener)
    {
        if (!_listeners.ContainsKey(eventType))
        {
            _listeners[eventType] = new List<IEventListener>();
        }

        _listeners[eventType].Add(listener);
    }

    public void UnSubscribe(EventType eventType, IEventListener listener)
    {
        if (_listeners.ContainsKey(eventType))
        {
            _listeners[eventType].Remove(listener);
        }
    }

    public void Notify(EventType eventType, int data)
    {
        if(!_listeners.ContainsKey(eventType) || _listeners[eventType].Count <= 0)
        {
            return;
        }

        foreach (var listener in _listeners[eventType])
        {
            listener.EventUpdate(data);
        }
    }
}
