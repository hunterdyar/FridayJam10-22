using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Variables
    #endregion

    #region Scene Values
    [Header("Scene Management")]
    //public SceneItem[] Scenes;
    [SerializeField] private string currentSceneName;
    public string CurrentSceneName { get => currentSceneName; set => currentSceneName = value; }

    [SerializeField] private string levelSceneName;
    public string LevelSceneName { get => levelSceneName; set => levelSceneName = value; }

    [SerializeField] private CanvasGroup fadeScreen;
    public CanvasGroup FadeScreen { get => fadeScreen; set => fadeScreen = value; }

    [SerializeField] private float fadeDuration;
    public float FadeDuration { get => fadeDuration; set => fadeDuration = value; }

    [SerializeField] private bool isFading;
    public bool IsFading { get => isFading; set => isFading = value; }

    #endregion

    #region Monobehaviour
    private void Start()
    {
        Debug.Log("[GAME MANAGER] Start");
    }
    #endregion


    #region Scene Management
        public void LoadScene(int sceneIndex)
    {
        if (!isFading)
        {
            StartCoroutine(FadeAndSwitchScenes(sceneIndex));
        }
    }
    
    public void LoadScene(string sceneName)
    {
        if (!isFading)
        {
            StartCoroutine(FadeAndSwitchScenes(sceneName));
        }
    }
    private IEnumerator FadeAndSwitchScenes(int sceneIndex)
    {
        yield return StartCoroutine(Fade(1));
        yield return SceneManager.LoadSceneAsync(sceneIndex);
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(Fade(0));
    }

    private IEnumerator FadeAndSwitchScenes(string sceneName)
    {
        yield return StartCoroutine(Fade(1));
        yield return SceneManager.LoadSceneAsync(sceneName);
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(Fade(0));
    }
    private IEnumerator Fade(float finalAlpha)
    {
        isFading = true;
        FadeScreen.blocksRaycasts = true; // Blocks player Clicking on other Scene or UI GameObjects
        float fadeSpeed = Mathf.Abs(FadeScreen.alpha - finalAlpha) / fadeDuration;
        while (!Mathf.Approximately(FadeScreen.alpha, finalAlpha))
        {
            FadeScreen.alpha = Mathf.MoveTowards(FadeScreen.alpha, finalAlpha,
                fadeSpeed * Time.deltaTime);
            yield return null; //Lets the Coroutine finish
        }
        isFading = false;
        FadeScreen.blocksRaycasts = false;
    }
    #endregion

    public void StartGame()
    {
        
    }

    /// <summary>
    /// Quits the application.
    /// </summary>
    public void QuitGame()
    {
        //These are called regions. When the C# compiler encounters an #if directive, 
        //  followed eventually by an #endif directive, it compiles the code between the 
        //  directives only if the specified symbol is defined. 
#if UNITY_STANDALONE //If we are running in a standalone build of the game
        //Quit the application
        Application.Quit();
#endif

        //If we are running in the editor
#if UNITY_EDITOR
        //Stop playing the scene.
        UnityEditor.EditorApplication.isPlaying = false;
#endif      
    }

}
