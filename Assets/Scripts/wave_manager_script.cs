using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class wave_manager_script : MonoBehaviour
{
    public int wave = 1;
    public TMP_Text waveText;
    public List<GameObject> enemies = new List<GameObject>();
    private GameObject setMap;
    private bool alreadySet = false;

    void Awake()
    {
        waveText.text = "Wave "+ wave;
        setMap = GameObject.Find("SetMap");
    }
    public void NewWave()
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemies.Add(enemy);
        }
        wave +=1;
        waveText.text = "Wave "+ wave;
        setMap.GetComponent<set_map_script>().SetMap();
        alreadySet = true;
    }
    void Update()
    {
        Debug.Log(enemies.Count);
        if (enemies.Count == 0 && alreadySet == false)
        {
            NewWave();
            Debug.Log("b");
        }
        if (enemies.Count > 0 && alreadySet == true)
        {
            alreadySet = false;
            Debug.Log("a");
        }
    }
}
