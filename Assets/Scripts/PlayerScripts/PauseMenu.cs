using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public AudioSource buttonSource;
    public InputActionAsset playerInputs;
    private InputAction pauseInput;
    public GameObject pauseMenu;

    [Header ("Buttons")]
    public Button saveButton;
    public Button continueButton;
    public Button mainMenuButton;
    public Button exitButton;

    public bool isPaused = false;
    public bool isDead = false;
    public static PauseMenu instance { get; private set; }

    private void Start()
    {
        instance = this;
        playerInputs.FindActionMap("Player").Enable();
        pauseInput = playerInputs.FindActionMap("Player").FindAction("Pause");
        isPaused = false;
        saveButton.interactable = false;
        continueButton.interactable = false;
        mainMenuButton.interactable = false;
        exitButton.interactable = false;
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

    public void PauseGame()
    {
        Debug.Log(isPaused);
        if (isPaused)
        {
            Debug.Log("pasuing");
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;

            pauseMenu.SetActive(true);
            if (!isDead)
            {
                saveButton.interactable = true;
            }else {saveButton.interactable = false;}
            continueButton.interactable = true;
            exitButton.interactable = true;
            mainMenuButton.interactable = true;

        }else
        {
            if (!isDead)
            {
                Debug.Log("not Pausing");
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Time.timeScale = 1;            
                pauseMenu.SetActive(false);

            }
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
        if (isDead)
        {
            isDead = false;
            DataPersistenceManager.instance.LoadGame();
        }
        PauseGame();
    }
    public void MainMenuButton()
    {
        isPaused = false;
        isDead = false;
        DataPersistenceManager.instance.SaveGame();
        SceneManager.LoadSceneAsync("StartScene");
    }
    public void ExitButton()
    {
        isDead = false;
        isPaused = false;
        DataPersistenceManager.instance.SaveGame();
        Application.Quit();
    }

    
    
}
