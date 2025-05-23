using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Static fields

    #endregion Static fields

    #region Fields

    private EventManager _events;
    private DifficultyManager _difficulty;
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
        _difficulty = DifficultyManager.GetInstance();
        _difficulty.SetDifficulty(DifficultyType.Normal);
        _playerControler = transform.parent.GetComponentInChildren<PlayerManager>();
        _currentMandoState = MandoState.Idle;

        GameObject gameDurationUiDebug_GameObject = GameObject.Find("_gameDurationValue");
        if(gameDurationUiDebug_GameObject != null)
        {
            _gameDurationUiDebug = gameDurationUiDebug_GameObject.GetComponent<TMP_Text>();
        }

        GameObject scoreValueUiDebug_GameObject = GameObject.Find("_scoreValue");
        if(scoreValueUiDebug_GameObject != null)
        {
            _scoreUiDebug = scoreValueUiDebug_GameObject.GetComponent<TMP_Text>();
        }

        GameObject targetScoreValue_GameObject = GameObject.Find("_targetScoreValue");
        if(targetScoreValue_GameObject != null)
        {
            _targetScoreUiDebug = targetScoreValue_GameObject.GetComponent<TMP_Text>();
        }

        GameObject mandoCheckValue_GameObject = GameObject.Find("_mandoCheckValue");
        if(mandoCheckValue_GameObject != null)
        {
            _mandoCheckUiDebug = mandoCheckValue_GameObject.GetComponent<TMP_Text>();
        }

        GameObject triggerBeforeTimerValue_GameObject = GameObject.Find("_triggerBeforeTimerValue");
        if(triggerBeforeTimerValue_GameObject != null)
        {
            _triggerBeforeTimerUiDebug = triggerBeforeTimerValue_GameObject.GetComponent<TMP_Text>();
        }

        GameObject triggerDuringTimerValue_GameObject = GameObject.Find("_triggerDuringTimerValue");
        if(triggerDuringTimerValue_GameObject != null)
        {
            _triggerDuringTimerUiDebug = triggerDuringTimerValue_GameObject.GetComponent<TMP_Text>();
        }

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
        _gameDuration = _difficulty.GetDifficultyStats().GAME_DURATION_MAX;
    }

    private void InitTarget()
    {
        _targetScore = UnityEngine.Random.Range(5, 95);

        _targetDuration = _difficulty.GetDifficultyStats().TARGET_DURATION_MAX;

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
        _triggerDuringTimer = _difficulty.GetDifficultyStats().TRIGGER_DURING_TIMER_MAX;

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
            _score = _score - _difficulty.GetDifficultyStats().LOSE_SCORE <= 0 ? 0 : _score - _difficulty.GetDifficultyStats().LOSE_SCORE;

            _currentMandoState = MandoState.Found;
        }
        else if (_playerControler.PowerValue >= _targetScore - _difficulty.GetDifficultyStats().RANGE && _playerControler.PowerValue <= _targetScore + _difficulty.GetDifficultyStats().RANGE)
        {
            _score += _difficulty.GetDifficultyStats().GAIN_SCORE;
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
