using UnityEngine;
using UnityEngine.InputSystem;

public class UI_manager_script : MonoBehaviour
{
    public GameObject pauseMenu;
    private InputAction pauseAction;
    private bool pauseOpen;
    private bool canChange;

    void Awake()
    {
        pauseAction = InputSystem.actions.FindAction("Pause");
    }
    void Update()
    {
        if (pauseAction.ReadValue<float>() > 0 && canChange == true)
        {
            if (pauseOpen)
            {
                CloseUI();
            }
            else
            {
                OpenUI();
            }
            canChange = false;
        }
        if (pauseAction.ReadValue<float>() == 0 && canChange == false)
        {
            canChange = true;
        }
    }
    public void QuitGame () 
    {
        Application.Quit ();
        Debug.Log("a");
    }
    public void CloseUI()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        pauseOpen = false;
    }
    public void OpenUI()
    {
        Time.timeScale = 0;
        pauseOpen = true;
        pauseMenu.SetActive(true);
    }
}
