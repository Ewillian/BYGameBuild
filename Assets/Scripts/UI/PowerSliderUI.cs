using UnityEngine;
using UnityEngine.UI;

public class PowerSliderUI : MonoBehaviour, IEventListener
{
    #region Fields

    private EventManager _events;
    private Slider _slider;
    private Transform _target;

    private Vector3 _topWorld;
    private Vector3 _bottomWorld;
    private float _onePercent;

    #endregion Fields

    private void Awake()
    {
        _events = EventManager.GetInstance();
        _slider = GetComponent<Slider>();
        _target = _slider.transform.Find("Target");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _events.Subscribe(EventEnum.PowerTarget, this);

        Vector3[] corners = new Vector3[4];
        _slider.transform.Find("Background").GetComponent<Image>().rectTransform.GetWorldCorners(corners); // 0: BL, 1: TL, 2: TR, 3: BR

        _topWorld = (corners[1] + corners[2]) * 0.5f; // milieu du bord supérieur
        _bottomWorld = (corners[0] + corners[3]) * 0.5f; // milieu du bord inférieur

        _onePercent = (1 * (_topWorld.y - _bottomWorld.y)) / 100;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EventUpdate(EventEnum eventEnum, float data)
    {
        switch (eventEnum)
        {
            case EventEnum.PowerTarget:
                float res = _bottomWorld.y + _onePercent * data;
                _target.transform.SetPositionAndRotation(new Vector3(_bottomWorld.x, res, _bottomWorld.z), _target.transform.rotation);
                break;
        }
    }
}
