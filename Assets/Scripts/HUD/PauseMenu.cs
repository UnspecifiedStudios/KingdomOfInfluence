using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {
    public static bool GameIsPaused = false;
    public Button quitButton;

    public GameObject pauseMenuUI;
    public CanvasGroup canvasGroup;
    public float timeToFade = 1f;

    private bool fadeIn = false;
    private bool fadeOut = false;

    private PlayerInput playerInput;
    private InputAction pauseAction;

    private void Awake() {
        playerInput = GetComponent<PlayerInput>();
        pauseAction = playerInput.actions["PauseGame"];
    }

    private void OnEnable() {
        pauseAction.performed += OnPauseGame;
    }

    private void OnDisable() {
        pauseAction.performed -= OnPauseGame;
    }

    private void Start() {
        canvasGroup.alpha = 0;
        pauseMenuUI.SetActive(false);
    }

    private void Update() {
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
    private void OnPauseGame(InputAction.CallbackContext context) {
        Debug.Log("Pause triggered");

        // check if the game is paused or not
        if (GameIsPaused & !fadeIn & !fadeOut)
            Resume();
        else if (!fadeIn & !fadeOut)
            Pause();
    }

    // resume game
    public void Resume() {
        fadeOut = true;
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    // show pause canvas
    public void Pause() {
        pauseMenuUI.SetActive(true);
        fadeIn = true;
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    // quit application. idk if works
    public void QuitGame() {
        Debug.Log("Quitting Game");
        Application.Quit();
    }
}
