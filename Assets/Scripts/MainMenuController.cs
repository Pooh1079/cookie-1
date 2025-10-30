using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // Нажатие кнопки Play в меню
    public void PlayGame()
    {
        int lvl = PlayerPrefs.GetInt("currentLevel", 1);
        string sceneName = "Level" + lvl;

        Debug.Log("MainMenu: currentLevel = " + lvl);
        Debug.Log("MainMenu: loading " + sceneName);

        // Проверка существования сцены
        if (!SceneExists(sceneName))
        {
            Debug.LogError("Scene " + sceneName + " does not exist!");
            return;
        }

        SceneManager.LoadScene(sceneName);
    }

    private bool SceneExists(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string currentSceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            if (currentSceneName == sceneName)
                return true;
        }
        return false;
    }

    // Сброс прогресса (опционально)
    public void ResetProgress()
    {
        PlayerPrefs.DeleteKey("currentLevel");
        Debug.Log("MainMenu: progress reset");
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}