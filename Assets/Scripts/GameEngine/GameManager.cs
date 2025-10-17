using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Public fields

    public TMP_Text TextVague;

    [Header("Menus containers")]
    public GameObject ContainerMenuPause;
    public GameObject ContainerMenuEnd;

    [Header("Debug container")]
    public GameObject ContainerDebugUi;

    #endregion Public fields

    #region Fields

    private EventManager _events;
    private DifficultyManager _difficulty;
    private GameEnum _currentGameEnum;
    private MandoEnum _currentMandoEnum;
    private PlayerManager _playerControler;
    private int _gameDuration;
    private int _targetDuration;
    private int _targetScore;
    private int _score;
    private int _triggerBeforeTimer;
    private int _triggerDuringTimer;

    private TMP_Text _gameDurationUiDebug;
    private TMP_Text _scoreUiDebug;
    private TMP_Text _targetScoreUiDebug;
    private TMP_Text _triggerBeforeTimerUiDebug;
    private TMP_Text _triggerDuringTimerUiDebug;
    private TMP_Text _currentMandoStateUiDebug;

    #endregion Fields

    #region Public methods

    /// <summary>
    /// Start is called at the creation of GameManager
    /// </summary>
    public void StartGame()
    {
        ContainerMenuPause.SetActive(false);
        ContainerMenuEnd.SetActive(false);

        InitGameDuration();
        InitTarget();
        InitBeforeMando();
        InitDuringMando();

        InputManager.instance.PauseAction.performed += TogglePause;

        _currentMandoEnum = MandoEnum.Idle;
        UpdateGameEvent(GameEnum.Start);

        // Starting in 0 seconds, a call will be do every 1 seconds
        InvokeRepeating("UpdateGameTime", 0, 1);
    }

    /// <summary>
    /// Stop is called once the timer is reached
    /// </summary>
    public void StopGame(bool isEndGame)
    {
        UpdateGameEvent(GameEnum.Stop);

        CancelInvoke("UpdateGameTime");
        if (isEndGame)
        {
            TextVague.SetText($"{_score}");
            ContainerMenuEnd.SetActive(true);
        }
    }

    /// <summary>
    /// Stop is called once the timer is reached
    /// </summary>
    public void RestartGame()
    {
        StopGame(false);
        StartGame();
    }

    /// <summary>
    /// Can be called from a UI Button OnClick event to toggle pause/resume.
    /// </summary>
    public void TogglePauseFromUI()
    {
        OnPause();
    }

    /// <summary>
    /// 
    /// </summary>
    public void LoadMainMenu()
    {
        StopGame(false);
        _events.UnSubscribeAll();
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// 
    /// </summary>
    public void QuitGame()
    {
        PlayerPrefs.Save();
        StopAllCoroutines();
        Application.Quit();
    }

    #endregion Public methods

    #region Private methods

    private void Awake()
    {
        ContainerDebugUi.SetActive(Debug.isDebugBuild);

        _events = EventManager.GetInstance();
        _difficulty = DifficultyManager.GetInstance();
        _playerControler = transform.parent.GetComponentInChildren<PlayerManager>();
        UpdateGameEvent(GameEnum.Idle);
        _currentMandoEnum = MandoEnum.Idle;

        int newDifficulty = PlayerPrefs.GetInt("DifficultyLevel", (int)DifficultyEnum.Easy);
        _difficulty.SetDifficulty(DifficultyEnum.IsDefined(typeof(DifficultyEnum), newDifficulty) ? (DifficultyEnum)newDifficulty : DifficultyEnum.Easy);

        GameObject gameDurationUiDebug_GameObject = GameObject.Find("_gameDurationValue");
        if (gameDurationUiDebug_GameObject != null)
        {
            _gameDurationUiDebug = gameDurationUiDebug_GameObject.GetComponent<TMP_Text>();
        }

        GameObject scoreValueUiDebug_GameObject = GameObject.Find("_scoreValue");
        if (scoreValueUiDebug_GameObject != null)
        {
            _scoreUiDebug = scoreValueUiDebug_GameObject.GetComponent<TMP_Text>();
        }

        GameObject targetScoreValue_GameObject = GameObject.Find("_targetScoreValue");
        if (targetScoreValue_GameObject != null)
        {
            _targetScoreUiDebug = targetScoreValue_GameObject.GetComponent<TMP_Text>();
        }

        GameObject triggerBeforeTimerValue_GameObject = GameObject.Find("_triggerBeforeTimerValue");
        if (triggerBeforeTimerValue_GameObject != null)
        {
            _triggerBeforeTimerUiDebug = triggerBeforeTimerValue_GameObject.GetComponent<TMP_Text>();
        }

        GameObject triggerDuringTimerValue_GameObject = GameObject.Find("_triggerDuringTimerValue");
        if (triggerDuringTimerValue_GameObject != null)
        {
            _triggerDuringTimerUiDebug = triggerDuringTimerValue_GameObject.GetComponent<TMP_Text>();
        }

        GameObject currentMandoStateValue_GameObject = GameObject.Find("_currentMandoStateValue");
        if (currentMandoStateValue_GameObject != null)
        {
            _currentMandoStateUiDebug = currentMandoStateValue_GameObject.GetComponent<TMP_Text>();
        }

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        StartGame();
    }

    private void OnDestroy()
    {
        InputManager.instance.PauseAction.performed -= TogglePause;
    }

    // Update is called once per frame
    private void Update()
    {

    }

    private void InitGameDuration()
    {
        _gameDuration = _difficulty.GAME_DURATION_MAX;
    }

    private void InitTarget()
    {
        _targetScore = UnityEngine.Random.Range(5, 95);

        _targetDuration = _difficulty.TARGET_DURATION_MAX;
        UpdatePowerTargetEvent();
    }

    private void InitBeforeMando()
    {
        _triggerBeforeTimer = UnityEngine.Random.Range(_difficulty.TRIGGER_BEFORE_TIMER_MIN, _difficulty.TRIGGER_BEFORE_TIMER_MAX);

        _currentMandoEnum = MandoEnum.Check;
    }

    private void InitDuringMando()
    {
        _triggerDuringTimer = _difficulty.TRIGGER_DURING_TIMER_MAX;

        _currentMandoEnum = MandoEnum.Idle;
    }

    /// <summary>
    /// Called by the Input System when the PauseAction is performed.
    /// </summary>
    /// <param name="context">The input callback context.</param>
    private void TogglePause(InputAction.CallbackContext context)
    {
        OnPause();
    }

    /// <summary>
    /// Handles the pause and resume logic when the PauseAction input is triggered.
    /// If the game is not currently paused, it switches to pause state,
    /// cancels the repeating game time updates, and shows the pause menu.
    /// If the game is already paused, it resumes the game,
    /// hides the pause menu, and restarts the repeating game time updates.
    /// </summary>
    /// <param name="context">The input callback context provided by the Input System when the action is triggered.</param>
    private void OnPause()
    {
        if (_currentGameEnum != GameEnum.Pause)
        {
            UpdateGameEvent(GameEnum.Pause);
            CancelInvoke("UpdateGameTime");
            ContainerMenuPause.SetActive(true);
        }
        else
        {
            UpdateGameEvent(GameEnum.Resume);
            ContainerMenuPause.SetActive(false);
            InvokeRepeating("UpdateGameTime", 0, 1);
        }
    }

    // Call every seconds
    private void UpdateGameTime()
    {
        UpdateMando();
        UpdateScore();

        UpdateTarget();
        UpdateGameDuration();

        UpdateMandoEvent();

        UpdateDebugUi();
    }

    private void UpdateMandoEvent()
    {
        _events.Notify(EventEnum.Mando, (int) _currentMandoEnum);
    }

    private void UpdateGameEvent(GameEnum newCurrentGame)
    {
        _currentGameEnum = newCurrentGame;
        _events.Notify(EventEnum.Game, (int) _currentGameEnum);
    }

    private void UpdatePowerTargetEvent()
    {
        _events.Notify(EventEnum.PowerTarget, (int) _targetScore);
    }

    private void UpdateScore()
    {
        if ((_currentMandoEnum == MandoEnum.Check || _currentMandoEnum == MandoEnum.Found) && InputManager.Action())
        {
            _score = _score - _difficulty.LOSE_SCORE <= 0 ? 0 : _score - _difficulty.LOSE_SCORE;

            _currentMandoEnum = MandoEnum.Found;
        }
        else if (_playerControler.PowerValue >= _targetScore - _difficulty.RANGE && _playerControler.PowerValue <= _targetScore + _difficulty.RANGE)
        {
            _score += _difficulty.GAIN_SCORE;
        }
    }

    private void UpdateMando()
    {
        switch(_currentMandoEnum)
        {
            case MandoEnum.Idle:
            case MandoEnum.Prepare:
                // Time before _mando check
                _triggerBeforeTimer -= 1;
                if(_triggerBeforeTimer <= 0)
                {
                    InitBeforeMando();
                }
                else if(_triggerBeforeTimer <= _difficulty.TRIGGER_PREPARE_TIMER)
                {
                    _currentMandoEnum = MandoEnum.Prepare;
                }
                break;
            case MandoEnum.Check:
            case MandoEnum.Found:
                // Time during _mando check
                _triggerDuringTimer -= 1;
                if (_triggerDuringTimer <= 0)
                {
                    InitDuringMando();
                }
                break;
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
            StopGame(true);
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

        if(!_currentMandoStateUiDebug.IsUnityNull())
        {
            _currentMandoStateUiDebug.SetText(_currentMandoEnum.ToString());
        }
        else
        {
            Debug.Log($"_currentMandoStateValue: {_currentMandoEnum}");
        }
    }

    #endregion Private methods
}
