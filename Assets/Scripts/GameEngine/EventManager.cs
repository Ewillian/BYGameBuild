using System.Collections.Generic;

public class EventManager
{
    private static EventManager _instance;

    private Dictionary<EventEnum, List<IEventListener>> _listeners;

    private EventManager() 
    {
        _listeners = new Dictionary<EventEnum, List<IEventListener>>();
    }

    public static EventManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = new EventManager();
        }

        return _instance;
    }

    public void Subscribe(EventEnum eventEnum, IEventListener listener)
    {
        if (!_listeners.ContainsKey(eventEnum))
        {
            _listeners[eventEnum] = new List<IEventListener>();
        }

        _listeners[eventEnum].Add(listener);
    }

    public void UnSubscribe(EventEnum eventEnum, IEventListener listener)
    {
        if (_listeners.ContainsKey(eventEnum))
        {
            _listeners[eventEnum].Remove(listener);
        }
    }

    public void Notify(EventEnum eventEnum, int data)
    {
        if(!_listeners.ContainsKey(eventEnum) || _listeners[eventEnum].Count <= 0)
        {
            return;
        }

        foreach (var listener in _listeners[eventEnum])
        {
            listener.EventUpdate(data);
        }
    }
}
