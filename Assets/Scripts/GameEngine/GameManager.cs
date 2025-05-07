using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Static fields

    private const int GAME_DURATION_MAX = 120;
    private const int GAIN_SCORE = 1;
    private const int LOSE_SCORE = 10;
    private const int TARGET_DURATION_MAX = 10;
    private const int TRIGGER_DURING_TIMER_MAX = 3;
    private const int RANGE = 10;

    #endregion Static fields

    #region Fields

    private EventManager _events;
    private MandoState _currentMandoState;
    private PlayerManager _playerControler;
    private int _gameDuration;
    private int _targetDuration;
    private int _targetScore;
    private int _score;

    private bool _mando = false;
    private int _triggerBeforeTimer;
    private int _triggerDuringTimer;

    private TMP_Text _gameDurationUiDebug;
    private TMP_Text _scoreUiDebug;
    private TMP_Text _targetScoreUiDebug;
    private TMP_Text _mandoCheckUiDebug;
    private TMP_Text _triggerBeforeTimerUiDebug;
    private TMP_Text _triggerDuringTimerUiDebug;

    #endregion Fields

    #region Public methods

    #endregion Public methods

    #region Private methods

    private void Awake()
    {
        _events = EventManager.GetInstance();
        _playerControler = transform.parent.GetComponentInChildren<PlayerManager>();
        _currentMandoState = MandoState.Idle;

        _gameDurationUiDebug = GameObject.Find("_gameDurationValue").GetComponent<TMP_Text>();
        _scoreUiDebug = GameObject.Find("_scoreValue").GetComponent<TMP_Text>();
        _targetScoreUiDebug = GameObject.Find("_targetScoreValue").GetComponent<TMP_Text>();
        _mandoCheckUiDebug = GameObject.Find("_mandoCheckValue").GetComponent<TMP_Text>();
        _triggerBeforeTimerUiDebug = GameObject.Find("_triggerBeforeTimerValue").GetComponent<TMP_Text>();
        _triggerDuringTimerUiDebug = GameObject.Find("_triggerDuringTimerValue").GetComponent<TMP_Text>();

        InitGameDuration();
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

    private void InitGameDuration()
    {
        _gameDuration = GAME_DURATION_MAX;
    }

    private void InitTarget()
    {
        _targetScore = UnityEngine.Random.Range(5, 95);

        _targetDuration = TARGET_DURATION_MAX;

        _currentMandoState = MandoState.Idle;
    }

    private void InitBeforeMando()
    {
        _mando = true;
        _triggerBeforeTimer = UnityEngine.Random.Range(5, 25);

        _currentMandoState = MandoState.Prepare;
    }

    private void InitDuringMando()
    {
        _mando = false;
        _triggerDuringTimer = TRIGGER_DURING_TIMER_MAX;

        _currentMandoState = MandoState.Check;
    }

    // Call every seconds
    private void UpdateGameTime()
    {
        UpdateScore();
        UpdateMando();
        UpdateTarget();
        UpdateGameDuration();

        _events.Notify(EventType.Mando, (int) _currentMandoState);

        UpdateDebugUi();
    }

    private void UpdateScore()
    {
        if (_mando && InputManager.Action())
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
        if (_mando)
        {
            // Time during _mando check
            _triggerDuringTimer -= 1;
            if (_triggerDuringTimer <= 0)
            {
                InitDuringMando();
            }
        }
        else
        {
            // Time before _mando check
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

            InitGameDuration();
        }
    }
    
    private void UpdateDebugUi()
    {
        if(!_gameDurationUiDebug.IsUnityNull())
        {
            _gameDurationUiDebug.SetText(_gameDuration.ToString());
        }
        else
        {
            Debug.Log($"_gameDuration: {_gameDuration}");
        }

        if(!_scoreUiDebug.IsUnityNull())
        {
            _scoreUiDebug.SetText(_score.ToString());
        }
        else
        {
            Debug.Log($"_score: {_score}");
        }

        if(!_targetScoreUiDebug.IsUnityNull())
        {
            _targetScoreUiDebug.SetText(_targetScore.ToString());
        }
        else
        {
            Debug.Log($"_targetScore: {_targetScore}");
        }

        if(!_mandoCheckUiDebug.IsUnityNull())
        {
            _mandoCheckUiDebug.SetText(_mando.ToString());
        }
        else
        {
            Debug.Log($"_mando check: {_mando}");
        }

        if(!_triggerBeforeTimerUiDebug.IsUnityNull())
        {
            _triggerBeforeTimerUiDebug.SetText(_triggerBeforeTimer.ToString());
        }
        else
        {
            Debug.Log($"_triggerBeforeTimer: {_triggerBeforeTimer}");
        }

        if(!_triggerDuringTimerUiDebug.IsUnityNull())
        {
            _triggerDuringTimerUiDebug.SetText(_triggerDuringTimer.ToString());
        }
        else
        {
            Debug.Log($"_triggerDuringTimer: {_triggerDuringTimer}");
        }
    }

    #endregion Private methods
}
