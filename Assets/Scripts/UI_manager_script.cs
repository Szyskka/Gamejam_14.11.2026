using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class UI_manager_script : MonoBehaviour
{
    [Header("Pause menu")]
    public GameObject pauseMenu;
    public float popupTime;
    private InputAction pauseAction;
    private bool pauseOpen;
    private bool canChange;
    private CanvasGroup backgroundGroup;
    private RectTransform popupWindow;
    private Coroutine animCoroutine;

    void Awake()
    {
        backgroundGroup = pauseMenu.GetComponent<CanvasGroup>();
        popupWindow = pauseMenu.GetComponent<RectTransform>();
        pauseAction = InputSystem.actions.FindAction("Pause");
    }
    void Start()
    {
        backgroundGroup.alpha = 0f;
        backgroundGroup.interactable = false;
        backgroundGroup.blocksRaycasts = false;
        popupWindow.localScale = Vector3.zero;
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
        Application.Quit();
        Debug.Log("a");
    }
    public void CloseUI()
    {
        Time.timeScale = 1;
        pauseOpen = false;
        if (animCoroutine != null) StopCoroutine(animCoroutine);
        animCoroutine = StartCoroutine(AnimatePopup(false));
    }
    public void OpenUI()
    {
        Time.timeScale = 0;
        pauseOpen = true;
        if (animCoroutine != null) StopCoroutine(animCoroutine);
        animCoroutine = StartCoroutine(AnimatePopup(true));
    }
    private IEnumerator AnimatePopup(bool show)
    {
        float startAlpha = backgroundGroup.alpha;
        float targetAlpha = show ? 1f : 0f;

        Vector3 startScale = popupWindow.localScale;
        Vector3 targetScale = show ? Vector3.one : Vector3.zero;

        // Odblokuj klikanie od razu przy otwieraniu, zablokuj przy zamykaniu
        backgroundGroup.interactable = show;
        backgroundGroup.blocksRaycasts = show;

        float elapsed = 0f;
        while (elapsed < popupTime)
        {
            // Używamy unscaledDeltaTime, ponieważ Time.timeScale wynosi 0!
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / popupTime;

            // Efekt SmoothStep sprawia, że animacja ładnie zwalnia na końcu
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            backgroundGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, smoothT);
            popupWindow.localScale = Vector3.Lerp(startScale, targetScale, smoothT);

            yield return null;
        }

        backgroundGroup.alpha = targetAlpha;
        popupWindow.localScale = targetScale;
    }
}
