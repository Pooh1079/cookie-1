using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // ������� ������ Play � ����
    public void PlayGame()
    {
        int lvl = PlayerPrefs.GetInt("currentLevel", 1); // �� ��������� Level1
        string sceneName = "Level" + lvl;
        Debug.Log("MainMenu: loading " + sceneName);
        SceneManager.LoadScene(sceneName);
    }

    // ����� ��������� (�����������)
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