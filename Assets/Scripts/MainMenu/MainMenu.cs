using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private Button continueButton;
    [SerializeField] private Button newGameButton;


    private void Start()
    {
        if (!DataPersistenceManager.instance.HasGameData())
        {
            continueButton.interactable = false;
        }
    }
    public void OnNewGameClicked()
    {
        DisableMenuButtons();
        //create a new game - which will intialize our game data
        DataPersistenceManager.instance.NewGame();
        //load the gameplayscene
        SceneManager.LoadSceneAsync("TestScene");
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
}
