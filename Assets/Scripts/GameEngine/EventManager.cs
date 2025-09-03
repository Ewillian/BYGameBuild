using System.Collections.Generic;
using System.Diagnostics;

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
        if (!_listeners.ContainsKey(eventEnum) || _listeners[eventEnum].Count <= 0 && !ValidEnumValue(eventEnum, data))
        {
            return;
        }

        foreach (var listener in _listeners[eventEnum])
        {
            listener.EventUpdate(eventEnum, data);
        }
    }

    public bool ValidEnumValue(EventEnum eventEnum, int currentData)
    {
        switch (eventEnum)
        {
            case EventEnum.Game:
                return GameEnum.IsDefined(typeof(GameEnum), (GameEnum) currentData);
            case EventEnum.Mando:
                return MandoEnum.IsDefined(typeof(MandoEnum), (MandoEnum) currentData);
        }

        return false;
    }
}
