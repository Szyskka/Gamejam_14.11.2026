using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager Instance { get; private set; }

    [System.Serializable]
    public struct LeaderboardEntry
    {
        public string playerName;
        public int waveDied;

        public LeaderboardEntry(string name, int wave)
        {
            playerName = name;
            waveDied = wave;
        }
    }

    [Header("UI Settings")]
    public TMP_FontAsset font;
    [SerializeField] private float rowHeight = 40f;
    [SerializeField] private int maxEntries = 10;

    [Header("Container Reference")]
    [Tooltip("Drag your UI Panel here")]
    [SerializeField] private Transform leaderboardContainer;

    private string cachedContainerName;
    private List<LeaderboardEntry> entries = new List<LeaderboardEntry>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            // If the new scene has its own LeaderboardManager with a dragged container, pass that reference over
            if (leaderboardContainer != null)
            {
                Instance.SetContainer(leaderboardContainer);
            }
            Destroy(gameObject);
            return;
        }

        Instance = this;
        transform.SetParent(null);
        DontDestroyOnLoad(gameObject);

        if (leaderboardContainer != null)
        {
            cachedContainerName = leaderboardContainer.name;
            ConfigureContainerLayout(leaderboardContainer.gameObject);
        }
        else
        {
            Debug.LogWarning("LeaderboardManager: Please drag a UI panel into the Leaderboard Container slot!");
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // If the previous scene's UI was destroyed, find the matching panel in the newly loaded scene
        if (leaderboardContainer == null && !string.IsNullOrEmpty(cachedContainerName))
        {
            RectTransform[] allRects = FindObjectsByType<RectTransform>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach (RectTransform rect in allRects)
            {
                if (rect.name == cachedContainerName)
                {
                    SetContainer(rect.transform);
                    break;
                }
            }
        }

        UpdateLeaderboard(entries);
    }

    public void SetContainer(Transform newContainer)
    {
        leaderboardContainer = newContainer;
        if (leaderboardContainer != null)
        {
            cachedContainerName = leaderboardContainer.name;
            ConfigureContainerLayout(leaderboardContainer.gameObject);
            UpdateLeaderboard(entries);
        }
    }

    public void OpenLeaderboard()
    {
        if (leaderboardContainer != null)
        {
            leaderboardContainer.gameObject.SetActive(true);
            UpdateLeaderboard(entries);
        }
    }

    public void UpdateLeaderboard(List<LeaderboardEntry> updatedEntries)
    {
        entries = updatedEntries
            .OrderByDescending(e => e.waveDied)
            .Take(maxEntries)
            .ToList();

        if (leaderboardContainer == null) return;

        foreach (Transform child in leaderboardContainer)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < entries.Count; i++)
        {
            CreateEntryText(i + 1, entries[i]);
        }
    }

    public void AddScoreAndRefresh(string playerName, int waveDied)
    {
        Debug.Log("b");
        entries.Add(new LeaderboardEntry(playerName, waveDied));
        UpdateLeaderboard(entries);
    }

    private void CreateEntryText(int rank, LeaderboardEntry entry)
    {
        GameObject rowObj = new GameObject($"Row_{rank}_{entry.playerName}");
        rowObj.transform.SetParent(leaderboardContainer, false);

        TextMeshProUGUI tmp = rowObj.AddComponent<TextMeshProUGUI>();
        tmp.text = $"#{rank}  {entry.playerName} — Wave {entry.waveDied}";
        tmp.fontSize = 15;
        if (font != null) tmp.font = font;
        tmp.color = Color.white;
        tmp.alignment = TextAlignmentOptions.MidlineLeft;
        tmp.textWrappingMode = TextWrappingModes.NoWrap;
        tmp.overflowMode = TextOverflowModes.Ellipsis;

        LayoutElement layoutElement = rowObj.AddComponent<LayoutElement>();
        layoutElement.preferredHeight = rowHeight;
    }

    private void ConfigureContainerLayout(GameObject containerObj)
    {
        VerticalLayoutGroup layout = containerObj.GetComponent<VerticalLayoutGroup>();
        if (layout == null)
        {
            layout = containerObj.AddComponent<VerticalLayoutGroup>();
        }

        layout.padding = new RectOffset(15, 15, 60, 15);
        layout.spacing = -5f;
        layout.childAlignment = TextAnchor.UpperCenter;
        layout.childControlWidth = true;
        layout.childControlHeight = true;
        layout.childForceExpandWidth = true;
        layout.childForceExpandHeight = false;

        if (containerObj.GetComponent<RectMask2D>() == null)
        {
            containerObj.AddComponent<RectMask2D>();
        }
    }
}