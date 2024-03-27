using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    
    public InputActionAsset playerInputs;
    private InputAction pauseInput;
    public GameObject pauseMenu;

    [Header ("Buttons")]
    public Button saveButton;
    public Button continueButton;
    public Button mainMenuButton;
    public Button exitButton;

    private bool isPaused = false;

    private void Start()
    {
        playerInputs.FindActionMap("Player").Enable();
        pauseInput = playerInputs.FindActionMap("Player").FindAction("Pause");
        isPaused = false;
        PauseGame();
    }

    private void Update()
    {
        if (pauseInput.WasPressedThisFrame())
        {
            isPaused = !isPaused;
            PauseGame();
        }
    }

    private void PauseGame()
    {
        Debug.Log(isPaused);
        if (isPaused)
        {
            Debug.Log("pasuing");
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;

            pauseMenu.SetActive(true);
            saveButton.interactable = true;
            continueButton.interactable = true;
            exitButton.interactable = true;
            mainMenuButton.interactable = true;

        }else
        {
            Debug.Log("not Pausing");
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1;            
            pauseMenu.SetActive(false);
        }
    }

    public void SaveButton()
    {
        DataPersistenceManager.instance.SaveGame();
        saveButton.interactable = false;
    }
    public void ContinueButton()
    {
        isPaused = false;
        PauseGame();
    }
    public void MainMenuButton()
    {
        DataPersistenceManager.instance.SaveGame();
        SceneManager.LoadSceneAsync("StartScene");
    }
    public void ExitButton()
    {
        DataPersistenceManager.instance.SaveGame();
        Application.Quit();
    }
    
}
