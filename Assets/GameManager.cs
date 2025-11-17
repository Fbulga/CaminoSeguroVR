using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int previousScene;

    [SerializeField] public int totalScore;
    [SerializeField] public int touchStreetCount;
    [SerializeField] public int dontLookSidesCount;
    [SerializeField] public int crossPedestrianCount;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);
    }
    
    public void HandleNextLevel()
    {
        previousScene = SceneManager.GetActiveScene().buildIndex;
        SceneLoader.LoadNextLevel();
    }

    public void LoadScene(int sceneIndex)
    {
        previousScene = SceneManager.GetActiveScene().buildIndex;
        SceneLoader.LoadSceneAsync(sceneIndex);
    }

    public void LoadSceneByName(string sceneName)
    {
        previousScene = SceneManager.GetActiveScene().buildIndex;
        SceneLoader.LoadSceneAsync(sceneName);
    }

    public void ReloadScene()
    {
        SceneLoader.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
}