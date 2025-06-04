using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour, IEventListener
{
    #region Fields

    private EventManager _events;

    private static float POWER_VALUE_GAIN = .1f;
    private static float POWER_VALUE_LOSE = .15f;

    [SerializeField]
    private Slider _slider;

    public float PowerValue
    {
        get
        {
            return this._slider.value;
        }
        set
        {
            this._slider.value = value;
        }
    }

    private GameEnum _currentGameEnum;

    #endregion Fields

    #region Public methods

    #endregion Public methods

    #region Private methods

    private void Awake()
    {
        _events = EventManager.GetInstance();
        _slider = GetComponent<Slider>();
        _currentGameEnum = GameEnum.Idle;

        PowerValue = 0;
    }

    private void Start()
    {
        _events.Subscribe(EventEnum.Game, this);

        // Starting in 0 seconds, a projectile will be launched every 0.1 seconds
        InvokeRepeating("UpdatePowerValue", 0, .01f);
        // CancelInvoke("UpdatePowerValue");
    }

    private void Update()
    {

    }

    public void EventUpdate(EventEnum eventEnum, int data)
    {
        if (eventEnum == EventEnum.Game)
        {
            _currentGameEnum = (GameEnum)data;
            if (GameEnum.Start == _currentGameEnum)
            {
                PowerValue = 0;
            }
        }
    }

    private void UpdatePowerValue()
    {
        if (_currentGameEnum != GameEnum.Start)
        {
            return;
        }

        if (InputManager.Action())
        {
            PowerValue = PowerValue + POWER_VALUE_GAIN < _slider.maxValue ? PowerValue + POWER_VALUE_GAIN : _slider.maxValue;
        }
        else
        {
            PowerValue = PowerValue - POWER_VALUE_LOSE > _slider.minValue ? PowerValue - POWER_VALUE_LOSE : _slider.minValue;
        }
    }

    #endregion Private methods
}
