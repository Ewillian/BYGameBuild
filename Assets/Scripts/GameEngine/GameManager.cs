using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Static fields

    private const int TRIGGER_DURING_TIMER_MAX = 3;
    private const int TARGET_DURATION_MAX = 3;
    private const int GAIN_SCORE = 1;
    private const int LOSE_SCORE = 10;
    private const int RANGE = 5;

    #endregion Static fields

    #region Fields

    private EventManager _events;

    private PlayerManager _playerControler;

    private MandoState _currentMandoState;

    private int _gameDuration = 120;
    private int _targetDuration = 3;
    private int _targetScore = 0;
    private int _score = 0;

    private bool mando = false;
    private int _triggerBeforeTimer = 0;
    private int _triggerDuringTimer = 3;

    #endregion Fields

    #region Public methods

    #endregion Public methods

    #region Private methods

    private void Awake()
    {
        _events = EventManager.GetInstance();
        _playerControler = transform.parent.GetComponentInChildren<PlayerManager>();
        _currentMandoState = MandoState.Idle;

        InitTarget();
        InitBeforeMando();
        InitDuringMando();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        // Starting in 0 seconds, a projectile will be launched every 1 seconds
        InvokeRepeating("UpdateGameTime", 0, 1);
        // CancelInvoke("UpdateGameTime");
    }

    // Update is called once per frame
    private void Update()
    {

    }

    private void InitBeforeMando()
    {
        mando = true;
        _triggerBeforeTimer = Random.Range(5, 25);

        _currentMandoState = MandoState.Prepare;
    }

    private void InitDuringMando()
    {
        mando = false;
        _triggerDuringTimer = TRIGGER_DURING_TIMER_MAX;

        _currentMandoState = MandoState.Check;
    }

    private void InitTarget()
    {
        _targetScore = Random.Range(5, 95);

        _targetDuration = TARGET_DURATION_MAX;

        _currentMandoState = MandoState.Idle;
    }

    // Call every seconds
    private void UpdateGameTime()
    {
        UpdateScore();
        UpdateMando();
        UpdateTarget();
        UpdateGameDuration();

        _events.Notify(EventType.Mando, (int) _currentMandoState);

        Debug.Log($"_targetScore: {_targetScore}");
        Debug.Log($"_score: {_score}");
        Debug.Log($"_gameDuration: {_gameDuration}");

        Debug.Log($"mando check: {mando}");
        Debug.Log($"_triggerBeforeTimer: {_triggerBeforeTimer}");
        Debug.Log($"_triggerDuringTimer: {_triggerDuringTimer}");
    }

    private void UpdateScore()
    {
        if (mando && InputManager.Action())
        {
            _score = _score - LOSE_SCORE <= 0 ? 0 : _score - LOSE_SCORE;

            _currentMandoState = MandoState.Found;
        }
        else if (_playerControler.PowerValue >= _targetScore - RANGE && _playerControler.PowerValue <= _targetScore + RANGE)
        {
            _score += GAIN_SCORE;
        }
    }

    private void UpdateMando()
    {
        if (mando)
        {
            // Time during mando check
            _triggerDuringTimer -= 1;
            if (_triggerDuringTimer <= 0)
            {
                InitDuringMando();
            }
        }
        else
        {
            // Time before mando check
            _triggerBeforeTimer -= 1;
            if(_triggerBeforeTimer <= 0)
            {
                InitBeforeMando();
            }
        }
    }

    private void UpdateTarget()
    {
        _targetDuration -= 1;
        if (_targetDuration <= 0)
        {
            InitTarget();
        }
    }

    private void UpdateGameDuration()
    {
        _gameDuration -= 1;
        if (_gameDuration == 0)
        {
            CancelInvoke("UpdateGameTime");

            _gameDuration = 10;
        }
    }

    #endregion Private methods
}
