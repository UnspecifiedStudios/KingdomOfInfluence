using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {
    public static bool GameIsPaused = false;
    public Button quitButton;
    public PlayerMovement playerMoveScript;

    public GameObject pauseMenuUI;
    public CanvasGroup canvasGroup;
    public float timeToFade = 1f;

    private bool fadeIn = false;
    private bool fadeOut = false;

    public PlayerInput playerInput;


    private void OnEnable() {
        playerInput.onActionTriggered += HandleAction;
    }

    private void OnDisable() {
        playerInput.onActionTriggered -= HandleAction;
    }

    private void Start() {
        canvasGroup.alpha = 0;
        pauseMenuUI.SetActive(false);
    }

    private void Update() {
        Debug.Log(playerInput.currentActionMap.name);
        // fading in the canvas (pause)
        if (fadeIn) {
            canvasGroup.alpha += timeToFade * Time.unscaledDeltaTime;

            if (canvasGroup.alpha >= 1f) {
                canvasGroup.alpha = 1f;
                fadeIn = false;
                quitButton.onClick.AddListener(QuitGame);
            }
        }
        // fading in the canvas (resume)
        if (fadeOut) {
            canvasGroup.alpha -= timeToFade * Time.unscaledDeltaTime;

            if (canvasGroup.alpha <= 0f) {
                canvasGroup.alpha = 0f;
                fadeOut = false;
                pauseMenuUI.SetActive(false);
            }
        }
    }

    // trigger pause input
    private void HandleAction(InputAction.CallbackContext context) {
        // check that theres a pause game
        if (context.action.name != "PauseGame") return;
        // make sure the action was onbly preformed once
        if (!context.performed) return;

        // check if it is still in animation
        if (fadeIn || fadeOut) return;

        // pause or resume game
        if (GameIsPaused)
            Resume();
        else
            Pause();
    }

    // resume game
    public void Resume() {
        fadeOut = true;
        // globasl time is normal 
        Time.timeScale = 1f;
        GameIsPaused = false;
        playerInput.SwitchCurrentActionMap("Player");
        playerInput.actions["PauseGame"].Enable();

    }

    // show pause canvas
    public void Pause() {
        pauseMenuUI.SetActive(true);
        fadeIn = true;
        // global time is stopped (all updates stop)
        Time.timeScale = 0f;
        GameIsPaused = true;
        playerInput.SwitchCurrentActionMap("UI");
        playerInput.actions["PauseGame"].Enable();
    }

    // quit application. idk if works
    public void QuitGame() {
        Debug.Log("Quitting Game");
        Application.Quit();
    }
}
