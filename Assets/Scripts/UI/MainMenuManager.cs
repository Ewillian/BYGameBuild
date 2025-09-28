using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    /// <summary>
    /// The actual displayed panel.
    /// </summary>
    private GameObject DisplayedPanel;

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

        HideMenu();

        yield break;
    }

    #endregion Private Methods

    #region Public Methods

    /// <summary>
    /// Displays the specified panel while hiding the one it replaces.
    /// </summary>
    /// <param name="panelToActivate">The panel to display.</param>
    public void ShowMenu(GameObject panelToActivate)
    {
        if (panelToActivate == null || DisplayedPanel == panelToActivate)
        {
            return;
        }

        var panelToBeFound = MenuPanels.FirstOrDefault(panel => panel == panelToActivate);

        if (DisplayedPanel != null)
        {
            DisplayedPanel.SetActive(false);
        }

        panelToBeFound.SetActive(true);

        DisplayedPanel = panelToBeFound;
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
