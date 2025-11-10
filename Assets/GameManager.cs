using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    
    private int previousScene;
    
    public GameObject loadingScreen;
    public Slider slider;
    [SerializeField] private float fillSpeed; 
    
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
    
    private void HandleChangeScene(int sceneIndex)
    {
        previousScene = SceneManager.GetActiveScene().buildIndex;
        loadingScreen.SetActive(true);
        StartCoroutine(LoadSceneAsync((int)sceneIndex));
    }

    public void HandleNextLevel()
    {
        int nextLevel =  SceneManager.GetActiveScene().buildIndex + 1;
        HandleChangeScene(nextLevel);
    }
    
    private IEnumerator LoadSceneAsync(int sceneId)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);
        operation.allowSceneActivation = false; 

        while (!operation.isDone)
        {
           
            float targetFill = Mathf.Clamp01(operation.progress / 0.9f);

            slider.value = Mathf.MoveTowards(slider.value, targetFill, fillSpeed * Time.deltaTime);;
            
            if (operation.progress >= 0.9f && slider.value >= 0.999f)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
        loadingScreen.SetActive(false);
        slider.value = 0f;
    }
}