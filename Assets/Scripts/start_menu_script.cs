using UnityEngine;
using UnityEngine.SceneManagement;

public class start_menu_script : MonoBehaviour
{
    public void StartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("a");
    }
}
