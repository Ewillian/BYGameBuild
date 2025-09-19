using System.Collections;
using System.Collections.Generic;

using UnityEngine;

/// <summary>
/// Handles initialization and states of menu panels in the game.
/// </summary>
public class MenuInitiator : MonoBehaviour
{
    #region private variables

    /// <summary>
    /// A list of menu panels which will change states.
    /// </summary>
    [SerializeField] private List<GameObject> MenuPanels;

    /// <summary>
    /// The main panel that is displayed by default when the menu starts.
    /// </summary>
    [SerializeField] private GameObject MainPanel;
    #endregion private variables

    #region Private Methods

    /// <summary>
    /// Unity's Start coroutine. Initializes the menu when the script starts.
    /// </summary>
    private IEnumerator Start()
    {
        yield return StartCoroutine(InitiateMenu());
    }

    /// <summary>
    /// Initializes the menu by activating the main panel 
    /// and disabling all other panels.
    /// </summary>
    private IEnumerator InitiateMenu()
    {
        MainPanel.SetActive(true);

        foreach (var panel in MenuPanels)
        {
            panel.SetActive(false);
        }

        yield break;
    }

    #endregion Private Methods

    #region Public Methods

    /// <summary>
    /// Displays the specified panel while hiding all other panels.
    /// </summary>
    /// <param name="panelToActivate">The panel to display.</param>
    public void ShowMenu(GameObject panelToActivate)
    {
        if (!panelToActivate.activeSelf)
        {
            HideMenu();
            panelToActivate.SetActive(true);
        }
    }

    /// <summary>
    /// Hides all menu panels in the scene.
    /// </summary>
    public void HideMenu()
    {
        foreach (var panel in MenuPanels)
        {
            panel.SetActive(false);
        }
    }

    /// <summary>
    /// Saves player preferences, stops all coroutines, 
    /// and quits the application.
    /// </summary>
    public void QuitGame()
    {
        PlayerPrefs.Save();
        StopAllCoroutines();
        Application.Quit();
    }

    #endregion Public Methods
}
