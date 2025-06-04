using System.Collections.Generic;

public class DifficultyManager
{
    private class DifficultyStats
    {
        /// <summary>
        /// Default value statistics depend difficulty
        /// </summary>
        /// <param name="pGAME_DURATION_MAX">Game duration (in second)</param>
        /// <param name="pGAIN_SCORE">Number of points earned for every second in the target</param>
        /// <param name="pLOSE_SCORE">Number of points lost every second in the target</param>
        /// <param name="pTARGET_DURATION_MAX">Maximum time during which the target remains (in second)</param>
        /// <param name="pTRIGGER_BEFORE_TIMER_MIN">Minimum time before Mando remains active (in second)</param>
        /// <param name="pTRIGGER_BEFORE_TIMER_MAX">Maximum time before Mando remains active (in second)</param>
        /// <param name="pTRIGGER_DURING_TIMER_MAX">Maximum time where Mando remains active (in second)</param>
        /// <param name="pRANGE">Range in which the player wins a point (in percent) x2</param>
        public DifficultyStats(int pGAME_DURATION_MAX, int pGAIN_SCORE, int pLOSE_SCORE, int pTARGET_DURATION_MAX, int pTRIGGER_BEFORE_TIMER_MIN, int pTRIGGER_BEFORE_TIMER_MAX, int pTRIGGER_DURING_TIMER_MAX, int pRANGE)
        {
            GAME_DURATION_MAX = pGAME_DURATION_MAX;
            GAIN_SCORE = pGAIN_SCORE;
            LOSE_SCORE = pLOSE_SCORE;
            TARGET_DURATION_MAX = pTARGET_DURATION_MAX;
            TRIGGER_BEFORE_TIMER_MIN = pTRIGGER_BEFORE_TIMER_MIN;
            TRIGGER_BEFORE_TIMER_MAX = pTRIGGER_BEFORE_TIMER_MAX;
            TRIGGER_DURING_TIMER_MAX = pTRIGGER_DURING_TIMER_MAX;
            RANGE = pRANGE;
        }

        public int GAME_DURATION_MAX { get; private set; }
        public int GAIN_SCORE { get; private set; }
        public int LOSE_SCORE { get; private set; }
        public int TARGET_DURATION_MAX { get; private set; }
        public int TRIGGER_BEFORE_TIMER_MIN { get; private set; }
        public int TRIGGER_BEFORE_TIMER_MAX { get; private set; }
        public int TRIGGER_DURING_TIMER_MAX { get; private set; }
        public int RANGE { get; private set; }
    }

    #region Static fields

    private static DifficultyManager _instance;

    #endregion Static Fields

    #region Private fields

    private Dictionary<DifficultyEnum, DifficultyStats> _difficultyDictionary;

    private DifficultyEnum _currentDifficulty = DifficultyEnum.Easy;

    public int GAME_DURATION_MAX
    {
        get { return _difficultyDictionary[_currentDifficulty].GAME_DURATION_MAX;}
        private set {}
    }

    public int GAIN_SCORE
    {
        get { return _difficultyDictionary[_currentDifficulty].GAIN_SCORE;}
        private set {}
    }

    public int LOSE_SCORE
    {
        get { return _difficultyDictionary[_currentDifficulty].LOSE_SCORE;}
        private set {}
    }

    public int TARGET_DURATION_MAX
    {
        get { return _difficultyDictionary[_currentDifficulty].TARGET_DURATION_MAX;}
        private set {}
    }

    public int TRIGGER_BEFORE_TIMER_MIN
    {
        get { return _difficultyDictionary[_currentDifficulty].TRIGGER_BEFORE_TIMER_MIN;}
        private set {}
    }

    public int TRIGGER_BEFORE_TIMER_MAX
    {
        get { return _difficultyDictionary[_currentDifficulty].TRIGGER_BEFORE_TIMER_MAX;}
        private set {}
    }

    public int TRIGGER_DURING_TIMER_MAX
    {
        get { return _difficultyDictionary[_currentDifficulty].TRIGGER_DURING_TIMER_MAX;}
        private set {}
    }

    public int RANGE
    {
        get { return _difficultyDictionary[_currentDifficulty].RANGE;}
        private set {}
    }

    #endregion Private fields

    #region Private methods

    private DifficultyManager()
    {
        _difficultyDictionary = new Dictionary<DifficultyEnum, DifficultyStats>();

        _difficultyDictionary.Add(DifficultyEnum.Easy, new DifficultyStats(120, 5, 2, 10, 5, 25, 3, 13));
        _difficultyDictionary.Add(DifficultyEnum.Normal, new DifficultyStats(90, 4, 4, 12, 3, 25, 3, 10));
        _difficultyDictionary.Add(DifficultyEnum.Hard, new DifficultyStats(60, 3, 5, 15, 2, 20, 4, 7));
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

    public DifficultyEnum GetDifficulty()
    {
        return _currentDifficulty;
    }

    public void SetDifficulty(DifficultyEnum newDifficulty)
    {
        if (_currentDifficulty == newDifficulty)
        {
            return;
        }

        _currentDifficulty = newDifficulty;
    }

    #endregion Public methods
}
