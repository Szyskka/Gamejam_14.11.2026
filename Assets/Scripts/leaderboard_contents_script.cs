using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LeaderboardManager : MonoBehaviour
{
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

    [Header("Optional: Leave empty to auto-create UI")]
    [SerializeField] private Transform leaderboardContainer;

    private List<LeaderboardEntry> entries = new List<LeaderboardEntry>();

    private void Awake()
    {
        if (leaderboardContainer == null)
        {
            CreateLeaderboardContainer();
        }
        else
        {
            ConfigureContainerLayout(leaderboardContainer.gameObject);
        }
    }

    private void Start()
    {
        // Example test data
        List<LeaderboardEntry> sampleData = new List<LeaderboardEntry>
        {
            new LeaderboardEntry("Alex", 12),
            new LeaderboardEntry("Sam", 25),
            new LeaderboardEntry("Jordan", 8)
        };

        UpdateLeaderboard(sampleData);
    }

    /// <summary>
    /// Clears existing UI rows, sorts entries by highest wave, and rebuilds the text UI.
    /// </summary>
    public void UpdateLeaderboard(List<LeaderboardEntry> updatedEntries)
    {
        // 1. Sort descending so the highest wave is at the top and cap at maxEntries
        entries = updatedEntries
            .OrderByDescending(e => e.waveDied)
            .Take(maxEntries)
            .ToList();

        // 2. Clear previous UI rows
        foreach (Transform child in leaderboardContainer)
        {
            Destroy(child.gameObject);
        }

        // 3. Create new TextMeshPro UI rows
        for (int i = 0; i < entries.Count; i++)
        {
            CreateEntryText(i + 1, entries[i]);
        }
    }

    /// <summary>
    /// Helper to add a single player score and immediately refresh the UI.
    /// </summary>
    public void AddScoreAndRefresh(string playerName, int waveDied)
    {
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

        // Tells the VerticalLayoutGroup how tall each row should be while width stretches to fit parent
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

        // Clips any rows that exceed the panel's bounds
        if (containerObj.GetComponent<RectMask2D>() == null)
        {
            containerObj.AddComponent<RectMask2D>();
        }
    }

    private void CreateLeaderboardContainer()
    {
        Canvas canvas = FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("Canvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
        }

        GameObject panelObj = new GameObject("LeaderboardContainer");
        panelObj.transform.SetParent(canvas.transform, false);

        RectTransform panelRect = panelObj.AddComponent<RectTransform>();
        panelRect.sizeDelta = new Vector2(500, 600);
        panelRect.anchoredPosition = Vector2.zero;

        ConfigureContainerLayout(panelObj);
        leaderboardContainer = panelObj.transform;
    }
}
