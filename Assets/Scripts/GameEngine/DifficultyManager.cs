using System.Collections.Generic;

public class DifficultyManager
{
    public class DifficultyStats
    {
        /// <summary>
        /// Default value statistics depend difficulty
        /// </summary>
        /// <param name="pGAME_DURATION_MAX"></param>
        /// <param name="pGAIN_SCORE"></param>
        /// <param name="pLOSE_SCORE"></param>
        /// <param name="pTARGET_DURATION_MAX"></param>
        /// <param name="pTRIGGER_DURING_TIMER_MAX"></param>
        /// <param name="pRANGE"></param>
        public DifficultyStats(int pGAME_DURATION_MAX, int pGAIN_SCORE, int pLOSE_SCORE, int pTARGET_DURATION_MAX, int pTRIGGER_DURING_TIMER_MAX, int pRANGE)
        {
            GAME_DURATION_MAX = pGAME_DURATION_MAX;
            GAIN_SCORE = pGAIN_SCORE;
            LOSE_SCORE = pLOSE_SCORE;
            TARGET_DURATION_MAX = pTARGET_DURATION_MAX;
            TRIGGER_DURING_TIMER_MAX = pTRIGGER_DURING_TIMER_MAX;
            RANGE = pRANGE;
        }

        public int GAME_DURATION_MAX { get; private set; }
        public int GAIN_SCORE { get; private set; }
        public int LOSE_SCORE { get; private set; }
        public int TARGET_DURATION_MAX { get; private set; }
        public int TRIGGER_DURING_TIMER_MAX { get; private set; }
        public int RANGE { get; private set; }
    }

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

        _difficultyDictionary.Add(DifficultyType.Easy, new DifficultyStats(120, 5, 2, 10, 3, 13));
        _difficultyDictionary.Add(DifficultyType.Normal, new DifficultyStats(90, 4, 4, 12, 3, 10));
        _difficultyDictionary.Add(DifficultyType.Hard, new DifficultyStats(60, 3, 5, 15, 4, 7));
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
