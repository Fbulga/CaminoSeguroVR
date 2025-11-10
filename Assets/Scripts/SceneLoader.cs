using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    [Header("Loading Screen Prefab")]
    [SerializeField] private GameObject loadingScreenPrefab;

    [Header("Loading Screen Components (Auto-assigned)")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider progressSlider;
    [SerializeField] private float fillSpeed = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Instanciar el LoadingScreen una sola vez si no existe
            if (loadingScreen == null && loadingScreenPrefab != null)
            {
                GameObject instantiatedScreen = Instantiate(loadingScreenPrefab, transform);
                loadingScreen = instantiatedScreen;
                progressSlider = instantiatedScreen.GetComponentInChildren<Slider>();

                // Desactivar inicialmente
                loadingScreen.SetActive(false);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Carga una escena de forma asíncrona por su índice en build settings
    /// </summary>
    public static void LoadSceneAsync(int sceneIndex)
    {
        if (Instance == null)
        {
            Debug.LogWarning("SceneLoader no existe en la escena. Cargando escena de forma directa.");
            SceneManager.LoadScene(sceneIndex);
            return;
        }

        Instance.StartCoroutine(Instance.LoadSceneAsyncCoroutine(sceneIndex));
    }

    /// <summary>
    /// Carga una escena de forma asíncrona por su nombre
    /// </summary>
    public static void LoadSceneAsync(string sceneName)
    {
        if (Instance == null)
        {
            Debug.LogWarning("SceneLoader no existe en la escena. Cargando escena de forma directa.");
            SceneManager.LoadScene(sceneName);
            return;
        }

        Instance.StartCoroutine(Instance.LoadSceneAsyncCoroutine(sceneName));
    }

    /// <summary>
    /// Carga el siguiente nivel (escena actual + 1)
    /// </summary>
    public static void LoadNextLevel()
    {
        int nextLevel = SceneManager.GetActiveScene().buildIndex + 1;
        LoadSceneAsync(nextLevel);
    }

    private IEnumerator LoadSceneAsyncCoroutine(int sceneIndex)
    {
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(true);
        }

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            float targetFill = Mathf.Clamp01(operation.progress / 0.9f);

            if (progressSlider != null)
            {
                progressSlider.value = Mathf.MoveTowards(progressSlider.value, targetFill, fillSpeed * Time.deltaTime);
            }

            if (operation.progress >= 0.9f && (progressSlider == null || progressSlider.value >= 0.999f))
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }

        if (loadingScreen != null)
        {
            loadingScreen.SetActive(false);
        }

        if (progressSlider != null)
        {
            progressSlider.value = 0f;
        }
    }

    private IEnumerator LoadSceneAsyncCoroutine(string sceneName)
    {
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(true);
        }

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            float targetFill = Mathf.Clamp01(operation.progress / 0.9f);

            if (progressSlider != null)
            {
                progressSlider.value = Mathf.MoveTowards(progressSlider.value, targetFill, fillSpeed * Time.deltaTime);
            }

            if (operation.progress >= 0.9f && (progressSlider == null || progressSlider.value >= 0.999f))
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }

        if (loadingScreen != null)
        {
            loadingScreen.SetActive(false);
        }

        if (progressSlider != null)
        {
            progressSlider.value = 0f;
        }
    }
}
