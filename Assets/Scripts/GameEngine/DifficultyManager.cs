using System.Collections.Generic;
using System.Data;

public enum DifficultyType
{
    Easy,
    Normal,
    Hard
}

public class DifficultyStats
{
    public int GAME_DURATION_MAX;
    public int GAIN_SCORE;
    public int LOSE_SCORE;
    public int TARGET_DURATION_MAX;
    public int TRIGGER_DURING_TIMER_MAX;
    public int RANGE;
}

public class DifficultyManager
{
    #region Static fields

    private static DifficultyManager _instance;

    #endregion Static Fields


    #region Private fields

    private Dictionary<DifficultyType, DifficultyStats> _difficultyDictionary;

    private DifficultyType _currentDifficulty = DifficultyType.Easy;

    #endregion Private fields

    #region Private methods

    private DifficultyManager()
    {
        _difficultyDictionary = new Dictionary<DifficultyType, DifficultyStats>();

        DifficultyStats easyDifficulty = new DifficultyStats();
        easyDifficulty.GAME_DURATION_MAX = 120;
        easyDifficulty.GAIN_SCORE = 5;
        easyDifficulty.LOSE_SCORE = 2;
        easyDifficulty.TARGET_DURATION_MAX = 10;
        easyDifficulty.TRIGGER_DURING_TIMER_MAX = 3;
        easyDifficulty.RANGE = 13;
        _difficultyDictionary.Add(DifficultyType.Easy, easyDifficulty);

        DifficultyStats normalDifficulty = new DifficultyStats();
        normalDifficulty.GAME_DURATION_MAX = 90;
        normalDifficulty.GAIN_SCORE = 4;
        normalDifficulty.LOSE_SCORE = 4;
        normalDifficulty.TARGET_DURATION_MAX = 12;
        normalDifficulty.TRIGGER_DURING_TIMER_MAX = 3;
        normalDifficulty.RANGE = 10;
        _difficultyDictionary.Add(DifficultyType.Normal, normalDifficulty);

        DifficultyStats hardDifficulty = new DifficultyStats();
        hardDifficulty.GAME_DURATION_MAX = 60;
        hardDifficulty.GAIN_SCORE = 3;
        hardDifficulty.LOSE_SCORE = 5;
        hardDifficulty.TARGET_DURATION_MAX = 15;
        hardDifficulty.TRIGGER_DURING_TIMER_MAX = 4;
        hardDifficulty.RANGE = 7;
        _difficultyDictionary.Add(DifficultyType.Hard, hardDifficulty);
    }

    #endregion Private methods

    #region Public static methods

    public static DifficultyManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = new DifficultyManager();
        }

        return _instance;
    }

    #endregion Private static methods

    #region Public methods

    public DifficultyType GetDifficulty()
    {
        return _currentDifficulty;
    }

    public void SetDifficulty(DifficultyType newDifficulty)
    {
        if (_currentDifficulty == newDifficulty)
        {
            return;
        }

        _currentDifficulty = newDifficulty;
    }

    public DifficultyStats GetDifficultyStats()
    {
        return _difficultyDictionary[_currentDifficulty];
    }

    #endregion Public methods
}