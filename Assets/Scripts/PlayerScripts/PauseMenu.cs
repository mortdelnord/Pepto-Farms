using System.Collections;
using System.Collections.Generic;

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
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }
    
}
