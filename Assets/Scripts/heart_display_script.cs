using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class heart_display_script : MonoBehaviour
{
    private int maxHealth;
    public int currentHealth;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public float distance;
    private List<GameObject> hearts = new List<GameObject>();
    private GameObject player;
    private bool dieSoundPlayed = false;
    private GameObject leaderboard;
    private GameObject wave;
    public string playerName;

    void Start()
    {
        leaderboard = GameObject.Find("UpdateLeadetboard");
        wave = GameObject.Find("WaveManager");
        player = GameObject.Find("Player");
        maxHealth = player.GetComponent<player_script>().health;
        currentHealth = maxHealth+1;
        for (int i = 0; i<maxHealth; i++)
        {
            GameObject imgObject = new GameObject("Heart_"+i);
            imgObject.tag = "Heart";
            hearts.Add(imgObject);
            imgObject.transform.SetParent(gameObject.transform.parent, false);
            Image newImage = imgObject.AddComponent<Image>();
            newImage.sprite = fullHeart;
        }
        UpdateHearts();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void UpdateHearts()
    {
        currentHealth = Mathf.Clamp(currentHealth-1, 0, maxHealth);
        for (int i = 0; i<currentHealth; i++)
        {
            hearts[i].GetComponent<RectTransform>().localScale = new Vector3(0.25f, 0.25f, 0.25f);
            hearts[i].GetComponent<RectTransform>().position = new Vector3(GetComponent<RectTransform>().position.x + i*distance, GetComponent<RectTransform>().position.y , 0);
            hearts[i].GetComponent<Image>().sprite = fullHeart;
        }
        for (int i = 0; i<maxHealth-currentHealth; i++)
        {
            hearts[i+currentHealth].GetComponent<RectTransform>().localScale = new Vector3(0.25f, 0.25f, 0.25f);
            hearts[i+currentHealth].GetComponent<RectTransform>().position = new Vector3(GetComponent<RectTransform>().position.x + (i+currentHealth)*distance, GetComponent<RectTransform>().position.y , 0);
            hearts[i+currentHealth].GetComponent<Image>().sprite = emptyHeart;
        }
    }
    void Update()
    {
        if (currentHealth == 0)
        {
            if (!dieSoundPlayed)
            {
                sound_script.Instance.PlayDieSound();
                dieSoundPlayed = true;
            }
            GameObject.Find("SetMap").GetComponent<set_map_script>().TurnOffSound();
            GameObject.Find("UIManager").GetComponent<UI_manager_script>().Death();
        }
    }
    public void SetPlayerName(string name)
    {
        playerName = name;
        Debug.Log(name);
        leaderboard.GetComponent<LeaderboardManager>().AddScoreAndRefresh(playerName, wave.GetComponent<wave_manager_script>().wave);
    }
}
