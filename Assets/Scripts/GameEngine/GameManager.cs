using UnityEngine;
using UnityEngine.UI;

public class GameControler : MonoBehaviour
{
    #region Fields

    private PlayerControler _playerControler;

    private int _gameDuration = 10;
    private int _targetDuration = 3;
    private int _targetScore = 0;
    private int _points = 0;
    private int _range = 5;

    #endregion Fields

    #region Public methods

    #endregion Public methods

    #region Private methods

    private void Awake()
    {
        _playerControler = transform.parent.GetComponentInChildren<PlayerControler>();

        _targetScore = Random.Range(5, 95);
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

    private void UpdateGameTime()
    {
        if(_playerControler.PowerValue >= _targetScore - _range && _playerControler.PowerValue <= _targetScore + _range)
        {
            _points += 1;
        }

        _targetDuration -= 1;
        if(_targetDuration == 0)
        {
            _targetScore = Random.Range(5, 95);

            _targetDuration = 3;
        }

        _gameDuration -= 1;
        if(_gameDuration == 0)
        {
            CancelInvoke("UpdateGameTime");

            _gameDuration = 10;
        }

        Debug.Log($"_targetScore: {_targetScore}");
        Debug.Log($"_points: {_points}");
        Debug.Log($"_gameDuration: {_gameDuration}");
    }
    
    #endregion Private methods
}
