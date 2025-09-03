using System.Collections.Generic;
using UnityEngine;

public class MandoUI : MonoBehaviour, IEventListener
{
    private EventManager _events;
    [SerializeField] private List<GameObject> _mandoVisuals;

    private void Awake()
    {
        _events = EventManager.GetInstance();
        setVisual(0);
    }

    private void Start()
    {
        _events.Subscribe(EventEnum.Mando, this);
    }

    void Update()
    {
        
    }

    public void EventUpdate(EventEnum eventEnum, int data)
    {
        if (eventEnum == EventEnum.Mando)
        {
            setVisual(data);
        }
    }

    public void setVisual(int targetMando)
    {
        for(var i = 0; i < _mandoVisuals.Count; i++)
        {
            bool current = false;

            if(targetMando == i || (targetMando == 1 && (i == 0 || i == 1)))
            {
                current = true;
            }

            _mandoVisuals[i].SetActive(current);
        }
    }
}
