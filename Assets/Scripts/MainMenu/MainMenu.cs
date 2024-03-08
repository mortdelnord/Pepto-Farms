using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenu : Menu
{
    [Header ("Menu Navigation")]

    [SerializeField] private SaveSlotsMenu saveSlotMenu;

    [SerializeField] private Button continueButton;
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button loadGameButton;


    private void Start()
    {
        if (!DataPersistenceManager.instance.HasGameData())
        {
            continueButton.interactable = false;
            loadGameButton.interactable = false;
        }
    }
    public void OnNewGameClicked()
    {
        saveSlotMenu.ActivateMenu(false);
        this.DeactivateMenu();
    }

    public void OnLoadGame()
    {
        saveSlotMenu.ActivateMenu(true);
        this.DeactivateMenu();
    }

    public void OnContinueGameClicked()
    {
        DisableMenuButtons();
        // laod the next scene - which will in turn Load the game because of
        // OnScene Loaded() in the datat persinstence manager
        SceneManager.LoadSceneAsync("TestScene");
    }

    private void DisableMenuButtons()
    {
        newGameButton.interactable = false;
        continueButton.interactable = false;
    }

    public void ActivateMenu()
    {
        this.gameObject.SetActive(true);
    }

    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }
}
