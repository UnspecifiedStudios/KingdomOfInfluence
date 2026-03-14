using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour {

    [Header("Scene Settings")]
    public float delayBeforeLoad = 0f;

    // Singleton instance
    public static SceneManagerScript Instance { get; private set; }

    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject); // prevent duplicates
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Load scene by name
    public void LoadScene(string sceneName) {
        if (delayBeforeLoad > 0) {
            StartCoroutine(LoadSceneWithDelay(sceneName, delayBeforeLoad));
        } else {
            Debug.Log("LOADING SCENE: " + sceneName);
            SceneManager.LoadScene(sceneName);
        }
    }

    // Load scene by build index
    public void LoadScene(int sceneIndex) {
        if (delayBeforeLoad > 0) {
            StartCoroutine(LoadSceneWithDelay(sceneIndex, delayBeforeLoad));
        } else {
            SceneManager.LoadScene(sceneIndex);
        }
    }

    // Reload current scene
    public void ReloadCurrentScene() {
        LoadScene(SceneManager.GetActiveScene().name);
    }

    // Load next scene in build order
    public void LoadNextScene() {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings) {
            LoadScene(nextSceneIndex);
        } else {
            Debug.LogWarning("No next scene available!");
        }
    }

    // Load previous scene in build order
    public void LoadPreviousScene() {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int previousSceneIndex = currentSceneIndex - 1;

        if (previousSceneIndex >= 0) {
            LoadScene(previousSceneIndex);
        } else {
            Debug.LogWarning("No previous scene available!");
        }
    }

    // Quit application
    public void QuitGame() {
        Debug.Log("Quitting game...");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // Load scene with delay
    IEnumerator LoadSceneWithDelay(string sceneName, float delay) {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }

    // Load scene with delay (by index)
    IEnumerator LoadSceneWithDelay(int sceneIndex, float delay) {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneIndex);
    }

    // Load scene asynchronously with loading screen support
    public void LoadSceneAsync(string sceneName) {
        StartCoroutine(LoadSceneAsyncCoroutine(sceneName));
    }

    IEnumerator LoadSceneAsyncCoroutine(string sceneName) {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the scene fully loads
        while (!asyncLoad.isDone) {
            // You can use asyncLoad.progress to show loading bar
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            Debug.Log("Loading progress: " + (progress * 100) + "%");

            yield return null;
        }
    }
}