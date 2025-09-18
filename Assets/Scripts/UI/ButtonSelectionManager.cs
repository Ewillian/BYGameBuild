using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Manages the selection of buttons and handles navigation between them.
/// </summary>
public class ButtonSelectionManager : MonoBehaviour
{
    #region Public Fields

    /// <summary>
    /// Singleton instance of the <see cref="ButtonSelectionManager"/>.
    /// </summary>
    public static ButtonSelectionManager Instance { get; private set; }

    /// <summary>
    /// The last selected button.
    /// </summary>
    public GameObject lastSelectedButton { get; set; }

    /// <summary>
    /// The index of the last selected button.
    /// </summary>
    public int lastSelectedButtonIndex { get; set; }

    /// <summary>
    /// Array of buttons managed by this manager.
    /// </summary>
    public GameObject[] buttons;

    #endregion

    #region Private Methods
    /// <summary>
    /// Unity awake default method
    /// </summary>
    private void Awake()
    {
        SetInstance();
    }

    /// <summary>
    /// Sets the singleton instance for <see cref="ButtonSelectionManager"/>
    /// </summary>
    private void SetInstance()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    /// <summary>
    /// Called when the object becomes enabled and active.
    /// </summary>
    private void OnEnable()
    {
        StartCoroutine(SetSelectedAfterOneFrame());
        StartCoroutine(SetEndAndStartButton());
    }

    /// <summary>
    /// Updates the selected button based on input.
    /// </summary>
    private void Update()
    {
        if(InputManager.instance.MovementInput.y > 0)
        {
            if (lastSelectedButtonIndex >= 0)
            {
                HandleNextButtonSelection(-1);
            }
        }
        else if(InputManager.instance.MovementInput.y < 0)
        {
            HandleNextButtonSelection(1);
        }
    }

    /// <summary>
    /// Sets the selected button after one frame.
    /// </summary>
    private IEnumerator SetSelectedAfterOneFrame()
    {
        yield return null;
        EventSystem.current.SetSelectedGameObject(buttons[0]);
    }

    /// <summary>
    /// Sets the navigation for the first and last buttons.
    /// </summary>
    private IEnumerator SetEndAndStartButton()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            Navigation navigation = buttons[i].GetComponent<Button>().navigation;
            if(i == 0){
                navigation.selectOnUp = buttons.LastOrDefault().GetComponent<Button>();
                navigation.selectOnDown = buttons[1].GetComponent<Button>();
            } else if(i == buttons.Length - 1){
                navigation.selectOnUp = buttons[i - 1].GetComponent<Button>();
                navigation.selectOnDown = buttons.FirstOrDefault().GetComponent<Button>();
            }
            else{
                navigation.selectOnUp = buttons[i - 1].GetComponent<Button>();
                navigation.selectOnDown = buttons[i + 1].GetComponent<Button>();
            }
            buttons[i].GetComponent<Button>().navigation = navigation;
        }
        yield return null;
    }

    /// <summary>
    /// Handles the selection of the next button.
    /// </summary>
    /// <param name="addition">The index addition to the current selected button.</param>
    private void HandleNextButtonSelection(int addition)
    {
        if(EventSystem.current.currentSelectedGameObject == null && lastSelectedButton != null)
        {
            int newIndex = lastSelectedButtonIndex + addition;
            newIndex = Mathf.Clamp(newIndex, 0, buttons.Length - 1);

            if(newIndex == 0 && addition == -1)
            {
                newIndex = buttons.Length - 1;
            }
            else if(newIndex == buttons.Length - 1 && addition == 1)
            {
                newIndex = 0;
            }

            EventSystem.current.SetSelectedGameObject(buttons[newIndex]);
        }
    }
    #endregion
}
