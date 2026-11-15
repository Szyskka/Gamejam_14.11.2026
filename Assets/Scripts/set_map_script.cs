using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class set_map_script : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    [Header("Bounds")]
    public float yTOP = 72.5f;
    public float yBOTTOM = -72.5f;
    public float xRIGHT = 72.5f;
    public float xLEFT = 72.5f;
    [Header("Grass")]
    public int grassNum;
    public Sprite[] grassSprites;
    private GameObject player;
    private GameObject wave;
    private List<GameObject> enemies = new List<GameObject>();
    private bool already = false;
    private GameObject UI;

    void Awake()
    {
        UI = GameObject.Find("UIManager");
        wave = GameObject.Find("WaveManager");
        SetMap();
        SetGrass();
    }
    public void SetMap()
    {
        foreach(GameObject p in GameObject.FindGameObjectsWithTag("Projectile"))
        {
            Destroy(p);
        }
        wave_manager_script wave_Manager = wave.GetComponent<wave_manager_script>();
        SetPlayer();
        int i = 1+(wave_Manager.wave*2);
        SetEnemies(i);
        UI.GetComponent<UI_manager_script>().ShowWave();
    }

    void SetPlayer()
    {
        player = GameObject.Find("Player");
        if (player == null)
        {
            player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        }
        else
        {
            player.GetComponent<Rigidbody2D>().position = new Vector3(0, 0, 0);
        }
    }

    void SetEnemies(int num)
    {
        for (int i = 0; i < num; i++)
        {
            GameObject e = Instantiate(enemyPrefab);
            enemies.Add(e);
            e.name = "Enemy_" + (i+1);
        }
        foreach (GameObject e in enemies)
        {
            float posX = Random.Range(xLEFT, xRIGHT);
            float posY = Random.Range(yBOTTOM, yTOP);
            while ((posX < player.GetComponent<Rigidbody2D>().position.x+10 && posX > player.GetComponent<Rigidbody2D>().position.x-10) || (posY < player.GetComponent<Rigidbody2D>().position.y+10 && posY > player.GetComponent<Rigidbody2D>().position.y-10))
            {
                posX = Random.Range(xLEFT, xRIGHT);
                posY = Random.Range(yBOTTOM, yTOP);
            }
            e.transform.position = new Vector2 (posX, posY);
        }
    }
    public void TurnOffSound()
    {
        foreach (GameObject g in enemies)
        {
            g.GetComponent<enemy_script>().PlayerIsDead();
        }
    }
    void Update()
    {
        if (enemies.Count != GameObject.FindGameObjectsWithTag("Enemy").Count())
        {
            enemies.Clear();
            if (GameObject.FindGameObjectsWithTag("Enemy").Count() != 0)
            {
                foreach (GameObject i in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    enemies.Add(i);
                }
            }
        }
        if (enemies.Count == 0 && already)
        {
            wave.GetComponent<wave_manager_script>().wave +=1;
            SetMap();
            already = false;
        }
        if (enemies.Count>0 && !already)
        {
            already = true;
        }
    }
    private void SetGrass()
    {
        for (int i = 0; i<grassNum; i++)
        {
            GameObject imgObject = new GameObject("Grass_"+i);
            imgObject.transform.SetParent(GameObject.Find("Grass").transform);
            imgObject.AddComponent<SpriteRenderer>();
            imgObject.GetComponent<SpriteRenderer>().sprite = grassSprites[Random.Range(0, grassSprites.Count()-1)];
            float posX = Random.Range(xLEFT, xRIGHT);
            float posY = Random.Range(yBOTTOM, yTOP);
            imgObject.transform.position = new Vector3 (posX, posY, 1);
        }
    }
}
