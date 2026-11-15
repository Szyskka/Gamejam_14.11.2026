using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UI_manager_script : MonoBehaviour
{
    [Header("Pause menu")]
    public GameObject pauseMenu;
    public float popupPauseTime = 0.25f;
    [Header("Wave")]
    public float wavePopupTime = 0.25f;
    public GameObject waveShow;
    public CanvasGroup waveGroup;
    public RectTransform popupWave;
    [Header("Death")]
    public float deathPopupTime = 0.5f;
    public CanvasGroup deathGroup;
    public RectTransform popupDeath;
    private InputAction pauseAction;
    private bool pauseOpen;
    private bool canChange;
    private CanvasGroup pauseGroup;
    private RectTransform popupPause;
    private Coroutine animCoroutine;

    void Awake()
    {
        pauseGroup = pauseMenu.GetComponent<CanvasGroup>();
        popupPause = pauseMenu.GetComponent<RectTransform>();
        pauseAction = InputSystem.actions.FindAction("Pause");

    }

    void Start()
    {
        pauseGroup.alpha = 0f;
        pauseGroup.interactable = false;
        pauseGroup.blocksRaycasts = false;
        popupPause.localScale = Vector3.zero;

        waveGroup.alpha = 0f;
        waveGroup.interactable = false;
        waveGroup.blocksRaycasts = false;
        popupWave.localScale = Vector3.zero;

        deathGroup.alpha = 0f;
        deathGroup.interactable = false;
        deathGroup.blocksRaycasts = false;
        popupDeath.localScale = Vector3.zero;
    }

    void Update()
    {
        if (pauseAction.ReadValue<float>() > 0 && canChange)
        {
            if (pauseOpen)
                CloseUI();
            else
                OpenUI();

            canChange = false;
        }
        if (pauseAction.ReadValue<float>() == 0 && !canChange)
        {
            canChange = true;
        }
    }

    public void Death()
    {
        Time.timeScale = 0f;
        foreach (GameObject h in GameObject.FindGameObjectsWithTag("Heart"))
        {
            h.SetActive(false);
        }
        if (animCoroutine != null) StopCoroutine(animCoroutine);
        animCoroutine = StartCoroutine(AnimatePopup(deathPopupTime, true, popupDeath, deathGroup));
    }

    public void ShowWave()
    {
        if (animCoroutine != null) StopCoroutine(animCoroutine);
        animCoroutine = StartCoroutine(ShowWaveSequence());
    }
    
    public void ReloadScene()
    {
        Time.timeScale = 1f;

        // Reloads the active scene by its build index
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator ShowWaveSequence()
    {
        Time.timeScale = 0f;

        // 1. Wait for open animation to finish
        yield return StartCoroutine(AnimatePopup(wavePopupTime, true, popupWave, waveGroup));

        // 2. Wait 1 real-time second (works while Time.timeScale == 0)
        yield return new WaitForSecondsRealtime(1.0f);

        // 3. Wait for close animation to finish
        yield return StartCoroutine(AnimatePopup(wavePopupTime, false, popupWave, waveGroup));

        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("a");
    }

    public void CloseUI()
    {
        Time.timeScale = 1f;
        pauseOpen = false;
        if (animCoroutine != null) StopCoroutine(animCoroutine);
        animCoroutine = StartCoroutine(AnimatePopup(popupPauseTime, false, popupPause, pauseGroup));
    }

    public void OpenUI()
    {
        Time.timeScale = 0f;
        pauseOpen = true;
        if (animCoroutine != null) StopCoroutine(animCoroutine);
        animCoroutine = StartCoroutine(AnimatePopup(popupPauseTime, true, popupPause, pauseGroup));
    }

    private IEnumerator AnimatePopup(float popupTime,bool show, RectTransform popupWindow, CanvasGroup backgroundGroup)
    {
        float startAlpha = backgroundGroup.alpha;
        float targetAlpha = show ? 1f : 0f;

        Vector3 startScale = popupWindow.localScale;
        Vector3 targetScale = show ? Vector3.one : Vector3.zero;

        backgroundGroup.interactable = show;
        backgroundGroup.blocksRaycasts = show;

        float elapsed = 0f;
        while (elapsed < popupTime)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / popupTime);
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            backgroundGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, smoothT);
            popupWindow.localScale = Vector3.Lerp(startScale, targetScale, smoothT);

            yield return null;
        }

        backgroundGroup.alpha = targetAlpha;
        popupWindow.localScale = targetScale;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

}
